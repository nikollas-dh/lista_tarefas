using System;
using System.Collections.Generic;

namespace AppMobile.Models;

public partial class SubTask
{
    public int Id { get; set; }

    public int? TaskId { get; set; }

    public string? Nome { get; set; }

    public bool? Concluida { get; set; }

    public virtual Task? Task { get; set; }
}
