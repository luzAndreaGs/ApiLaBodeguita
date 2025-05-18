using System.ComponentModel.DataAnnotations;

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

        public string MetodoPago { get; set; }

        public ICollection<DetalleVenta> Detalles { get; set; }
    }
}
