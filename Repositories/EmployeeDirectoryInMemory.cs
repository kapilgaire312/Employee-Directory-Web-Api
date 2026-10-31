using EmployeeDirWeb.Dtos;
using EmployeeDirWeb.Dtos.Api;
using EmployeeDirWeb.Exceptions;
using EmployeeDirWeb.Models;

class EmployeeDirectoryInMemory(List<EmployeeInfo> employeesList) : IEmployeeDirectory
{
    public async Task<EmployeeInfo> AddEmployeeAsync(
        EmployeeInfo employeeToAdd,
        CancellationToken ct
    )
    {
        //throw exception if ct is cancelled
        //but this is not mush useful for inmemory implementation as it is synchronous.
        //so ignoring it in inmemory is fine, but we do need it for implementing database
        ct.ThrowIfCancellationRequested();

        //check id to ensure it is unique
        foreach (var employeeInfo in employeesList)
        {
            if (employeeInfo.Id == employeeToAdd.Id)
            {
                throw new EmployeeAlreadyExistsException(
                    $"Employee with {employeeToAdd.Id} already exists."
                );
            }
        }

        employeesList.Add(employeeToAdd);

        return employeeToAdd;
    }

    public async Task<EmployeeInfo?> GetEmployeeUsingIdAsync(string id, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        var employee = employeesList.Find(e => e.Id == id);

        return employee;
    }

    public async Task<bool> RemoveEmployeeAsync(string id, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        var employee = employeesList.FirstOrDefault(e => e.Id == id);

        if (employee is null)
        {
            return false;
        }
        employeesList.Remove(employee);
        return true;
    }

    public async Task<EmployeeInfo?> EditEmployeeAsync(
        string id,
        EmployeeInfo employeeData,
        CancellationToken ct
    )
    {
        ct.ThrowIfCancellationRequested();
        // find index of employee in the directory
        int index = employeesList.FindIndex(e => e.Id == id);

        //if no emloyee is found
        if (index == -1)
        {
            return null;
            throw new EmployeeNotFoundException($"Employee with id {id} not found.");
        }

        EmployeeInfo employeeToAdd = new(
            employeeData.Id,
            employeeData.Name,
            employeeData.Department,
            employeeData.Salary,
            employeeData.HiredDate
        );

        //Replace the old employee data with new
        employeesList[index] = employeeToAdd;

        return employeeToAdd;
    }

    public async Task<IReadOnlyCollection<EmployeeInfo>> SearchEmployeesAsync(
        string? name,
        string? department,
        CancellationToken ct
    )
    {
        ct.ThrowIfCancellationRequested();

        List<EmployeeInfo> foundEmployees = employeesList
            .Where(e =>
                e.Name.Contains(name ?? "", StringComparison.OrdinalIgnoreCase)
                && e.Department.Contains(department ?? "", StringComparison.OrdinalIgnoreCase)
            )
            .ToList();

        return foundEmployees.AsReadOnly();
    }
}
