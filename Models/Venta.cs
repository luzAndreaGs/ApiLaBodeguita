using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiLaBodeguita.Models
{
    public class Venta
    {
        public int Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        public int UsuarioId { get; set; }

        public Usuario? Usuario { get; set; } 

        [Required]
        public double Total { get; set; }

        public string MetodoPago { get; set; } // Efectivo, Tarjeta, etc.

        public ICollection<DetalleVenta> Detalles { get; set; }
    }
}
