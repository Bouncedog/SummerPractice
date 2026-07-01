using CRUDrequestsTask1.Models;

namespace CRUDrequestsTask1.Data
{
    public class EfEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EfEmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // CREATE
        public void Create(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        // READ: все
        public List<Employee> GetAll()
        {
            return _context.Employees.ToList();
        }

        // UPDATE
        public void Update(Employee employee)
        {
            _context.Employees.Update(employee);
            _context.SaveChanges();
        }

        // DELETE
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
}