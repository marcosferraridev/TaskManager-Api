namespace TaskManagerAPI.Models;

public class TaskItem
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; } // ? indica que a propriedade pode ser nula

    public bool Completed { get; set; }
}