using EmployeeDirWeb.Dtos;
using EmployeeDirWeb.Models;

public interface IEmployeeService
{
    public Task<EmployeeInfo> AddEmployeeAsync(
        AddEmployeeCommand employeeToAdd,
        CancellationToken ct
    );

    public Task<EmployeeInfo> GetEmployeeByIdAsync(string id, CancellationToken ct);

    public Task RemoveEmployeeAsync(string id, CancellationToken ct);

    public Task<EmployeeInfo> EditEmployeeAsync(
        string id,
        AddEmployeeCommand employeeData,
        CancellationToken ct
    );

    public Task<IReadOnlyCollection<EmployeeInfo>> SearchEmployeesAsync(
        string? name,
        string? department,
        CancellationToken ct
    );
}
