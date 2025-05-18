using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiLaBodeguita.Models;
using ApiLaBodeguita.Dtos;

namespace ApiLaBodeguita.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VentasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ventas
        [HttpGet]
        public async Task<ActionResult> GetVentas()
        {
            try
            {
                var ventas = await _context.Ventas
                    .Include(v => v.Usuario)
                    .Include(v => v.Detalles)
                        .ThenInclude(d => d.Producto)
                    .ToListAsync();

                // Validaciones antes de devolver
                foreach (var v in ventas)
                {
                    if (v.Usuario == null)
                        return StatusCode(500, $"Error: La venta con ID {v.Id} no tiene Usuario asociado.");

                    foreach (var d in v.Detalles)
                    {
                        if (d.Producto == null)
                            return StatusCode(500, $"Error: El detalle con ID {d.Id} no tiene Producto asociado.");
                    }
                }

                return Ok(ventas);
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.InnerException?.Message ?? ex.Message,
                    title: "Error al obtener ventas",
                    statusCode: 500);
            }
        }


        // GET: api/ventas/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Venta>> GetVenta(int id)
        {
            try
            {
                var venta = await _context.Ventas
                    .Include(v => v.Usuario)
                    .Include(v => v.Detalles)
                        .ThenInclude(d => d.Producto)
                    .FirstOrDefaultAsync(v => v.Id == id);

                if (venta == null)
                    return NotFound();

                return venta;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la venta: {ex.Message}");
            }
        }

        // POST: api/ventas
        [HttpPost]
        public async Task<IActionResult> PostVenta([FromBody] VentaDTO ventaDto)
        {
            try
            {
                // Validación del usuario
                var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.Id == ventaDto.UsuarioId);
                if (!usuarioExiste)
                    return BadRequest($"El usuario con ID {ventaDto.UsuarioId} no existe.");

                // Validación de productos
                foreach (var detalle in ventaDto.Detalles)
                {
                    var productoExiste = await _context.Producto.AnyAsync(p => p.Id == detalle.ProductoId);
                    if (!productoExiste)
                        return BadRequest($"El producto con ID {detalle.ProductoId} no existe.");
                }

                // Creación de la venta
                var venta = new Venta
                {
                    Fecha = DateTime.Now,
                    UsuarioId = ventaDto.UsuarioId,
                    MetodoPago = ventaDto.MetodoPago,
                    Total = ventaDto.Total,
                    Detalles = ventaDto.Detalles.Select(d => new DetalleVenta
                    {
                        ProductoId = d.ProductoId,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = (double)_context.Producto.First(p => p.Id == d.ProductoId).Precio
                    }).ToList()
                };


                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = "Venta registrada correctamente", venta.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al registrar la venta: {ex.Message}");
            }
        }
    }
}