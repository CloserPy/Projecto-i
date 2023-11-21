using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_BASA_SPA.Models;
using Newtonsoft.Json;
using API_BASA_SPA.Class;

namespace API_BASA_SPA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly ArriendoMaquinariasContext _context;

        public UsuarioController(ArriendoMaquinariasContext context)
        {
            _context = context;
        }

        // GET: api/Usuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
          if (_context.Usuarios == null)
          {
              return NotFound();
          }
            return await _context.Usuarios.ToListAsync();
        }

        // GET: api/Usuario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
          if (_context.Usuarios == null)
          {
              return NotFound();
          }
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/Usuario/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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

        // POST: api/Usuario
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] Object login)
        {
            Response r = new();
            var data = JsonConvert.DeserializeObject<dynamic>(login.ToString());
            var nombre = "";
            var pass = "";
            if (data.nombre != null && data.pass != null)
            {
                nombre = data.nombre;
                pass = data.pass;
            }
            var user = _context.Usuarios
                .Where(u => u.Nombre == nombre && u.Pass == pass)
                .FirstOrDefault();
            if (user == null)
            {
                r.Success = false;
                r.Message = "Credenciales Incorrectas";
                return Ok(r);
            }
            else
            {
                r.Message = "Se ha logeado con éxito";
                r.Success = true;
                return Ok(r);
            }


        }

        

        private bool UsuarioExists(int id)
        {
            return (_context.Usuarios?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
