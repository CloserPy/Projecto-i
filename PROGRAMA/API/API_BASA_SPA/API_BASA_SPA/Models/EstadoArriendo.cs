using System;
using System.Collections.Generic;

namespace API_BASA_SPA.Models;

public partial class EstadoArriendo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Arriendo> Arriendos { get; set; } = new List<Arriendo>();
}
