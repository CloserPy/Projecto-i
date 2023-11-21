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
    public class ProductoController : ControllerBase
    {
        private readonly ArriendoMaquinariasContext _context;

        public ProductoController(ArriendoMaquinariasContext context)
        {
            _context = context;
        }

        
        // GET: api/Producto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            Response r = new Response();
            try
            {
                var producto = await _context.Productos.Select(x => new
                {
                    numSerie = x.NumSerie,
                    nombre = x.Nombre,
                    tipo = x.IdTipoNavigation.Nombre,
                    idEstado = x.IdEstado,
                    estado = x.IdEstadoNavigation.Nombre,
                    fechaAdq = x.FechaAdq.ToString("yyyy-MM-dd"),
                    valor = x.Valor,
                    vidaUtil = x.VidaUtil,
                    valorResidual = x.ValorResidual
                }).ToListAsync();
                if (producto.Any())
                {
                    r.Data = producto;
                    r.Success = true;
                    r.Message = "Los datos han sido cargado con éxito";
                    return Ok(r);
                }
                else
                {
                    r.Message = "No existen registros";
                    r.Success = false;
                    return Ok(r);
                }
            }
            catch (Exception ex)
            {
                r.Message = ex.Message;
                return BadRequest(r.Message);
            }
        }
        
        // GET: api/Producto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            Response r = new Response();
            if (_context.Productos == null)
            {
              return NotFound();
            }
            var producto = await _context.Productos.Select(x => new
            {
                numSerie = x.NumSerie,
                nombre = x.Nombre,
                idTipo = x.IdTipo,
                tipo = x.IdTipoNavigation.Nombre,
                idEstado = x.IdEstado,
                estado = x.IdEstadoNavigation.Nombre,
                fechaAdq = x.FechaAdq.ToString("yyyy-MM-dd"),
                valor = x.Valor,
                vidaUtil = x.VidaUtil,
                valorResidual = x.ValorResidual
            }).FirstOrDefaultAsync(x => x.numSerie == id);

            if (producto == null)
            {
                return NotFound();
            }
            r.Message = "Producto encontrado";
            r.Success = true;
            r.Data = producto;
            return Ok(r);
        }

        // PUT: api/Producto/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            Response r = new();
            if (id != producto.NumSerie)
            {
                return BadRequest();
            }

            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            r.Message = "Producto editado con éxito";
            r.Success = true;
            return Ok(r);
        }

        // POST: api/Producto
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            Response r = new();
          if (_context.Productos == null)
          {
              return Problem("Entity set 'ArriendoMaquinariasContext.Productos'  is null.");
          }
            _context.Productos.Add(producto);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProductoExists(producto.NumSerie))
                {
                    r.Message = "Ya existe un producto con el mismo número de serie";
                    r.Success = false;
                    return Ok(r);
                }
                else
                {
                    throw;
                }
            }
            r.Message = "¡¡Registro Exitoso!!";
            r.Success = true;
            return Ok(r);
        }

        // DELETE: api/Producto/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            Response r = new();
            if (_context.Productos == null)
            {
                return NotFound();
            }
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            r.Message = "Producto eliminado con éxito";
            r.Success = true;
            return Ok(r);
        }

        private bool ProductoExists(int id)
        {
            return (_context.Productos?.Any(e => e.NumSerie == id)).GetValueOrDefault();
        }
    }
}
