using System.Runtime.InteropServices.JavaScript;
using EmployeeManager.Models;

namespace EmployeeManager.Services
{
    public class EmployeeService
    {
        private readonly AppDbContext _context;

        public EmployeeService(AppDbContext context)
        {
            _context = context;
        }
        public List<Employee> GetAllEmployees()
        {
            return _context.Employees.ToList();
        }
        public Employee CreatEmployee(Employee employee)
        {
            if(employee == null)
                _context.Employees.Add(employee);
            _context.SaveChanges();
            return employee;
        }

        public bool DeleteEmployee(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null) return false;
            _context.Employees.Remove(employee);
            _context.SaveChanges();
            return true;
        }
        public List<object> GetDepartmentStats()
        {
            return _context.Employees
                .GroupBy(e => e.Department)
                .Select(g => new
                {
                    Department = g.Key,
                    EmployeeCount = g.Count(),
                    AvgSalary = g.Average(e => e.Salary)
                })
                .ToList()
                .Cast<object>()
                .ToList();
        }
    }
}
