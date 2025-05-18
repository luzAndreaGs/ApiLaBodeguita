using System.ComponentModel.DataAnnotations;

namespace ApiLaBodeguita.Models
{
    public class DetalleVenta
    {
        public int Id { get; set; }

        [Required]
        public int VentaId { get; set; }

        public Venta? Venta { get; set; }

        [Required]
        public int ProductoId { get; set; }

        public Producto? Producto { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public double PrecioUnitario { get; set; }
    }
}
