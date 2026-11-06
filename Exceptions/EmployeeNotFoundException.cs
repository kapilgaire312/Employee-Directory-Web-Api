namespace EmployeeDirWeb.Exceptions;

class EmployeeNotFoundException(string message) : Exception(message), IEmployeeException
{
    public int StatusCode => StatusCodes.Status404NotFound;
}
