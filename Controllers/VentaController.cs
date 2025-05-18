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
        public async Task<ActionResult<IEnumerable<Venta>>> GetVentas()
        {
            return await _context.Ventas
                .Include(v => v.Usuario)
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Producto)
                .ToListAsync();
        }

        // GET: api/ventas
        [HttpGet("{id}")]
        public async Task<ActionResult<Venta>> GetVenta(int id)
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

        // POST: api/ventas
        [HttpPost]
        public async Task<IActionResult> PostVenta([FromBody] VentaDTO ventaDto)
        {
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
                    PrecioUnitario = _context.Producto.First(p => p.Id == d.ProductoId).Precio
                }).ToList()
            };

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Venta registrada correctamente", venta.Id });
        }

    }
}
