using EmployeeDirWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeDirWeb.Data;

public class EmployeeDirectoryContext(DbContextOptions<EmployeeDirectoryContext> options)
    : DbContext(options)
{
    public DbSet<EmployeeInfo> EmployeeInfos { get; set; }
}
