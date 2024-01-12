using System.ComponentModel.DataAnnotations;

namespace Grpc.Server1.Models;

public class ToDoItem
{
    public ToDoItem(string title, string description)
    {
        Title = title;
        Description = description;
        Status = "New";
    }
    
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; private set; }
}
