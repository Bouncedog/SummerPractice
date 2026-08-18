using CRUDrequestsTask1.Models;

namespace CRUDrequestsTask1.Data;

public class EfEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    public EfEmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Создаёт работника.
    /// </summary>
    /// <param name="employee">работник.</param>
    public void Create(Employee employee)
    {
        _context.Employees.Add(employee);
        _context.SaveChanges();
    }

    /// <summary>
    /// Получает список всех имеющихся работников.
    /// </summary>
    public List<Employee> GetAll()
    {
        return _context.Employees.ToList();
    }

    /// <summary>
    /// Обновляет информацию об работнике.
    /// </summary>
    /// <param name="employee">работник.</param>
    public void Update(Employee employee)
    {
        _context.Employees.Update(employee);
        _context.SaveChanges();
    }

    /// <summary>
    /// Удаляет работника по ID.
    /// </summary>
    /// <param name="id"> id работника.</param>
    public void Delete(int id)
    {
        var emp = _context.Employees.Find(id);
        if (emp != null)
        {
            _context.Employees.Remove(emp);
            _context.SaveChanges();
        }
    }
}