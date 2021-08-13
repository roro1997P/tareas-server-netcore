using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tareas_server.Context;
using tareas_server.Models;

namespace tareas_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareaController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        public TareaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var listaTareas = await _context.Tareas.ToListAsync();
                return Ok(listaTareas);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Tarea tarea)
        {
            try
            {
                _context.Tareas.Add(tarea);
                await _context.SaveChangesAsync();

                return Ok(new { message = "La tarea se registró con éxito" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Tarea tarea)
        {
            try
            {
                if(id != tarea.Id)
                {
                    return NotFound();
                }

                tarea.Estado = !tarea.Estado;
                _context.Update(tarea);
                await _context.SaveChangesAsync();
                return Ok(new { message = "La tarea se actualizó con éxito" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var tarea = await _context.Tareas.FindAsync(id);
                if (tarea == null) return NotFound();

                _context.Tareas.Remove(tarea);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Tarea eliminada con éxito" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
