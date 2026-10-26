using EmployeeDirWeb.Dtos;
using EmployeeDirWeb.Models;

public interface IEmployeeDirectory
{
    EmployeeInfo AddEmployee(AddEmployeeCommand employeeToAdd);

    public EmployeeInfo GetEmployeeUsingId(string id);

    public List<EmployeeInfo> SearchEmployees(string? name, string? department);

    bool RemoveEmployee(string? id);

    public EmployeeInfo EditEmployee(string id, AddEmployeeCommand employeeData);
}
