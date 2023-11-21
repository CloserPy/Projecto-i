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
    public class MantenimientoController : ControllerBase
    {
        private readonly ArriendoMaquinariasContext _context;

        public MantenimientoController(ArriendoMaquinariasContext context)
        {
            _context = context;
        }

        // GET: api/Mantenimiento
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mantenimiento>>> GetMantenimientos()
        {
            Response r = new();
            if (_context.Mantenimientos == null)
            {
              return NotFound();
            }
            var mantenimientos = await _context.Mantenimientos.Select(x => new
            {
                id = x.Id,
                inicio = x.Inicio.ToString("yyyy-MM-dd"),
                termino = x.Termino.ToString("yyyy-MM-dd"),
                costo = x.Costo,
                idTipo = x.IdTipo,
                idEstado = x.IdEstado,
                idProducto = x.IdProducto,
                tipo = x.IdTipoNavigation.Nombre,
                estado = x.IdEstadoNavigation.Nombre,
                producto = x.IdProductoNavigation.Nombre
            }).ToListAsync();
            r.Message = "Exito";
            r.Success = true;
            r.Data = mantenimientos;
            return Ok(r);
        }

        // GET: api/Mantenimiento/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Mantenimiento>> GetMantenimiento(int id)
        {
            Response r = new();
          if (_context.Mantenimientos == null)
          {
              return NotFound();
          }
            var mantenimiento = await _context.Mantenimientos.Select(x => new
            {
                id = x.Id,
                inicio = x.Inicio.ToString("yyyy-MM-dd"),
                termino = x.Termino.ToString("yyyy-MM-dd"),
                costo = x.Costo,
                idTipo = x.IdTipo,
                idEstado = x.IdEstado,
                idProducto = x.IdProducto,
                tipo = x.IdTipoNavigation.Nombre,
                estado = x.IdEstadoNavigation.Nombre,
                producto = x.IdProductoNavigation.Nombre
            }).FirstOrDefaultAsync(x => x.id == id);

            if (mantenimiento == null)
            {
                return NotFound();
            }
            r.Message = "Exito";
            r.Success = true;
            r.Data = mantenimiento;
            return Ok(r);
        }

        // PUT: api/Mantenimiento/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMantenimiento(int id, Mantenimiento mantenimiento)
        {
            Response r = new();
            if (id != mantenimiento.Id)
            {
                return BadRequest();
            }

            _context.Entry(mantenimiento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MantenimientoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            r.Message = "Exito";
            r.Success = true;

            return Ok(r);
        }

        // POST: api/Mantenimiento
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Mantenimiento>> PostMantenimiento(Mantenimiento mantenimiento)
        {
            Response r = new();
          if (_context.Mantenimientos == null)
          {
              return Problem("Entity set 'ArriendoMaquinariasContext.Mantenimientos'  is null.");
          }
            _context.Mantenimientos.Add(mantenimiento);
            await _context.SaveChangesAsync();

            r.Message = "Exito";
            r.Success = true;
            return Ok(r);
        }

        // DELETE: api/Mantenimiento/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMantenimiento(int id)
        {
            Response r = new();
            if (_context.Mantenimientos == null)
            {
                return NotFound();
            }
            var mantenimiento = await _context.Mantenimientos.FindAsync(id);
            if (mantenimiento == null)
            {
                return NotFound();
            }

            _context.Mantenimientos.Remove(mantenimiento);
            await _context.SaveChangesAsync();

            r.Message = "Exito";
            r.Success = true;
            return Ok(r);
        }

        private bool MantenimientoExists(int id)
        {
            return (_context.Mantenimientos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
