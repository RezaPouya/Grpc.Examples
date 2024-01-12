using System.ComponentModel.DataAnnotations;

namespace Asp.Server.GrpcClientServices.Dtos;

public class ToDoItemOutpDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get;  set; }
}
