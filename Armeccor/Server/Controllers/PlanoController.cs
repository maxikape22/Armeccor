using Armeccor.Datos;
using Armeccor.Datos.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Armeccor.Server.Controllers
{
    [ApiController]
    [Route("api/Planos")]
    public class PlanoController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public PlanoController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plano>>> GetPlanos()
        {
            var planos = await context.Planos.ToListAsync();
            return Ok(planos);
        }

        [HttpPost]
        public async Task<ActionResult<Plano>> PostPlano(Plano plano)
        {
            context.Add(plano);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
