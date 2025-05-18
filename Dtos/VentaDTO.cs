namespace ApiLaBodeguita.Dtos
{
    public class VentaDTO
    {
        public int UsuarioId { get; set; }
        public double Total { get; set; }
        public string MetodoPago { get; set; } = "Efectivo";
        public List<DetalleDTO> Detalles { get; set; }
    }

    public class DetalleDTO
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
    }
}
