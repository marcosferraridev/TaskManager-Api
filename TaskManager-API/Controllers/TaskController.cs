using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers;

[ApiController]
[Route("tasks")] // rota para acessar as tarefas

public class TaskController : ControllerBase
{
    private static List<TaskItem> tasks = new();

    [HttpGet] // acesso via GET para obter a lista de tarefas
    public IActionResult GetAction()
    {
        return Ok(tasks); // retorna o status 200 OK e a lista de tarefas no corpo da resposta
    }

    [HttpPost] // acesso via POST para criar uma nova tarefa
    public IActionResult CreateTask(TaskItem task)
    {
        if (task == null)
        {
            return BadRequest("A tarefa não pode ser nula!");
        }
        if (string.IsNullOrWhiteSpace(task.Title))
        {
            return BadRequest("É necessário informar o título da tarefa!");
        }
        if (task.Description?.Length > 600) // ?. só acessa Length se Description não for nulo
        {
            return BadRequest("A descrição da tarefa deve ter no máximo 600 caracteres!");
        }
        tasks.Add(task);
        return Created("", task); // retorna o status 201 Created e a tarefa criada
    }

    [HttpGet("{id}")]  // acesso via GET para obter uma tarefa específica pelo ID, ex: /tasks/{id}
    public IActionResult GetTaskById(int id)
    {
        // firstOrDefault retorna a primeira tarefa que corresponde ao ID ou null se não encontrar
        // t é a variável de iteração que representa cada tarefa na lista tasks
        var task = tasks.FirstOrDefault(t => t.Id == id);

        if (task == null) // verificação
        {
            return NotFound();
        }
        return Ok(task);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTaskById(int id) // logica parecida com a de GetTaskById, mas para deletar a tarefa
    {
        var task = tasks.FirstOrDefault(t => t.Id == id);

        if (task == null)
        {
            return NotFound();
        }
        tasks.Remove(task);
        return NoContent(); // retorna o status 204 No Content para indicar que a tarefa foi deletada com sucesso, sem retornar nenhum conteúdo na resposta

    }

    [HttpPut("{id}")]
    public IActionResult UpdateTaskById(int id, TaskItem updatedTask) // logica parecida com a de GetTaskById, mas para atualizar a tarefa
    {
        var task = tasks.FirstOrDefault(t => t.Id == id);

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
        // o id normalmente não se altera por ser unico
        return NoContent();
    }
}

