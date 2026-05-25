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

            if(task == null)
            {
                return NotFound();
            }
            tasks.Remove(task);
            return NoContent(); // retorna o status 204 No Content para indicar que a tarefa foi deletada com sucesso, sem retornar nenhum conteúdo na resposta

    }
}

