using EmployeeDirWeb.Models;

namespace EmployeeDirWeb.Dtos.Api;

public static class EmployeeMappingExtensions
{
    // creating a extension method here, which is like adding a method ToResponse to the employeeInfo object
    // so instead of changing employee response in every api method when we are calling new EmployeeResponse() inside each of them,
    // we can simply change it here in one place.
    //
    // so, when we do employee.ToResponse(),
    // the compiler is actually doing:
    // EmployeeMappingExtensions.ToResponse(employee)
    public static EmployeeResponse ToResponse(this EmployeeInfo employee)
    {
        return new EmployeeResponse(employee.Id, employee.Name, employee.Department);
    }
}
