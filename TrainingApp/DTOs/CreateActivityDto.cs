using System.ComponentModel.DataAnnotations;

namespace TrainingApp.DTOs;

/// <summary>
/// DTO для создания новой активности.
/// </summary>
public class CreateActivityDto
{
    [Required]
    public int ExerciseId { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    [Range(1, 1440)]
    public int Minutes { get; set; }

    [StringLength(200)]
    public string Notes { get; set; }
}