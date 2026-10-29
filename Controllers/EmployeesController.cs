using EmployeeDirWeb.Dtos;
using EmployeeDirWeb.Dtos.Api;
using EmployeeDirWeb.Exceptions;
using EmployeeDirWeb.Models;
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

    [HttpPost()] // POST /api/employees
    public ActionResult<EmployeeResponse> AddEmployee([FromBody] AddEmployeeRequest employee)
    {
        // here we need to perform this check because salary is deciaml? and HiredDate is DateOnly?
        // this is to remove compiler error as we know that these value are validated already with [Required] attribute
        //
        // the check was removed and null forgiving operator was used as we know the null values are already filtered wit [Required]

        AddEmployeeCommand employeeToAdd = new(
            employee.Id,
            employee.Name,
            employee.Department,
            (decimal)employee.Salary!,
            (DateOnly)employee.HiredDate!
        );

        //TODO: later implement a exception handler middleware to handle exceptions there instead of controllers.
        try
        {
            var newEmployee = _employeeDirectory.AddEmployee(employeeToAdd);

            var addedEmployee = new EmployeeResponse(
                newEmployee.Id,
                newEmployee.Name,
                newEmployee.Department
            );

            return CreatedAtAction(
                nameof(GetEmployeeById),
                new { id = employee.Id },
                addedEmployee
            );
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

    [HttpDelete("{id}")] //DELETE /api/employees/id
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

    [HttpPut("{id}")] //PUT /api/employees/id
    public IActionResult EditEmployee(
        [FromRoute] string id,
        [FromBody] EditEmployeeRequest employee
    )
    {
        if (String.IsNullOrWhiteSpace(id))
        {
            return BadRequest("Employee Id is required.");
        }

        AddEmployeeCommand employeeToEdit = new AddEmployeeCommand(
            id,
            employee.Name,
            employee.Department,
            (decimal)employee.Salary!,
            (DateOnly)employee.HiredDate!
        );

        try
        {
            EmployeeInfo updatedEmployee = _employeeDirectory.EditEmployee(id, employeeToEdit);

            var employeeResponse = new EmployeeResponse(
                updatedEmployee.Id,
                updatedEmployee.Name,
                updatedEmployee.Department
            );

            return Ok(employeeResponse);
        }
        catch (EmployeeNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet()] //GET: /api/employees?name=kap&department=it
    public IActionResult SearchEmployees([FromQuery] string? name, [FromQuery] string? department)
    {
        List<EmployeeInfo> foundEmployees = _employeeDirectory.SearchEmployees(name, department);

        List<EmployeeResponse> employeesListToSend = foundEmployees
            .Select(e => new EmployeeResponse(e.Id, e.Name, e.Department))
            .ToList();

        return Ok(employeesListToSend);
    }
}
