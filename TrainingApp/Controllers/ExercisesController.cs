using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingApp.Data;
using TrainingApp.DTOs;
using TrainingApp.Models;

namespace TrainingApp.Controllers
{
    /// <summary>
    /// Возвращает упражнения текущего пользователя.
    /// Можно фильтровать по программе и признаку активности.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ExercisesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExercisesController(ApplicationDbContext context)
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
        /// Возвращает список упражнений текущего пользователя.
        /// Можно отфильтровать упражнения по программе и активности.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExerciseDto>>> GetExercises(
            [FromQuery] int? programId = null,
            [FromQuery] bool? isActive = null,
            [FromQuery] bool? includeDeleted = null)
        {
            var userId = GetUserId();
            var query = _context.Exercises
                .Include(e => e.Program)
                .Where(e => e.UserId == userId);

            if (includeDeleted != true)
                query = query.Where(e => !e.IsDeleted);

            if (programId.HasValue)
                query = query.Where(e => e.ProgramId == programId.Value);

            if (isActive.HasValue)
                query = query.Where(e => e.IsActive == isActive.Value);

            var exercises = await query
                .Select(e => new ExerciseDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    ProgramId = e.ProgramId,
                    IsActive = e.IsActive,
                    ProgramName = e.Program.Name,
                    IsDeleted = e.IsDeleted
                })
                .ToListAsync();

            return Ok(exercises);
        }

        /// <summary>
        /// Возвращает упражнение текущего пользователя по идентификатору.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ExerciseDto>> GetExercise(int id)
        {
            var userId = GetUserId();
            var exercise = await _context.Exercises
                .Include(e => e.Program)
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId && !e.IsDeleted);

            if (exercise == null)
                return NotFound();

            return Ok(new ExerciseDto
            {
                Id = exercise.Id,
                Name = exercise.Name,
                ProgramId = exercise.ProgramId,
                IsActive = exercise.IsActive,
                ProgramName = exercise.Program.Name,
                IsDeleted = exercise.IsDeleted
            });
        }

        /// <summary>
        /// Создаёт новое упражнение в тренировочной программе.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ExerciseDto>> CreateExercise(CreateExerciseDto dto)
        {
            var userId = GetUserId();

            var program = await _context.TrainingPrograms
                .FirstOrDefaultAsync(p => p.Id == dto.ProgramId && p.UserId == userId);
            if (program == null)
                return BadRequest("Указанная программа не существует или не принадлежит вам.");

            var exercise = new Exercise
            {
                Name = dto.Name,
                ProgramId = dto.ProgramId,
                IsActive = dto.IsActive,
                IsDeleted = false,
                UserId = userId
            };

            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();

            var result = new ExerciseDto
            {
                Id = exercise.Id,
                Name = exercise.Name,
                ProgramId = exercise.ProgramId,
                IsActive = exercise.IsActive,
                ProgramName = program.Name
            };

            return CreatedAtAction(nameof(GetExercise), new { id = exercise.Id }, result);
        }

        /// <summary>
        /// Обновляет упражнение текущего пользователя.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExercise(int id, CreateExerciseDto dto)
        {
            var userId = GetUserId();
            var exercise = await _context.Exercises
                .Include(e => e.Program)
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId && !e.IsDeleted);  // ←

            if (exercise == null)
                return NotFound();

            var program = await _context.TrainingPrograms
                .FirstOrDefaultAsync(p => p.Id == dto.ProgramId && p.UserId == userId);
            if (program == null)
                return BadRequest("Указанная программа не существует или не принадлежит вам.");

            exercise.Name = dto.Name;
            exercise.ProgramId = dto.ProgramId;
            exercise.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Помечает упражнение текущего пользователя как удалённое (архив).
        /// Упражнение скрывается из списков, но история активностей сохраняется.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercise(int id)
        {
            var userId = GetUserId();

            var exercise = await _context.Exercises
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (exercise == null)
                return NotFound();

            exercise.IsDeleted = true;
            exercise.IsActive = false;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}