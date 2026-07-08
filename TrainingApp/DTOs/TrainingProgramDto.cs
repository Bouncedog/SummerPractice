namespace TrainingApp.DTOs;

/// <summary>
/// DTO для передачи данных о тренировочной программе.
/// </summary>
public class TrainingProgramDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public bool IsActive { get; set; }
}