using System;
using System.Collections.Generic;

namespace API_BASA_SPA.Models;

public partial class Cliente
{
    public string Rut { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public int Telefono { get; set; }

    public string Direccion { get; set; } = null!;

    public int IdTipo { get; set; }

    public virtual ICollection<Arriendo> Arriendos { get; set; } = new List<Arriendo>();

    public virtual TipoCliente? IdTipoNavigation { get; set; }
}
