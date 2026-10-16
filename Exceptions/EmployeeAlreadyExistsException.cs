namespace EmployeeDirWeb.Exceptions;

class EmployeeAlreadyExistsException : Exception
{
    public EmployeeAlreadyExistsException(string message)
        : base(message) { }
}
