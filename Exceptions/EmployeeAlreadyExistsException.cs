namespace EmployeeDirWeb.Exceptions;

class EmployeeAlreadyExistsException(string message) : Exception(message), IEmployeeException
{
    public int StatusCode => StatusCodes.Status409Conflict;
}
