namespace TrainingApp.DTOs;

/// <summary>
/// DTO для передачи данных об упражнении.
/// </summary>
public class ExerciseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ProgramId { get; set; }
    public bool IsActive { get; set; }
    public string ProgramName { get; set; }   //Для отображения
}