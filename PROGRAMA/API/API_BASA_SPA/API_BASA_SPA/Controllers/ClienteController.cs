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
    public class ClienteController : ControllerBase
    {
        private readonly ArriendoMaquinariasContext _context;

        public ClienteController(ArriendoMaquinariasContext context)
        {
            _context = context;
        }

        // Ok
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            Response r = new Response();
            try
            {
                var cliente = await _context.Clientes.Select(c => new
                {
                    rut = c.Rut,
                    nombre = c.Nombre,
                    idTipo = c.IdTipo,
                    tipo = c.IdTipoNavigation.Nombre,
                    telefono = c.Telefono,
                    direccion = c.Direccion
                }).ToListAsync();
                if (cliente.Any())
                {
                    r.Data = cliente;
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

        // GET: api/Cliente/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(string id)
        {
            Response r = new Response();
            if (_context.Clientes == null)
          {
              return NotFound();
          }
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }
            r.Message = "Cliente Encontrado";
            r.Success = true;
            r.Data = cliente;
            return Ok(r);
        }

        // PUT: api/Cliente/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(string id, Cliente cliente)
        {
            Response r = new Response();
            if (id != cliente.Rut)
            {
                return BadRequest();
            }

            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            r.Message = "Cliente Editado con Exito";
            r.Success = true;
            return Ok(r);
        }

        // POST: api/Cliente
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            Response r = new Response();
          if (_context.Clientes == null)
          {
              return Problem("Entity set 'ArriendoMaquinariasContext.Clientes'  is null.");
          }
            _context.Clientes.Add(cliente);
            try
            {
                if (ClienteExists(cliente.Rut))
                {
                    r.Message = "Ya existe un cliente con el mismo rut";
                    r.Success = false;
                    return Ok(r);
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {

                throw;
            }
            r.Message = "Registro creado con Exito";
            r.Success = true;
            return Ok(r);
        }

        // DELETE: api/Cliente/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(string id)
        {
            Response r = new Response();
            if (_context.Clientes == null)
            {
                return NotFound();
            }
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            r.Message = "Cliente eliminado";
            r.Success = true;
            return Ok(r);
        }

        private bool ClienteExists(string id)
        {
            return (_context.Clientes?.Any(e => e.Rut == id)).GetValueOrDefault();
        }
    }
}
