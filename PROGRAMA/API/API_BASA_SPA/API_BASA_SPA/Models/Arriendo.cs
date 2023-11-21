using System;
using System.Collections.Generic;

namespace API_BASA_SPA.Models;

public partial class Arriendo
{
    public int Id { get; set; }

    public DateTime Inicio { get; set; }

    public DateTime Termino { get; set; }

    public decimal Valor { get; set; }

    public int IdProducto { get; set; }

    public string IdCliente { get; set; } = null!;

    public int IdEstado { get; set; }

    public int? IdFactura { get; set; }

    public virtual ICollection<Devolucion> Devolucions { get; set; } = new List<Devolucion>();

    public virtual Cliente? IdClienteNavigation { get; set; } 

    public virtual EstadoArriendo? IdEstadoNavigation { get; set; } 

    public virtual Factura? IdFacturaNavigation { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; } 
}
