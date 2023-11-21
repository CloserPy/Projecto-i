using System;
using System.Collections.Generic;

namespace API_BASA_SPA.Models;

public partial class EstadoMantenimiento
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Mantenimiento> Mantenimientos { get; set; } = new List<Mantenimiento>();
}
