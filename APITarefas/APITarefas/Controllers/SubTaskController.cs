using APITarefas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APITarefas.Controllers
{
    [Route("api/subtasks")]
    [ApiController]
    public class SubTaskController : ControllerBase
    {
        DbTasksContext _ctx = new DbTasksContext();

        [HttpGet]
        public ActionResult Get([FromQuery] int taskId) {

            try
            {
                var subtasks = _ctx.SubTasks.Where(s => s.TaskId == taskId).ToList();
                return Ok(subtasks);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("{id:int}")]
        public ActionResult Post(int id, [FromBody] SubTask sub)
        {
            try
            {
                _ctx.SubTasks.Add(sub);
                _ctx.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public ActionResult StatusSubtarefa(int id, [FromBody] SubTask sub)
        {
            var subTarefa = _ctx.SubTasks.FirstOrDefault(s => s.Id == id);

            if (subTarefa == null)
            {
                return NotFound("SubTarefa não encontrada");
            }
            try
            {
                
                    subTarefa.Concluida = sub.Concluida;
            
                _ctx.SaveChanges();
                return Ok("Status atualizado com sucesso");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno ao editar a tarefa.");
            }
        }

    }
}
