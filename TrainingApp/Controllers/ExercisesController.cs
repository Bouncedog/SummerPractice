using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingApp.Data;
using TrainingApp.DTOs;
using TrainingApp.Models;

namespace TrainingApp.Controllers
{
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

#if DEBUG
            return "dev-user";
#else
    return "default";
#endif
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExerciseDto>>> GetExercises(
            [FromQuery] int? programId = null,
            [FromQuery] bool? isActive = null)
        {
            var userId = GetUserId();
            var query = _context.Exercises
                .Include(e => e.Program)
                .Where(e => e.UserId == userId);

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
                    ProgramName = e.Program.Name
                })
                .ToListAsync();

            return Ok(exercises);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExerciseDto>> GetExercise(int id)
        {
            var userId = GetUserId();
            var exercise = await _context.Exercises
                .Include(e => e.Program)
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (exercise == null)
                return NotFound();

            return Ok(new ExerciseDto
            {
                Id = exercise.Id,
                Name = exercise.Name,
                ProgramId = exercise.ProgramId,
                IsActive = exercise.IsActive,
                ProgramName = exercise.Program.Name
            });
        }

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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExercise(int id, CreateExerciseDto dto)
        {
            var userId = GetUserId();
            var exercise = await _context.Exercises
                .Include(e => e.Program)
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercise(int id)
        {
            var userId = GetUserId();
            var exercise = await _context.Exercises
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (exercise == null)
                return NotFound();

            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}