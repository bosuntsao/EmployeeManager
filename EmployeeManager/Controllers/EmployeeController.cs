using EmployeeManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.Controllers
{
    // Controllers/EmployeeController.cs
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var employees = _context.Employees
                .Where(e=>e.Department == "IT")
                .OrderBy(e=>e.Salary)
                .ToList();
            return View(employees);
        }

        public IActionResult Stats()
        {
            var stats = _context.Employees
                .GroupBy(e => e.Department)
                .Select(g=>new { Department = g.Key, AvgSalary = g.Average(e => e.Salary) })
                .ToList();
            return Json(stats);
        }

        [HttpGet]
        public IActionResult Create() => View();
                
        [HttpPost]
        public IActionResult Create([FromBody] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();
                return Json(new { success = true, message = "員工新增成功！", data = employee });
            }
            return Json(new { success = false, message = "輸入資料有誤，請檢查" });
        }
    }
}
