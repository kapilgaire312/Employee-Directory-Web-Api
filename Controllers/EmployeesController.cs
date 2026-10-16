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

    [HttpPost("add")] // POST /api/employees/add
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

    [HttpGet("{id}")] //GET /api/employees/id
    public ActionResult<EmployeeResponse> GetEmployeeById([FromRoute] string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("Employee ID is required.");
        }
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

    [HttpGet()] // GET /api/employees
    public ActionResult<List<EmployeeResponse>> ListAllEmployees()
    {
        var employees = _employeeDirectory.ListEmployee();

        List<EmployeeResponse> employeeListToReturn = employees
            .Select(e => new EmployeeResponse(e.Id, e.Name, e.Department))
            .ToList();
        return employeeListToReturn;
    }

    [HttpDelete("delete/{id}")]
    public IActionResult DeleteEmployee(string id)
    {
        if (String.IsNullOrWhiteSpace(id))
        {
            return BadRequest("Employee id is required.");
        }

        bool employeeExist = _employeeDirectory.RemoveEmployee(id);

        if (employeeExist)
        {
            return NoContent();
        }
        return NotFound();
    }
}
