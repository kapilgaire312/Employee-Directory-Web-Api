namespace EmployeeDirWeb.Exceptions;

class EmployeeNotFoundException : Exception
{
    public EmployeeNotFoundException(string message)
        : base(message) { }
}
