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
    public class EstadoMantenimientoController : ControllerBase
    {
        private readonly ArriendoMaquinariasContext _context;

        public EstadoMantenimientoController(ArriendoMaquinariasContext context)
        {
            _context = context;
        }

        // GET: api/EstadoMantenimiento
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoMantenimiento>>> GetEstadoMantenimientos()
        {
            Response r = new();
            if (_context.EstadoMantenimientos == null)
            {
              return NotFound();
            }
            var estado_mantencion = await _context.EstadoMantenimientos.ToListAsync();
            r.Message = "Exito al listar Estado de Mantenimientos";
            r.Success = true;
            r.Data = estado_mantencion;
            return Ok(r);
        }

        // GET: api/EstadoMantenimiento/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoMantenimiento>> GetEstadoMantenimiento(int id)
        {
          if (_context.EstadoMantenimientos == null)
          {
              return NotFound();
          }
            var estadoMantenimiento = await _context.EstadoMantenimientos.FindAsync(id);

            if (estadoMantenimiento == null)
            {
                return NotFound();
            }

            return estadoMantenimiento;
        }
    }
}
