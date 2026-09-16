using Orders.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Shared.Entities;

public class City: IEntityWithName
{
    public int Id { get; set; }

    [Display(Name = "Ciudad")]
    [MaxLength(100, ErrorMessage = "{0} no puede exceder los {1} caracteres.")]
    [Required(ErrorMessage = "El nombre del campo: \"{0}\" es obligatorio.")]
    public string Name { get; set; } = null!;

    public int StateId { get; set; }
    public State? State { get; set; } 
}
