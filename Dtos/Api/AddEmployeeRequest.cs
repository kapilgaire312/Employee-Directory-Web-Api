using System.ComponentModel.DataAnnotations;

namespace EmployeeDirWeb.Dtos.Api;

public record AddEmployeeRequest(
    [Required] string Id,
    [Required] [Length(3, 50)] string Name,
    [Required] [Length(3, 40)] string Department,
    [Required] [Range(0, 1000000000)] decimal? Salary,
    [Required] DateOnly? HiredDate
);

/*
 * Here the Salary is decimal? instead of decimal because
 * as the Request json is parsed, if there is no field for the salary,
 * then it will autpmaticlly put Salary = 0 ,(its default value, since value type cannot be null.)
 * which will pass the validation as the range is (0,...)
 * so we use decimal?, meaning the value can be null.
 * so when parsing if the salary is not found, then it becomes Salary= null .
 * and when the validation is done (Required), it finds the value to be null and rejects it.
 * sending error message to the client.
 *
 * as for why not using string?, if the value of say Name is not present, it can be null(since it is a reference type),
 * so can be filtered by the validation.
 *
 * and for DateOnly if, it is not present it defaults to 0001-01-01
 *
 */
