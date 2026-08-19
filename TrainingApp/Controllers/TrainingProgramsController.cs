using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingApp.Data;
using TrainingApp.DTOs;
using TrainingApp.Models;

namespace TrainingApp.Controllers
{
    /// <summary>
    /// Возвращает список тренировочных программ текущего пользователя.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingProgramsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TrainingProgramsController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GetUserId()
        {
            if (Request.Headers.TryGetValue("X-UserId", out var userId))
                return userId.ToString();

#if DEBUG
            return "dev-user";
#else
    return "default";
#endif
        }

        /// <summary>
        /// Возвращает список тренировочных программ текущего пользователя.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingProgramDto>>> GetPrograms()
        {
            var userId = GetUserId();
            var programs = await _context.TrainingPrograms
                .Where(p => p.UserId == userId)
                .Select(p => new TrainingProgramDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Type = p.Type,
                    IsActive = p.IsActive
                })
                .ToListAsync();

            return Ok(programs);
        }

        /// <summary>
        /// Возвращает тренировочную программу текущего пользователя по идентификатору.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingProgramDto>> GetProgram(int id)
        {
            var userId = GetUserId();
            var program = await _context.TrainingPrograms
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (program == null)
                return NotFound();

            return Ok(new TrainingProgramDto
            {
                Id = program.Id,
                Name = program.Name,
                Type = program.Type,
                IsActive = program.IsActive
            });
        }

        /// <summary>
        /// Создаёт новую тренировочную программу.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TrainingProgramDto>> CreateProgram(CreateTrainingProgramDto dto)
        {
            var userId = GetUserId();
            var program = new TrainingProgram
            {
                Name = dto.Name,
                Type = dto.Type,
                IsActive = dto.IsActive,
                UserId = userId
            };

            _context.TrainingPrograms.Add(program);
            await _context.SaveChangesAsync();

            var result = new TrainingProgramDto
            {
                Id = program.Id,
                Name = program.Name,
                Type = program.Type,
                IsActive = program.IsActive
            };

            return CreatedAtAction(nameof(GetProgram), new { id = program.Id }, result);
        }

        /// <summary>
        /// Обновляет тренировочную программу текущего пользователя.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProgram(int id, CreateTrainingProgramDto dto)
        {
            var userId = GetUserId();
            var program = await _context.TrainingPrograms
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (program == null)
                return NotFound();

            program.Name = dto.Name;
            program.Type = dto.Type;
            program.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Удаляет тренировочную программу текущего пользователя.
        /// Нельзя удалить программу, если в ней есть упражнения.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgram(int id)
        {
            var userId = GetUserId();

            var program = await _context.TrainingPrograms
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (program == null)
                return NotFound();

            var hasExercises = await _context.Exercises
                .AnyAsync(e => e.ProgramId == id && e.UserId == userId);

            if (hasExercises)
            {
                return BadRequest(
                    "Нельзя удалить программу, пока в ней есть упражнения.");
            }

            _context.TrainingPrograms.Remove(program);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}