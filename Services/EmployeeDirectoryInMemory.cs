using EmployeeDirWeb.Models;

class EmployeeDirectoryInMemory : IEmployeeDirectory
{
    private readonly List<EmployeeInfo> _employeesList;

    public EmployeeDirectoryInMemory(List<EmployeeInfo> employeesList)
    {
        _employeesList = employeesList;
    }

    public (bool, string) AddEmployee(
        string? id,
        string? name,
        string? department,
        decimal? salary,
        DateOnly hiredDate
    )
    {
        if (String.IsNullOrWhiteSpace(id))
        {
            return (false, "Enter a valid Id.");
        }

        if (String.IsNullOrWhiteSpace(name))
        {
            return (false, "Enter a valid Name.");
        }
        if (String.IsNullOrWhiteSpace(department))
        {
            return (false, "Enter a valid Department.");
        }

        if (!salary.HasValue || salary < 0)
        {
            return (false, "Enter a valid salary.");
        }

        //check id to ensure it is unique
        foreach (var employeeInfo in _employeesList)
        {
            if (employeeInfo.Id == id)
            {
                return (false, $"Employee with {id} already exists.");
            }
        }

        decimal nsalary = salary ?? 0;
        EmployeeInfo newEmployee = new(id, name, department, nsalary, hiredDate);

        _employeesList.Add(newEmployee);

        return (true, $"Employee with id {id} was added successfully.");
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
        throw new EmployeeNotFoundException($"Employee with id {id} not found.");
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

        throw new EmployeeNotFoundException($"Employee with id {id} not found.");
    }
}
