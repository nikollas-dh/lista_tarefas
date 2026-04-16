using System;
using System.Collections.Generic;

namespace APITarefas.Models;

public partial class Task
{
    public int Id { get; set; }

    public string? Nome { get; set; }

    public string? Descrição { get; set; }

    public DateTime? Data { get; set; }

    public bool? Concluida { get; set; }

    public virtual ICollection<SubTask> SubTasks { get; set; } = new List<SubTask>();
}
