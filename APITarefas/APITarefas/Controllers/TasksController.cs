using APITarefas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APITarefas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Tasks: ControllerBase
    {
        //private readonly DbTasksContext ctx;

        //public Tasks(DbTasksContext ctx) {

        //    this.ctx = ctx;
        //}


        DbTasksContext _ctx = new DbTasksContext();


        [HttpGet]
        public IActionResult ListarTarefas() {

            var lista = _ctx.Tasks;

            try
            {
                return Ok(lista);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);

            }
        }

        [HttpPost]
        public IActionResult CriarTarefa([FromBody] Models.Task task)
        {
            try
            {
                task.Concluida = false;

                _ctx.Add(task);
                _ctx.SaveChanges();
                return Ok("Tarefa criada com sucesso");
            }
            catch (Exception ex) {

                return BadRequest($"{ex.Message} Tarefa não criada");
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeletarTarefa(int id)
        {
            var tarefa = _ctx.Tasks.Include(b => b.SubTasks).FirstOrDefault(b => b.Id == id);

            if (tarefa == null)
            {
                return NotFound();
            }
            try
            {
                foreach (var item in tarefa.SubTasks.ToList())
                {
                    var subTasks = _ctx.SubTasks.Find(item.Id);

                    _ctx.Remove(subTasks);
                    _ctx.SaveChanges();
                }

                _ctx.Remove(tarefa);  
                _ctx.SaveChanges();
                return Ok("Tarefa deletada com sucesso");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno ao tentar deletar a tarefa.");
            }
        }
        [HttpPut("{id:int}")]
        public IActionResult StatusTarefa(int id, [FromBody] Models.Task task)
        {
            var tarefaBanco = _ctx.Tasks.Include(b => b.SubTasks).FirstOrDefault(b => b.Id == id);

            if (tarefaBanco == null)
            {
                return NotFound("Tarefa não encontrada");
            }
            try
            {
                tarefaBanco.Concluida = task.Concluida;
                foreach (var subTarefa in tarefaBanco.SubTasks)
                {
                    subTarefa.Concluida = task.Concluida;
                }
                _ctx.SaveChanges();
                return Ok("Status atualizado com sucesso");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno ao tentar deletar a tarefa.");
            }
        }

    }
}
