using System.ComponentModel.DataAnnotations;

namespace TrainingApp.DTOs;

/// <summary>
/// DTO для создания нового упражнения.
/// </summary>
public class CreateExerciseDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    public int ProgramId { get; set; }

    public bool IsActive { get; set; } = true;
}