using System;
using System.Collections.Generic;

namespace API_BASA_SPA.Models;

public partial class TipoProducto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
