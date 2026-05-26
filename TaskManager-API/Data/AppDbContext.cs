using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Data;

public class AppDbContext : DbContext
{
    public DbSet<TaskItem> Tasks { get; set; } // representa a coleção de tarefas no banco de dados

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) // chama o construtor da classe base DbContext para configurar o contexto com as opções fornecidas
    {

    }
}

