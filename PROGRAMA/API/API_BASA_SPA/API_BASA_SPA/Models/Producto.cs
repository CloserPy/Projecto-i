using System;
using System.Collections.Generic;

namespace API_BASA_SPA.Models;

public partial class Producto
{
    public int NumSerie { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Valor { get; set; }

    public int VidaUtil { get; set; }

    public DateTime FechaAdq { get; set; }

    public decimal ValorResidual { get; set; }

    public int IdEstado { get; set; }

    public int IdTipo { get; set; }

    public virtual ICollection<Arriendo> Arriendos { get; set; } = new List<Arriendo>();

    public virtual EstadoProducto? IdEstadoNavigation { get; set; } 

    public virtual TipoProducto? IdTipoNavigation { get; set; } 

    public virtual ICollection<Mantenimiento> Mantenimientos { get; set; } = new List<Mantenimiento>();
}
