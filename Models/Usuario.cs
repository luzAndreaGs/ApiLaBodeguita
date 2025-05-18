using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiLaBodeguita.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string UsuarioLogin { get; set; }

        [JsonIgnore]
        [StringLength(100)]
        public string Contrasena { get; set; }

        [Required]
        [StringLength(20)]
        public string Proveedor { get; set; }  // "Manual", "Google", etc.

        public bool EsAdministrador { get; set; } = false;

        // Opcional: para navegación inversa
        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    }
}
