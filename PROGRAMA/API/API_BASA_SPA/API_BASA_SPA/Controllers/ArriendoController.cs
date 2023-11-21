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
    public class ArriendoController : ControllerBase
    {
        private readonly ArriendoMaquinariasContext _context;

        public ArriendoController(ArriendoMaquinariasContext context)
        {
            _context = context;
        }

        // GET: api/Arriendo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Arriendo>>> GetArriendos()
        {
            Response r = new();
            if (_context.Mantenimientos == null)
            {
                return NotFound();
            }
            var arriendos = await _context.Arriendos.Select(x => new
            {
                id = x.Id,
                inicio = x.Inicio.ToString("yyyy-MM-dd"),
                termino = x.Termino.ToString("yyyy-MM-dd"),
                valor = x.Valor,
                duracion = (x.Termino - x.Inicio).TotalDays,
                idProducto = x.IdProducto,
                idCliente = x.IdCliente,
                idEstado = x.IdEstado,
                producto = x.IdProductoNavigation.Nombre,
                cliente = x.IdClienteNavigation.Nombre,
                estado = x.IdEstadoNavigation.Nombre,
                idFactura = x.IdFactura
            }).ToListAsync();

            r.Message = "Exito";
            r.Success = true;
            r.Data = arriendos;
            return Ok(r);
        }
        [HttpGet("Activo")]
        public async Task<ActionResult<IEnumerable<Arriendo>>> GetArriendosActivos()
        {

            Response r = new();
            if (_context.Arriendos == null)
            {
                return NotFound();
            }
            var arriendos = await _context.Arriendos
                .Where(x => x.IdEstado == 1)
                .Select(x => new
                {
                    id = x.Id,
                    inicio = x.Inicio.ToString("yyyy-MM-dd"),
                    termino = x.Termino.ToString("yyyy-MM-dd"),
                    valor = x.Valor,
                    duracion = (x.Termino - x.Inicio).TotalDays,
                    idProducto = x.IdProducto,
                    idCliente = x.IdCliente,
                    idEstado = x.IdEstado,
                    producto = x.IdProductoNavigation.Nombre,
                    cliente = x.IdClienteNavigation.Nombre,
                    estado = x.IdEstadoNavigation.Nombre
                    
                }).ToListAsync();
            if (arriendos == null) { r.Message = "No Existen arriendos"; r.Success = false; return Ok(r); }

            r.Message = "Exito";
            r.Success = true;
            r.Data = arriendos;
            return Ok(r);
        }
        [HttpGet("Arrendados")]
        public async Task<ActionResult<IEnumerable<Arriendo>>> GetProductosArrendados()
        {

            Response r = new();
            if (_context.Arriendos == null)
            {
                return NotFound();
            }
            var productos = await _context.Arriendos
                .Where(x => x.IdProductoNavigation.IdEstado == 3)
                .Select(x => new
                {
                    id = x.Id,
                    inicio = x.Inicio.ToString("yyyy-MM-dd"),
                    termino = x.Termino.ToString("yyyy-MM-dd"),
                    valor = x.Valor,
                    duracion = (x.Termino - x.Inicio).TotalDays,
                    idProducto = x.IdProducto,
                    idCliente = x.IdCliente,
                    idEstado = x.IdEstado,
                    producto = x.IdProductoNavigation.Nombre,
                    cliente = x.IdClienteNavigation.Nombre,
                    estado = x.IdEstadoNavigation.Nombre

                }).ToListAsync();
            if (productos == null) { r.Message = "No Existen arriendos"; r.Success = false; return Ok(r); }

            r.Message = "Exito";
            r.Success = true;
            r.Data = productos;
            return Ok(r);
        }

        // GET: api/Arriendo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Arriendo>> GetArriendo(int id)
        {
            Response r = new();
            if (_context.Arriendos == null)
            {
                return NotFound();
            }
            var arriendo = await _context.Arriendos.Select(x => new
            {
                id = x.Id,
                inicio = x.Inicio.ToString("yyyy-MM-dd"),
                termino = x.Termino.ToString("yyyy-MM-dd"),
                valor = x.Valor,
                idProducto = x.IdProducto,
                idCliente = x.IdCliente,
                idEstado = x.IdEstado,
                producto = x.IdProductoNavigation.Nombre,
                cliente = x.IdClienteNavigation.Nombre,
                estado = x.IdEstadoNavigation.Nombre
            }).FirstOrDefaultAsync(x => x.id == id);

            if (arriendo == null)
            {
                return NotFound();
            }
            r.Message = "Exito";
            r.Success = true;
            r.Data = arriendo;
            return Ok(r);
        }
        [HttpGet("Facturados/{factura}")]
        public async Task<ActionResult<List<Arriendo>>> GetArriendosFacturados(int factura)
        {
            Response r = new();
            if (_context.Arriendos == null)
            {
                return NotFound();
            }
            var arriendos = await _context.Arriendos.Select(x => new
            {
                id = x.Id,
                inicio = x.Inicio.ToString("yyyy-MM-dd"),
                termino = x.Termino.ToString("yyyy-MM-dd"),
                valor = x.Valor,
                idProducto = x.IdProducto,
                duracion = (x.Termino - x.Inicio).TotalDays,
                idCliente = x.IdCliente,
                idEstado = x.IdEstado,
                producto = x.IdProductoNavigation.Nombre,
                cliente = x.IdClienteNavigation.Nombre,
                estado = x.IdEstadoNavigation.Nombre,
                idFactura = x.IdFactura
            }).Where(x => x.idFactura == factura).ToListAsync(); 

            if (arriendos == null || !arriendos.Any())
            {
                return NotFound();
            }
            r.Message = "Éxito";
            r.Success = true;
            r.Data = arriendos;
            return Ok(r);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutArriendo(int id, Arriendo arriendo)
        {
            Response r = new();
            if (id != arriendo.Id)
            {
                return BadRequest();
            }

            _context.Entry(arriendo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArriendoExists(id))
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

        [HttpPost]
        public async Task<ActionResult<Arriendo>> PostArriendo(Arriendo arriendo)
        {
            Response r = new();
          if (_context.Arriendos == null)
          {
              return Problem("Entity set 'ArriendoMaquinariasContext.Arriendos'  is null.");
          }
            _context.Arriendos.Add(arriendo);
            await _context.SaveChangesAsync();

            r.Message = "Exito";
            r.Success = true;
            return Ok(r);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArriendo(int id)
        {
            Response r = new();
            if (_context.Arriendos == null)
            {
                return NotFound();
            }
            var arriendo = await _context.Arriendos.FindAsync(id);
            if (arriendo == null)
            {
                return NotFound();
            }

            _context.Arriendos.Remove(arriendo);
            await _context.SaveChangesAsync();

            r.Message = "Exito";
            r.Success = true;
            return Ok(r);
        }

        private bool ArriendoExists(int id)
        {
            return (_context.Arriendos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
