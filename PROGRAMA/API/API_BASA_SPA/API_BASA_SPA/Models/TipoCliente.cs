using System;
using System.Collections.Generic;

namespace API_BASA_SPA.Models;

public partial class TipoCliente
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
}
