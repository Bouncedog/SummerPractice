namespace TrainingApp.Models;

/// <summary>
/// Базовый класс для всех сущностей, содержащий общие свойства.
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; set; }
    public string UserId { get; set; } //Для изоляции
}