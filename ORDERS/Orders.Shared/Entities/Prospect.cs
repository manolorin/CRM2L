using Orders.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Shared.Entities;
public class Prospect: IEntityWithName
{
    public int Id { get; set; }

    [Display(Name = "Prospecto")]
    [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string Name { get; set; } = null!;

    [Display(Name = "Dirección")]
    [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string Address { get; set; } = null!;

    public City? City { get; set; }

    [Display(Name = "Ciudad")]
    [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una {0}.")]
    public int CityId { get; set; }

    [Display(Name = "Activo")]
    bool Active = true;

    public ICollection<Contact>? Contacts { get; set; }
    public int ContactsNumber => Contacts == null ? 0 : Contacts.Count;

}
