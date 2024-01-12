using Microsoft.EntityFrameworkCore;
using Grpc.Server1.Data;
using Grpc.Server1.Models;

namespace Grpc.Server1.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ToDoItem> ToDoItems => Set<ToDoItem>();
}