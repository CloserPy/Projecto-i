using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_BASA_SPA.Models;
using API_BASA_SPA.Class;

namespace API_BASA_SPA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoProductoController : ControllerBase
    {
        private readonly ArriendoMaquinariasContext _context;

        public EstadoProductoController(ArriendoMaquinariasContext context)
        {
            _context = context;
        }

        // GET: api/EstadoProducto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoProducto>>> GetEstadoProductos()
        {
            Response r = new Response();
            if (_context.EstadoProductos == null)
            {
              return NotFound();
            }
            var estado_prod = await _context.EstadoProductos.ToListAsync();
            r.Message = "Exito al listar Estado Producto";
            r.Data = estado_prod;
            r.Success = true;
            return Ok(r);
        }

        // GET: api/EstadoProducto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoProducto>> GetEstadoProducto(int id)
        {
          if (_context.EstadoProductos == null)
          {
              return NotFound();
          }
            var estadoProducto = await _context.EstadoProductos.FindAsync(id);

            if (estadoProducto == null)
            {
                return NotFound();
            }

            return estadoProducto;
        }
    }
}
