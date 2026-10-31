using EmployeeDirWeb.Dtos;
using EmployeeDirWeb.Exceptions;
using EmployeeDirWeb.Models;

public class EmployeeService(IEmployeeDirectory employeeDirectory) : IEmployeeService
{
    public async Task<EmployeeInfo> AddEmployeeAsync(
        AddEmployeeCommand employeeToAdd,
        CancellationToken ct
    )
    {
        EmployeeInfo newEmployee = new(
            employeeToAdd.Id,
            employeeToAdd.Name,
            employeeToAdd.Department,
            employeeToAdd.Salary,
            employeeToAdd.HiredDate
        );

        var addedEmployee = await employeeDirectory.AddEmployeeAsync(newEmployee, ct);

        return addedEmployee;
    }

    public async Task<EmployeeInfo> GetEmployeeByIdAsync(string id, CancellationToken ct)
    {
        var employee = await employeeDirectory.GetEmployeeUsingIdAsync(id, ct);
        if (employee is null)
        {
            throw new EmployeeNotFoundException($"Employee with id {id} doesn't exist.");
        }
        return employee;
    }

    public async Task RemoveEmployeeAsync(string id, CancellationToken ct)
    {
        var result = await employeeDirectory.RemoveEmployeeAsync(id, ct);
        if (!result)
        {
            throw new EmployeeNotFoundException($"Employee with id {id} doesn't exist.");
        }
    }

    public async Task<EmployeeInfo> EditEmployeeAsync(
        string id,
        AddEmployeeCommand employeeData,
        CancellationToken ct
    )
    {
        EmployeeInfo employeeToAdd = new(
            id,
            employeeData.Name,
            employeeData.Department,
            employeeData.Salary,
            employeeData.HiredDate
        );

        var updatedEmployee = await employeeDirectory.EditEmployeeAsync(id, employeeToAdd, ct);

        //if no emloyee is found
        if (updatedEmployee is null)
        {
            throw new EmployeeNotFoundException($"Employee with id {id} not found.");
        }

        return updatedEmployee;
    }

    public async Task<IReadOnlyCollection<EmployeeInfo>> SearchEmployeesAsync(
        string? name,
        string? department,
        CancellationToken ct
    )
    {
        var employees = await employeeDirectory.SearchEmployeesAsync(name, department, ct);
        return employees;
    }
}
