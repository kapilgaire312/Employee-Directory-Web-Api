using EmployeeDirWeb.Dtos;
using EmployeeDirWeb.Exceptions;
using EmployeeDirWeb.Models;

class EmployeeDirectoryInMemory : IEmployeeDirectory
{
    private readonly List<EmployeeInfo> _employeesList;

    public EmployeeDirectoryInMemory()
    {
        _employeesList = new();
    }

    public EmployeeInfo AddEmployee(AddEmployeeCommand employeeToAdd)
    {
        //check id to ensure it is unique
        foreach (var employeeInfo in _employeesList)
        {
            if (employeeInfo.Id == employeeToAdd.Id)
            {
                throw new EmployeeAlreadyExistsException(
                    $"Employee with {employeeToAdd.Id} already exists."
                );
            }
        }

        EmployeeInfo newEmployee = new(
            employeeToAdd.Id,
            employeeToAdd.Name,
            employeeToAdd.Department,
            employeeToAdd.Salary,
            employeeToAdd.HiredDate
        );

        _employeesList.Add(newEmployee);

        return newEmployee;
    }

    public EmployeeInfo GetEmployeeUsingId(string id)
    {
        var employee = _employeesList.Find(e => e.Id == id);

        if (employee is null)
        {
            throw new EmployeeNotFoundException($"Employee with id {id} doesn't exist.");
        }

        return employee;
    }

    public IReadOnlyCollection<EmployeeInfo> ListEmployee()
    {
        return _employeesList.AsReadOnly();
    }

    public bool RemoveEmployee(string? id)
    {
        var employee = _employeesList.FirstOrDefault(e => e.Id == id);

        if (employee is null)
        {
            return false;
        }
        _employeesList.Remove(employee);
        return true;
    }

    public (bool, EmployeeInfo?, string?) SearchEmployee(string? id)
    {
        if (String.IsNullOrWhiteSpace(id))
        {
            return (false, null, "Enter a valid Id.");
        }

        foreach (var employeeInfo in _employeesList)
        {
            if (employeeInfo.Id == id)
            {
                return (true, employeeInfo, null);
            }
        }

        throw new Exception($"Employee with id {id} not found.");
    }
}
