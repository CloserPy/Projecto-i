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
    public class EstadoArriendoController : ControllerBase
    {
        private readonly ArriendoMaquinariasContext _context;

        public EstadoArriendoController(ArriendoMaquinariasContext context)
        {
            _context = context;
        }

        // GET: api/EstadoArriendo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoArriendo>>> GetEstadoArriendos()
        {
            Response r = new();
            if (_context.EstadoArriendos == null)
            {
              return NotFound();
            }
            var estado_arriendo = await _context.EstadoArriendos.ToListAsync();
            r.Message = "Exito";
            r.Success = true;
            r.Data = estado_arriendo;
            return Ok(r);
        }

        // GET: api/EstadoArriendo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoArriendo>> GetEstadoArriendo(int id)
        {
            Response r = new();
            if (_context.EstadoArriendos == null)
            {
              return NotFound();
            }
            var estadoArriendo = await _context.EstadoArriendos.FindAsync(id);

            if (estadoArriendo == null)
            {
                return NotFound();
            }
            r.Message = "Exito";
            r.Success = true;
            return Ok(r);
        }
    }
}
