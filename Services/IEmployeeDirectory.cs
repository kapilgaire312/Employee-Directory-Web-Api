using EmployeeDirWeb.Models;

public interface IEmployeeDirectory
{
    (bool, string) AddEmployee(
        string? id,
        string? name,
        string? department,
        decimal? salary,
        DateOnly hiredDate
    );

    IReadOnlyCollection<EmployeeInfo> ListEmployee();

    (bool, EmployeeInfo?, string?) SearchEmployee(string? id);

    (bool, EmployeeInfo?, string?) RemoveEmployee(string? id);
}
