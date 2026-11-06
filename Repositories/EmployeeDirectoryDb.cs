namespace EmployeeDirWeb.Repositories;

using EmployeeDirWeb.Data;
using EmployeeDirWeb.Exceptions;
using EmployeeDirWeb.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

public class EmployeeDirectoryDb(EmployeeDirectoryContext dbContext) : IEmployeeDirectory
{
    public async Task<EmployeeInfo> AddEmployeeAsync(
        EmployeeInfo employeeToAdd,
        CancellationToken ct
    )
    {
        dbContext.EmployeeInfos.Add(employeeToAdd);
        try
        {
            await dbContext.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            if (ex.GetBaseException() is PostgresException pgEx && pgEx.SqlState == "23505")
            {
                throw new EmployeeAlreadyExistsException(
                    $"Employee with id {employeeToAdd.Id} already exists."
                );
            }
        }
        return employeeToAdd;
    }

    public async Task<EmployeeInfo?> GetEmployeeUsingIdAsync(string id, CancellationToken ct)
    {
        var res = await dbContext.EmployeeInfos.FindAsync(id, ct);
        return res;
    }

    public async Task<IReadOnlyCollection<EmployeeInfo>> SearchEmployeesAsync(
        string? name,
        string? department,
        CancellationToken ct
    )
    {
        var employees = await dbContext
            .EmployeeInfos.AsNoTracking()
            .Where(e =>
                EF.Functions.ILike(e.Name, $"%{name}%")
                && EF.Functions.ILike(e.Department, $"%{department}%")
            )
            .ToListAsync(ct);

        return employees.AsReadOnly();
    }

    public async Task<bool> RemoveEmployeeAsync(string id, CancellationToken ct)
    {
        var res = await dbContext.EmployeeInfos.FindAsync(id, ct);
        if (res is null)
        {
            return false;
        }
        dbContext.EmployeeInfos.Remove(res);
        await dbContext.SaveChangesAsync(ct);
        return true;
    }

    public async Task<EmployeeInfo?> EditEmployeeAsync(
        string id,
        EmployeeInfo employeeData,
        CancellationToken ct
    )
    {
        var oldEmployee = await dbContext.EmployeeInfos.FindAsync(id, ct);

        if (oldEmployee is null)
        {
            return null;
        }

        dbContext.Entry(oldEmployee).CurrentValues.SetValues(employeeData);

        await dbContext.SaveChangesAsync(ct);

        return employeeData;
    }
}
