using System.ComponentModel.DataAnnotations;

namespace Orders.Shared.Entities;

public class Country
{
    public int Id { get; set; }

    [Display(Name = "Pais")]
    [MaxLength(100, ErrorMessage = "{0} no puede exceder los {1} caracteres.")]
    [Required(ErrorMessage = "El nombre de {0} es requerido.")]
    public string Name { get; set; } = null!;
}