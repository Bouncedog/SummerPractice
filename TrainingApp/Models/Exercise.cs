namespace TrainingApp.Models;

/// <summary>
/// Упражнение, относящееся к определённой программе.
/// </summary>
public class Exercise : BaseEntity
{
    public string Name { get; set; }
    public int ProgramId { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public TrainingProgram Program { get; set; }
    public ICollection<Activity> Activities { get; set; }
}