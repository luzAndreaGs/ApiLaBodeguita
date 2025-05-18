using System.ComponentModel.DataAnnotations;

namespace ApiLaBodeguita.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public string Marca { get; set; }

        [Required]
        public string CodigoBarras { get; set; }

        [Range(0, double.MaxValue)]
        public double Precio { get; set; }

        public int Existencia { get; set; }
    }
}
