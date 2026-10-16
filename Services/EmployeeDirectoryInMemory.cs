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

    public IReadOnlyCollection<EmployeeInfo> ListEmployee()
    {
        return _employeesList.AsReadOnly();
    }

    public (bool, EmployeeInfo?, string?) RemoveEmployee(string? id)
    {
        if (String.IsNullOrWhiteSpace(id))
        {
            return (false, null, "Enter a valid employee id to remove.");
        }

        //the list should be iterated backward for removing on the same list
        //but since we only remove one item. it wont break.
        for (int i = 0; i < _employeesList.Count; i++)
        {
            if (_employeesList[i].Id == id)
            {
                EmployeeInfo removedEmployee = _employeesList[i];
                _employeesList.RemoveAt(i);
                return (true, removedEmployee, null);
            }
        }
        throw new Exception($"Employee with id {id} not found.");
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
