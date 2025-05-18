using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiLaBodeguita.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(50)]
        public string Marca { get; set; }

        [Required]
        [StringLength(20)]
        public string CodigoBarras { get; set; }

        [Range(0, 999999.99)]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }

        public int Existencia { get; set; }

        // Opcional: relación inversa
        public ICollection<DetalleVenta> DetallesVenta { get; set; } = new List<DetalleVenta>();
    }
}
