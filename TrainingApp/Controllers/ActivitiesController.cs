using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingApp.Data;
using TrainingApp.DTOs;
using TrainingApp.Models;

namespace TrainingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ActivitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GetUserId()
        {
            if (Request.Headers.TryGetValue("X-UserId", out var userId))
                return userId.ToString();

            throw new InvalidOperationException("Заголовок X-UserId не найден.");
        }

        /// <summary>
        /// Возвращает все активности текущего пользователя.
        /// Можно передать startDate и endDate для фильтрации по диапазону дат.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActivityDto>>> GetActivities(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var userId = GetUserId();
            var query = _context.Activities
                .Include(a => a.Exercise)
                .Where(a => a.UserId == userId);

            if (startDate.HasValue)
                query = query.Where(a => a.Date >= startDate.Value.Date);

            if (endDate.HasValue)
                query = query.Where(a => a.Date <= endDate.Value.Date);

            var activities = await query
                .Select(a => new ActivityDto
                {
                    Id = a.Id,
                    ExerciseId = a.ExerciseId,
                    Date = a.Date,
                    Minutes = a.Minutes,
                    Notes = a.Notes,
                    ExerciseName = a.Exercise.Name
                })
                .OrderByDescending(a => a.Date)
                .ThenBy(a => a.Id)
                .ToListAsync();

            return Ok(activities);
        }

        /// <summary>
        /// Возвращает активности и общую длительность за указанный день.
        /// </summary>
        [HttpGet("day")]
        public async Task<ActionResult<object>> GetDayActivities([FromQuery] DateTime date)
        {
            var userId = GetUserId();
            var day = date.Date;
            var activities = await _context.Activities
                .Include(a => a.Exercise)
                .Where(a => a.Date == day && a.UserId == userId)
                .Select(a => new ActivityDto
                {
                    Id = a.Id,
                    ExerciseId = a.ExerciseId,
                    Date = a.Date,
                    Minutes = a.Minutes,
                    Notes = a.Notes,
                    ExerciseName = a.Exercise.Name
                })
                .ToListAsync();

            var totalMinutes = activities.Sum(a => a.Minutes);

            return Ok(new
            {
                Date = day,
                TotalMinutes = totalMinutes,
                Activities = activities
            });
        }

        /// <summary>
        /// Возвращает все активности текущего пользователя за указанный месяц.
        /// </summary>
        [HttpGet("month")]
        public async Task<ActionResult<IEnumerable<ActivityDto>>> GetMonthActivities(
            [FromQuery] int year,
            [FromQuery] int month)
        {
            var userId = GetUserId();
            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1).AddDays(-1);

            var activities = await _context.Activities
                .Include(a => a.Exercise)
                .Where(a => a.Date >= start && a.Date <= end && a.UserId == userId)
                .Select(a => new ActivityDto
                {
                    Id = a.Id,
                    ExerciseId = a.ExerciseId,
                    Date = a.Date,
                    Minutes = a.Minutes,
                    Notes = a.Notes,
                    ExerciseName = a.Exercise.Name
                })
                .OrderByDescending(a => a.Date)
                .ThenBy(a => a.Id)
                .ToListAsync();

            return Ok(activities);
        }

        /// <summary>
        /// Создаёт новую активность для активного упражнения.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ActivityDto>> CreateActivity(CreateActivityDto dto)
        {
            var userId = GetUserId();

            var exercise = await _context.Exercises
                .FirstOrDefaultAsync(e => e.Id == dto.ExerciseId && e.UserId == userId);
            if (exercise == null)
                return BadRequest("Упражнение не найдено или не принадлежит вам.");
            if (!exercise.IsActive)
                return BadRequest("Нельзя выбрать неактивное упражнение.");

            var day = dto.Date.Date;
            var existingMinutes = await _context.Activities
                .Where(a => a.Date == day && a.UserId == userId)
                .SumAsync(a => a.Minutes);

            if (existingMinutes + dto.Minutes > 1440)
                return BadRequest("Суммарная длительность активностей за день не может превышать 1440 минут.");

            var activity = new Activity
            {
                ExerciseId = dto.ExerciseId,
                Date = day,
                Minutes = dto.Minutes,
                Notes = dto.Notes,
                UserId = userId
            };

            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();

            var result = new ActivityDto
            {
                Id = activity.Id,
                ExerciseId = activity.ExerciseId,
                Date = activity.Date,
                Minutes = activity.Minutes,
                Notes = activity.Notes,
                ExerciseName = exercise.Name
            };

            return CreatedAtAction(nameof(GetActivity), new { id = activity.Id }, result);
        }

        /// <summary>
        /// Возвращает активность текущего пользователя по идентификатору.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityDto>> GetActivity(int id)
        {
            var userId = GetUserId();
            var activity = await _context.Activities
                .Include(a => a.Exercise)
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (activity == null)
                return NotFound();

            return Ok(new ActivityDto
            {
                Id = activity.Id,
                ExerciseId = activity.ExerciseId,
                Date = activity.Date,
                Minutes = activity.Minutes,
                Notes = activity.Notes,
                ExerciseName = activity.Exercise.Name
            });
        }

        /// <summary>
        /// Обновляет активность текущего пользователя.
        /// Если исходное упражнение стало неактивным, изменить упражнение нельзя.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateActivity(int id, UpdateActivityDto dto)
        {
            var userId = GetUserId();
            var activity = await _context.Activities
                .Include(a => a.Exercise)
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (activity == null)
                return NotFound();

            // Проверка: если текущее упражнение неактивно, запрещаем менять упражнение
            if (dto.ExerciseId != activity.ExerciseId)
            {
                if (!activity.Exercise.IsActive)
                    return BadRequest("Нельзя изменить упражнение, так как текущее упражнение неактивно.");

                var newExercise = await _context.Exercises
                    .FirstOrDefaultAsync(e => e.Id == dto.ExerciseId && e.UserId == userId);
                if (newExercise == null)
                    return BadRequest("Новое упражнение не найдено или не принадлежит вам.");
                if (!newExercise.IsActive)
                    return BadRequest("Новое упражнение должно быть активным.");
            }

            // Проверка лимита дня
            var newDate = dto.Date.Date;
            var sumWithoutCurrent = await _context.Activities
                .Where(a => a.Date == newDate && a.Id != id && a.UserId == userId)
                .SumAsync(a => a.Minutes);

            if (sumWithoutCurrent + dto.Minutes > 1440)
                return BadRequest("Суммарная длительность активностей за день не может превышать 1440 минут.");

            activity.ExerciseId = dto.ExerciseId;
            activity.Date = newDate;
            activity.Minutes = dto.Minutes;
            activity.Notes = dto.Notes;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Удаляет активность текущего пользователя.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(int id)
        {
            var userId = GetUserId();
            var activity = await _context.Activities
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (activity == null)
                return NotFound();

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}