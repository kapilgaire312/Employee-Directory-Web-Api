using EmployeeDirWeb.Dtos;
using EmployeeDirWeb.Models;

public interface IEmployeeDirectory
{
    EmployeeInfo AddEmployee(AddEmployeeCommand employeeToAdd);

    public EmployeeInfo GetEmployeeUsingId(string id);

    IReadOnlyCollection<EmployeeInfo> ListEmployee();

    (bool, EmployeeInfo?, string?) SearchEmployee(string? id);

    bool RemoveEmployee(string? id);

    public EmployeeInfo EditEmployee(string id, AddEmployeeCommand employeeData);
}
