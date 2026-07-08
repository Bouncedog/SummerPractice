namespace TrainingApp.Models;

/// <summary>
/// Запись о выполненной тренировке (активность).
/// </summary>
public class Activity : BaseEntity
{
    public int ExerciseId { get; set; }
    public DateTime Date { get; set; }
    public int Minutes { get; set; }
    public string Notes { get; set; }
    public Exercise Exercise { get; set; }
}