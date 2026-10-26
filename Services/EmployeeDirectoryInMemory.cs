using EmployeeDirWeb.Dtos;
using EmployeeDirWeb.Dtos.Api;
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

    public EmployeeInfo EditEmployee(string id, AddEmployeeCommand employeeData)
    {
        EmployeeInfo? employee = _employeesList.FirstOrDefault(e => e.Id == id);

        if (employee is null)
        {
            throw new EmployeeNotFoundException($"Employee with id {id} not found.");
        }

        EmployeeInfo employeeToAdd = new(
            employeeData.Id,
            employeeData.Name,
            employeeData.Department,
            employeeData.Salary,
            employeeData.HiredDate
        );

        //Remove existing employee
        _employeesList.Remove(employee);

        //Add employee with updated data
        _employeesList.Add(employeeToAdd);

        return employeeToAdd;
    }

    public List<EmployeeInfo> SearchEmployees(string? name, string? department)
    {
        Console.WriteLine(name);
        Console.WriteLine(department);

        string nameInLowercase = name?.ToLower() ?? string.Empty;
        string departmentInLowercase = department?.ToLower() ?? string.Empty;
        List<EmployeeInfo> foundEmployees = _employeesList
            .Where(e =>
                e.Name.ToLower().Contains(nameInLowercase)
                && e.Department.ToLower().Contains(departmentInLowercase)
            )
            .ToList();

        return foundEmployees;
    }
}
