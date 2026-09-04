using System;
using System.Collections.Generic;

namespace AppTask.Models;

public partial class Departamento
{
    public int Codigo { get; set; }

    public string Nome { get; set; } = null!;

    public string Sigla { get; set; } = null!;

    public virtual ICollection<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();
}

