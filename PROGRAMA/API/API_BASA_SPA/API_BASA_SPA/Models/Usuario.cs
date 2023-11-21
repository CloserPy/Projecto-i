using System;
using System.Collections.Generic;

namespace API_BASA_SPA.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Pass { get; set; } = null!;
}
