using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppTask.Models;

public class CentralDeCusto
{
    [Key]
    public int Codigo { get; set; }

    [Required(ErrorMessage = "Informe o nome da central de custo.")]
    [StringLength(250)]
    [Display(Name = "Nome da central")]
    public string NomeCentral { get; set; } = string.Empty;

    [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "A meta anual não pode ser negativa.")]
    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Meta anual")]
    public decimal ValorMetaAnual { get; set; }
}
