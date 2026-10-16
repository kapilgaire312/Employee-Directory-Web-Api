using EmployeeDirWeb.Dtos;
using EmployeeDirWeb.Dtos.Api;
using EmployeeDirWeb.Exceptions;
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
    public ActionResult<EmployeeResponse> AddEmployee([FromBody] AddEmployeeRequest employee)
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

        //TODO: later implement a exception handler middleware to handle exceptions there instead of controllers.
        try
        {
            var newEmployee = _employeeDirectory.AddEmployee(employeeToAdd);

            return new EmployeeResponse(newEmployee.Id, newEmployee.Name, newEmployee.Department);
        }
        catch (EmployeeAlreadyExistsException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public ActionResult<EmployeeResponse> GetEmployeeById([FromRoute] string id)
    {
        try
        {
            var employee = _employeeDirectory.GetEmployeeUsingId(id);

            var foundEmployee = new EmployeeResponse(
                employee.Id,
                employee.Name,
                employee.Department
            );

            return foundEmployee;
        }
        catch (EmployeeNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public IActionResult CheckHealth()
    {
        return Ok();
    }
}
