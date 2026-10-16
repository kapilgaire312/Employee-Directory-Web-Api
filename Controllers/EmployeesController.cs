using EmployeeDirWeb.Dtos;
using EmployeeDirWeb.Dtos.Api;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDirWeb.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeDirectory _employeeDirectory;

    public EmployeesController(IEmployeeDirectory employeeDirectory)
    {
        _employeeDirectory = employeeDirectory;
    }

    [HttpPost("add")]
    public ActionResult<AddEmployeeResponse> AddEmployee([FromBody] AddEmployeeRequest employee)
    {
        Console.WriteLine(employee.Salary);
        Console.WriteLine(employee.HiredDate);

        // here we need to perform this check because salary is deciaml? and HiredDate is DateOnly?
        // this is to remove compiler error as we know that these value are validated already with [Required] attribute

        if (employee.Salary is not decimal esalary || employee.HiredDate is not DateOnly dateHired)
        {
            return NotFound();
        }

        AddEmployeeCommand employeeToAdd = new(
            employee.Id,
            employee.Name,
            employee.Department,
            esalary,
            dateHired
        );

        _employeeDirectory.AddEmployee(employeeToAdd);

        return new AddEmployeeResponse(
            employeeToAdd.Id,
            employeeToAdd.Name,
            employeeToAdd.Department
        );
    }

    [HttpGet]
    public IActionResult CheckHealth()
    {
        return Ok();
    }
}
