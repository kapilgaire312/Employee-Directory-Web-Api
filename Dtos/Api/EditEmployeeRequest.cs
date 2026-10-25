using System.ComponentModel.DataAnnotations;

namespace EmployeeDirWeb.Dtos.Api;

public record EditEmployeeRequest(
    [Required] [Length(3, 50)] string Name,
    [Required] [Length(3, 40)] string Department,
    [Required] [Range(0, 1000000000)] decimal? Salary,
    [Required] DateOnly? HiredDate
);
