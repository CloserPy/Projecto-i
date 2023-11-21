using System;
using System.Collections.Generic;

namespace API_BASA_SPA.Models;

public partial class TipoMantencion
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Mantenimiento> Mantenimientos { get; set; } = new List<Mantenimiento>();
}
