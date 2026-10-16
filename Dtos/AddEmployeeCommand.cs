namespace EmployeeDirWeb.Dtos;

public record AddEmployeeCommand(
    string Id,
    string Name,
    string Department,
    decimal Salary,
    DateOnly HiredDate
);
