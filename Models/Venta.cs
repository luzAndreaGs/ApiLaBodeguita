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

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        public string MetodoPago { get; set; }

        public ICollection<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
    }
}
