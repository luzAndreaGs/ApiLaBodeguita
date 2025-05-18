using System.ComponentModel.DataAnnotations;

namespace ApiLaBodeguita.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string UsuarioLogin { get; set; }  // puede ser un correo electrónico

        public string Contrasena { get; set; }  // null si usa login con Google

        [Required]
        public string Proveedor { get; set; }  // "Manual", "Google", "Facebook"

        public bool EsAdministrador { get; set; } = false;
    }
}
