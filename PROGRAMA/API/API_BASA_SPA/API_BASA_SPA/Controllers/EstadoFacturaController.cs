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
    public class EstadoFacturaController : ControllerBase
    {
        private readonly ArriendoMaquinariasContext _context;

        public EstadoFacturaController(ArriendoMaquinariasContext context)
        {
            _context = context;
        }

        // GET: api/EstadoFactura
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoFactura>>> GetEstadoFacturas()
        {
            Response r = new();
          if (_context.EstadoFacturas == null)
          {
              return NotFound();
          }
            var estado_facturas = await _context.EstadoFacturas.ToListAsync();
            r.Message = "Exito";
            r.Success = true;
            r.Data = estado_facturas;
            return Ok(r);
        }

        // GET: api/EstadoFactura/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoFactura>> GetEstadoFactura(int id)
        {
            Response r = new();
          if (_context.EstadoFacturas == null)
          {
              return NotFound();
          }
            var estadoFactura = await _context.EstadoFacturas.FindAsync(id);

            if (estadoFactura == null)
            {
                return NotFound();
            }

            r.Message = "Exito";
            r.Success = true;
            r.Data = estadoFactura;
            return Ok(r);
        }

        // PUT: api/EstadoFactura/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstadoFactura(int id, EstadoFactura estadoFactura)
        {
            if (id != estadoFactura.Id)
            {
                return BadRequest();
            }

            _context.Entry(estadoFactura).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoFacturaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/EstadoFactura
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EstadoFactura>> PostEstadoFactura(EstadoFactura estadoFactura)
        {
          if (_context.EstadoFacturas == null)
          {
              return Problem("Entity set 'ArriendoMaquinariasContext.EstadoFacturas'  is null.");
          }
            _context.EstadoFacturas.Add(estadoFactura);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EstadoFacturaExists(estadoFactura.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEstadoFactura", new { id = estadoFactura.Id }, estadoFactura);
        }

        // DELETE: api/EstadoFactura/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstadoFactura(int id)
        {
            if (_context.EstadoFacturas == null)
            {
                return NotFound();
            }
            var estadoFactura = await _context.EstadoFacturas.FindAsync(id);
            if (estadoFactura == null)
            {
                return NotFound();
            }

            _context.EstadoFacturas.Remove(estadoFactura);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstadoFacturaExists(int id)
        {
            return (_context.EstadoFacturas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
