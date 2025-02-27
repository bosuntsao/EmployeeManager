using EmployeeManager.Models;
using EmployeeManager.Services;
using Microsoft.AspNetCore.Mvc;

public class EmployeeController : Controller
{
    private readonly EmployeeService _employeeService;

    // 透過依賴注入接收 EmployeeService
    public EmployeeController(EmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    // 顯示員工清單頁面
    public IActionResult Index()
    {
        var employees = _employeeService.GetAllEmployees(); // 從 Service 獲取員工列表
        return View(employees);
    }

    // 新增員工 (POST)
    [HttpPost]
    public IActionResult Create([FromBody] Employee employee)
    {
        try
        {
            var createdEmployee = _employeeService.CreatEmployee(employee);
            return Json(new
            {
                success = true,
                message = "員工新增成功！",
                data = new
                {
                    createdEmployee.Id,
                    createdEmployee.Name,
                    createdEmployee.Department,
                    createdEmployee.Salary
                }
            });
        }
        catch (ArgumentException ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "新增失敗，請稍後再試" });
        }
    }

    // 刪除員工 (DELETE)
    [HttpDelete]
    public IActionResult Delete(int id)
    {
        var success = _employeeService.DeleteEmployee(id);
        if (success)
            return Json(new { success = true, message = "員工已刪除", id = id });
        return Json(new { success = false, message = "員工不存在" });
    }

    // 查詢部門統計 (GET)
    [HttpGet]
    public IActionResult Stats()
    {
        try
        {
            var stats = _employeeService.GetDepartmentStats();
            return Json(new { success = true, data = stats });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "查詢失敗: " + ex.Message });
        }
    }
}