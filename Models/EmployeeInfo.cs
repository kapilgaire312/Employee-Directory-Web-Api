namespace EmployeeDirWeb.Models;

public class EmployeeInfo
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Department { get; init; }
    public DateOnly HiredDate { get; init; }

    private decimal _salary;
    public decimal Salary
    {
        get { return _salary; }
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Salary cannot be negative.");
            }
            _salary = value;
        }
    }

    public EmployeeInfo(
        string id,
        string name,
        string department,
        decimal salary,
        DateOnly hiredDate
    )
    {
        Id = id;
        Name = name;
        Department = department;
        Salary = salary;
        HiredDate = hiredDate;
    }
}
