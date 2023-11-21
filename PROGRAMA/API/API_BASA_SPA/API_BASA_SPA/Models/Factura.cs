using System;
using System.Collections.Generic;

namespace API_BASA_SPA.Models;

public partial class Factura
{
    public int Id { get; set; }

    public decimal Bruto { get; set; }

    public decimal? Descuento { get; set; }

    public decimal Neto { get; set; }

    public decimal Iva { get; set; }

    public decimal Total { get; set; }

    public DateTime FechaEmision { get; set; }

    public DateTime FechaExpiracion { get; set; }

    public int IdEstado { get; set; }

    public virtual ICollection<Arriendo> Arriendos { get; set; } = new List<Arriendo>();

    public virtual EstadoFactura IdEstadoNavigation { get; set; } = null!;
}
