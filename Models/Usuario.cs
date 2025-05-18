using System.ComponentModel.DataAnnotations;

namespace ApiLaBodeguita.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string UsuarioLogin { get; set; }

        public string Contrasena { get; set; }

        [Required]
        public string Proveedor { get; set; }

        public bool EsAdministrador { get; set; } = false;
    }
}
