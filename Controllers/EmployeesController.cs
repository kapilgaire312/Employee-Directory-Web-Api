using EmployeeDirWeb.Dtos;
using EmployeeDirWeb.Dtos.Api;
using EmployeeDirWeb.Exceptions;
using EmployeeDirWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDirWeb.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class EmployeesController(IEmployeeService employeeService) : ControllerBase
{
    [HttpPost()] // POST /api/employees
    public async Task<ActionResult<EmployeeResponse>> AddEmployee(
        [FromBody] AddEmployeeRequest employee,
        CancellationToken ct
    )
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
            var newEmployee = await employeeService.AddEmployeeAsync(employeeToAdd, ct);

            //using extension mehtod to create response
            var addedEmployee = newEmployee.ToResponse();

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
    public async Task<ActionResult<EmployeeResponse>> GetEmployeeById(
        [FromRoute] string id,
        CancellationToken ct
    )
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("Employee ID is required.");
        }
        try
        {
            var employee = await employeeService.GetEmployeeByIdAsync(id, ct);

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
    public async Task<IActionResult> DeleteEmployee(string id, CancellationToken ct)
    {
        if (String.IsNullOrWhiteSpace(id))
        {
            return BadRequest("Employee id is required.");
        }

        await employeeService.RemoveEmployeeAsync(id, ct);

        return NoContent();
    }

    [HttpPut("{id}")] //PUT /api/employees/id
    public async Task<IActionResult> EditEmployee(
        [FromRoute] string id,
        [FromBody] EditEmployeeRequest employee,
        CancellationToken ct
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
            EmployeeInfo updatedEmployee = await employeeService.EditEmployeeAsync(
                id,
                employeeToEdit,
                ct
            );

            // using the extension method to create the employee response instead of manaully creating the object.
            var employeeResponse = updatedEmployee.ToResponse();
            return Ok(employeeResponse);
        }
        catch (EmployeeNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet()] //GET: /api/employees?name=kap&department=it
    public async Task<IActionResult> SearchEmployees(
        [FromQuery] string? name,
        [FromQuery] string? department,
        CancellationToken ct
    )
    {
        var foundEmployees = await employeeService.SearchEmployeesAsync(name, department, ct);

        List<EmployeeResponse> employeesListToSend = foundEmployees
            .Select(e => new EmployeeResponse(e.Id, e.Name, e.Department))
            .ToList();

        return Ok(employeesListToSend);
    }
}
