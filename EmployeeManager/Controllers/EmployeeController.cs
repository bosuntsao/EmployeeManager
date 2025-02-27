using EmployeeManager.Models;
using EmployeeManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.Controllers
{
    // Controllers/EmployeeController.cs
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public IActionResult Index()
        {
            var employees = _employeeService.GetAllEmployees();
            return View(employees);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Employee employee)
        {
            try
            {
                var createdEmployee = _employeeService.CreatEmployee(employee);
                return Json(new { success = true, message = "員工新增成功！", data = new { employee.Id, employee.Name, employee.Department, employee.Salary } });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "輸入資料有誤，請檢查" });
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var success = _employeeService.DeleteEmployee(id);
            if (success)
            {
                return Json(new { success = true, message = "員工已刪除" });                
            }
            return Json(new { success = false, message = "員工不存在" });
        }
    }
}
