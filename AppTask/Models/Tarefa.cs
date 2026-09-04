using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace AppTask.Models;

public partial class Tarefa
{
    public int Codigo { get; set; }

    public string Descricao { get; set; } = null!;

    public DateTime DataPlanejada { get; set; }

    public DateTime? DataIniciada { get; set; }

    public DateTime? DataFinalizada { get; set; }

    public DateTime? DataCancelada { get; set; }

    public string StatusTarefa { get; set; } = null!;

    public string Prazo { get; set; } = null!;

    public int FuncionarioId { get; set; }

    [ValidateNever]
    public virtual Funcionario Funcionario { get; set; } = null!;
}
