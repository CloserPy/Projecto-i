using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_BASA_SPA.Models;
using System.Security.Cryptography.Xml;
using API_BASA_SPA.Class;

namespace API_BASA_SPA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoClienteController : ControllerBase
    {
        private readonly ArriendoMaquinariasContext _context;

        public TipoClienteController(ArriendoMaquinariasContext context)
        {
            _context = context;
        }

        // GET: api/TipoCliente
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoCliente>>> GetTipoClientes()
        {
            Response r = new Response(); 
            //si no existe la tabla tipo clientes
          if (_context.TipoClientes == null)
          {
              return NotFound();
          }
            var tipos_cliente = await _context.TipoClientes.ToListAsync();
            r.Message = "Exito al listar tipo clientes";
            r.Success = true;
            r.Data = tipos_cliente;
            return Ok(r);
        }

        // GET: api/TipoCliente/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoCliente>> GetTipoCliente(int id)
        {
          if (_context.TipoClientes == null)
          {
              return NotFound();
          }
            var tipoCliente = await _context.TipoClientes.FindAsync(id);

            if (tipoCliente == null)
            {
                return NotFound();
            }

            return tipoCliente;
        }
    }
}
