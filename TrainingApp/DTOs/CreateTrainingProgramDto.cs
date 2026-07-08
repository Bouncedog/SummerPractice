using System.ComponentModel.DataAnnotations;

namespace TrainingApp.DTOs;

/// <summary>
/// DTO для создания новой тренировочной программы.
/// </summary>
public class CreateTrainingProgramDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(50)]
    public string Type { get; set; }

    public bool IsActive { get; set; } = true;
}