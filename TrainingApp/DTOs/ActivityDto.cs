namespace TrainingApp.DTOs;

/// <summary>
/// DTO для передачи данных об активности.
/// </summary>
public class ActivityDto
{
    public int Id { get; set; }
    public int ExerciseId { get; set; }
    public DateTime Date { get; set; }
    public int Minutes { get; set; }
    public string Notes { get; set; }
    public string ExerciseName { get; set; }
}