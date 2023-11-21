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
    public class FacturaController : ControllerBase
    {
        private readonly ArriendoMaquinariasContext _context;

        public FacturaController(ArriendoMaquinariasContext context)
        {
            _context = context;
        }

        // GET: api/Factura
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Factura>>> GetFacturas()
        {
            Response r = new();
            if (_context.Facturas == null)
            {
              return NotFound();
            }
            var facturas = await _context.Facturas.Select(x => new{
                id = x.Id,
                bruto = x.Bruto,
                descuento = x.Descuento,
                neto = x.Neto,
                iva = x.Iva,
                total = x.Total,
                fechaEmision = x.FechaEmision.ToString("yyyy-MM-dd"),
                fechaExpiracion = x.FechaExpiracion.ToString("yyyy-MM-dd"),
                estado = x.IdEstadoNavigation.Nombre

            }).ToListAsync();
            r.Message = "Exito";
            r.Success = true;
            r.Data = facturas;
            return Ok(r);
        }

        // GET: api/Factura/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Factura>> GetFactura(int id)
        {
            Response r = new();
            if (_context.Facturas == null)
            {
                return NotFound();
            }
            var factura = await _context.Facturas.Select(x => new {
                id = x.Id,
                bruto = x.Bruto,
                descuento = x.Descuento,
                neto = x.Neto,
                iva = x.Iva,
                total = x.Total,
                fechaEmision = x.FechaEmision.ToString("yyyy-MM-dd"),
                fechaExpiracion = x.FechaExpiracion.ToString("yyyy-MM-dd"),
                estado = x.IdEstadoNavigation.Nombre,
                idEstado = x.IdEstado

            }).FirstOrDefaultAsync(x => x.id == id);
            if (factura == null)
            {
                r.Message = "No se encontró la factura";
                r.Success = false;
                return Ok(r);
            }
            r.Message = "Exito";
            r.Success = true;
            r.Data = factura;
            return Ok(r);
        }

        // PUT: api/Factura/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFactura(int id, Factura factura)
        {
            Response r = new();
            if (id != factura.Id)
            {
                return BadRequest();
            }

            _context.Entry(factura).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacturaExists(id))
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

        // POST: api/Factura
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Factura>> PostFactura(Factura factura)
        {
            Response r = new();
          if (_context.Facturas == null)
          {
              return Problem("Entity set 'ArriendoMaquinariasContext.Facturas'  is null.");
          }
            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();

            r.Message = "Exito";
            r.Data = factura.Id;
            r.Success = true;
            return Ok(r);
        }

        // DELETE: api/Factura/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFactura(int id)
        {
            Response r = new();
            if (_context.Facturas == null)
            {
                return NotFound();
            }
            var factura = await _context.Facturas.FindAsync(id);
            if (factura == null)
            {
                return NotFound();
            }

            _context.Facturas.Remove(factura);
            await _context.SaveChangesAsync();

            r.Message = "Exito";
            r.Success = true;
            return Ok(r);
        }

        private bool FacturaExists(int id)
        {
            return (_context.Facturas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
