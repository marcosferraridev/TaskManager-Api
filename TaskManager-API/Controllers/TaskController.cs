using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Models;
using TaskManagerAPI.Data;

namespace TaskManagerAPI.Controllers;

// funcionalidade de API Controller, que inclui validação automática de modelo e formatação de resposta
[ApiController]
// rota base para acessar os endpoints do controlador, ex: /tasks para obter a lista de tarefas
[Route("tasks")] 

public class TaskController : ControllerBase
{
    // campo para acessar o contexto do banco de dados; readonly para garantir que só possa ser atribuído no construtor
    private readonly AppDbContext _context; 

    // construtor para injetar o contexto do banco de dados
    public TaskController(AppDbContext context) 
    {
        _context = context;
    }

    [HttpGet] 
    public IActionResult GetTasks()
    {
        // retorna o status 200 OK e a lista de tarefas convertida para uma lista em memória usando ToList()
        return Ok(_context.Tasks.ToList()); 
    }

    [HttpPost] 
    public IActionResult CreateTask(TaskItem task)
    {
        if (task == null)
        {
            // retorna o status 400 Bad Request 
            return BadRequest("A tarefa não pode ser nula!");
        }

        if (string.IsNullOrWhiteSpace(task.Title))
        {
            return BadRequest("É necessário informar o título da tarefa!");
        }

        // ?. só acessa Length se Description não for nulo
        if (task.Description?.Length > 600) 
        {
            return BadRequest("A descrição da tarefa deve ter no máximo 600 caracteres!");
        }

        // toda nova tarefa inicia como não concluída
        task.Completed = false; 

        _context.Tasks.Add(task);
        _context.SaveChanges();

        // retorna o status 201 Created e a tarefa criada
        return Created("", task); 
    }

    // acesso via GET para obter uma tarefa específica pelo ID, ex: /tasks/{id}
    [HttpGet("{id}")]  
    public IActionResult GetTaskById(int id)
    {
        // firstOrDefault retorna a primeira tarefa que corresponde ao ID ou null se não encontrar
        var task = _context.Tasks.FirstOrDefault(t => t.Id == id);

        if (task == null) 
        {
            // retorna o status 404 Not Found
            return NotFound();
        }

        return Ok(task);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTaskById(int id) 
    {
        var task = _context.Tasks.FirstOrDefault(t => t.Id == id);

        if (task == null)
        {
            return NotFound();
        }

        _context.Tasks.Remove(task);
        _context.SaveChanges();

        // retorna o status 204 No Content para indicar que a tarefa foi deletada com sucesso, sem retornar um corpo de resposta
        return NoContent(); 

    }

    [HttpPut("{id}")]
    public IActionResult UpdateTaskById(int id, TaskItem updatedTask) 
    {
        var task = _context.Tasks.FirstOrDefault(t => t.Id == id);

        if (task == null)
        {
            return NotFound();
        }

        if (String.IsNullOrWhiteSpace(updatedTask.Title))
        {
            return BadRequest("É necessário informar o título da tarefa!");
        }

        if (updatedTask.Description?.Length > 600)
        {
            return BadRequest("A descrição da tarefa deve ter no máximo 600 caracteres!");
        }

        task.Title = updatedTask.Title;
        task.Completed = updatedTask.Completed;
        task.Description = updatedTask.Description;
        // o id não é atualizado para manter a integridade do banco de dados
        _context.SaveChanges();

        return NoContent();
    }
}

