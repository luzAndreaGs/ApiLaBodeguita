namespace ApiLaBodeguita.Dtos
{
    public class UsuarioDTO
    {
        public string Nombre { get; set; }
        public string UsuarioLogin { get; set; }
        public string Contrasena { get; set; }
        public string Proveedor { get; set; } = "Manual";
        public bool EsAdministrador { get; set; } = false;
    }
}
