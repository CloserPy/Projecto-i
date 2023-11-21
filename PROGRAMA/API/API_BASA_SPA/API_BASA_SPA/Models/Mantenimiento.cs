using System;
using System.Collections.Generic;

namespace API_BASA_SPA.Models;

public partial class Mantenimiento
{
    public int Id { get; set; }

    public DateTime Inicio { get; set; }

    public DateTime Termino { get; set; }

    public decimal Costo { get; set; }

    public int IdProducto { get; set; }

    public int IdTipo { get; set; }

    public int IdEstado { get; set; }

    public virtual EstadoMantenimiento IdEstadoNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual TipoMantencion IdTipoNavigation { get; set; } = null!;
}
