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
    public class DevolucionController : ControllerBase
    {
        private readonly ArriendoMaquinariasContext _context;

        public DevolucionController(ArriendoMaquinariasContext context)
        {
            _context = context;
        }

        // GET: api/Devolucion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Devolucion>>> GetDevolucions()
        {
            Response r = new();
            if (_context.Devolucions == null)
            {
                return NotFound();
            }
            var devoluciones = await _context.Devolucions.Select(x => new
            {
                id = x.Id,
                idArriendo = x.IdArriendo,
                producto = x.IdArriendoNavigation.IdProductoNavigation.Nombre,
                cliente = x.IdArriendoNavigation.IdClienteNavigation.Nombre,
                fecha = x.Fecha.ToString("yyyy-MM-dd")
            }).ToListAsync();

            r.Data = devoluciones;
            r.Message = "Exito";
            r.Success = true;
            return Ok(r);
        }

        // GET: api/Devolucion/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Devolucion>> GetDevolucion(int id)
        {
            Response r = new();
            if (_context.Devolucions == null)
            {
                return NotFound();
            }

            var devolucion = await _context.Devolucions.Select(x => new
            {
                id = x.Id,
                idArriendo = x.IdArriendo,
                producto = x.IdArriendoNavigation.IdProductoNavigation.Nombre,
                fecha = x.Fecha.ToString("yyyy-MM-dd")
            }).FirstOrDefaultAsync(x => x.id == id);
            if (devolucion == null)
            {
                return NotFound();
            }
            r.Data = devolucion;
            r.Message = "Exito";
            r.Success = true;
            return Ok(r);
        }


        // PUT: api/Devolucion/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDevolucion(int id, Devolucion devolucion)
        {
            Response r = new();
            if (id != devolucion.Id)
            {
                return BadRequest();
            }

            _context.Entry(devolucion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DevolucionExists(id))
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
    

        // POST: api/Devolucion
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Devolucion>> PostDevolucion(Devolucion devolucion)
        {
            Response r = new();
            if (_context.Devolucions == null)
            {
                return Problem("Entity set 'ArriendoMaquinariasContext.Devolucions'  is null.");
            }
            _context.Devolucions.Add(devolucion);
            await _context.SaveChangesAsync();

            r.Message = "Exito";
            r.Success = true;
            return Ok(r);
        }

        // DELETE: api/Devolucion/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevolucion(int id)
        {
            Response r = new();
            if (_context.Devolucions == null)
            {
                return NotFound();
            }
            var devolucion = await _context.Devolucions.FindAsync(id);
            if (devolucion == null)
            {
                return NotFound();
            }

            _context.Devolucions.Remove(devolucion);
            await _context.SaveChangesAsync();

            r.Message = "Exito";
            r.Success = true;
            return Ok(r);
        }

        private bool DevolucionExists(int id)
        {
            return (_context.Devolucions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
