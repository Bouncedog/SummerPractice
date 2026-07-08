using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingApp.Data;
using TrainingApp.DTOs;
using TrainingApp.Models;

namespace TrainingApp.Controllers
{
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgram(int id)
        {
            var userId = GetUserId();
            var program = await _context.TrainingPrograms
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (program == null)
                return NotFound();

            _context.TrainingPrograms.Remove(program);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}