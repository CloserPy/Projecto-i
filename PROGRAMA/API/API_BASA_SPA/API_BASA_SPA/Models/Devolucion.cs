using System;
using System.Collections.Generic;

namespace API_BASA_SPA.Models;

public partial class Devolucion
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public int IdArriendo { get; set; }

    public virtual Arriendo IdArriendoNavigation { get; set; } = null!;
}
