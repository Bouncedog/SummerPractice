namespace TrainingApp.Models;

/// <summary>
/// Тренировочная программа.
/// </summary>
public class TrainingProgram : BaseEntity
{
    public string Name { get; set; }
    public string Type { get; set; }
    public bool IsActive { get; set; }
    public ICollection<Exercise> Exercises { get; set; }
}