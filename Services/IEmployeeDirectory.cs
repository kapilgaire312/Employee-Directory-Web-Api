using EmployeeDirWeb.Dtos;
using EmployeeDirWeb.Models;

public interface IEmployeeDirectory
{
    (bool, string) AddEmployee(AddEmployeeCommand employeeToAdd);

    IReadOnlyCollection<EmployeeInfo> ListEmployee();

    (bool, EmployeeInfo?, string?) SearchEmployee(string? id);

    (bool, EmployeeInfo?, string?) RemoveEmployee(string? id);
}
