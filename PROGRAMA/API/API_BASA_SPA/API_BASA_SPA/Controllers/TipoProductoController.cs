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
    public class TipoProductoController : ControllerBase
    {
        private readonly ArriendoMaquinariasContext _context;

        public TipoProductoController(ArriendoMaquinariasContext context)
        {
            _context = context;
        }

        // GET: api/TipoProducto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoProducto>>> GetTipoProductos()
        {
            Response r = new Response();
          if (_context.TipoProductos == null)
          {
              return NotFound();
          }

            var tipo_prod = await _context.TipoProductos.ToListAsync();
            r.Message = "Exito al listar los Tipos de Productos";
            r.Success = true;
            r.Data = tipo_prod;
            return Ok(r);
        }

        // GET: api/TipoProducto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoProducto>> GetTipoProducto(int id)
        {
          if (_context.TipoProductos == null)
          {
              return NotFound();
          }
            var tipoProducto = await _context.TipoProductos.FindAsync(id);

            if (tipoProducto == null)
            {
                return NotFound();
            }

            return tipoProducto;
        }
    }
}
