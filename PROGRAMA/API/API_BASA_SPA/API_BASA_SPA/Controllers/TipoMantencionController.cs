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
    public class TipoMantencionController : ControllerBase
    {
        private readonly ArriendoMaquinariasContext _context;

        public TipoMantencionController(ArriendoMaquinariasContext context)
        {
            _context = context;
        }

        // GET: api/TipoMantencion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoMantencion>>> GetTipoMantencions()
        {
            Response r = new();
          if (_context.TipoMantencions == null)
          {
              return NotFound();
          }
            var tipo_mantencion = await _context.TipoMantencions.ToListAsync();
            r.Success = true;
            r.Message = "Exito al cargar Tipos de Mantenciones";
            r.Data = tipo_mantencion;
            return Ok(r);
        }

        // GET: api/TipoMantencion/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoMantencion>> GetTipoMantencion(int id)
        {
          if (_context.TipoMantencions == null)
          {
              return NotFound();
          }
            var tipoMantencion = await _context.TipoMantencions.FindAsync(id);

            if (tipoMantencion == null)
            {
                return NotFound();
            }

            return tipoMantencion;
        }
    }
}
