namespace EmployeeDirWeb.Repositories;

using EmployeeDirWeb.Models;

public interface IEmployeeDirectory
{
    public Task<EmployeeInfo> AddEmployeeAsync(EmployeeInfo employeeToAdd, CancellationToken ct);

    public Task<EmployeeInfo?> GetEmployeeUsingIdAsync(string id, CancellationToken ct);

    public Task<IReadOnlyCollection<EmployeeInfo>> SearchEmployeesAsync(
        string? name,
        string? department,
        CancellationToken ct
    );

    public Task<bool> RemoveEmployeeAsync(string id, CancellationToken ct);

    public Task<EmployeeInfo?> EditEmployeeAsync(
        string id,
        EmployeeInfo employeeData,
        CancellationToken ct
    );
}
