using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiLaBodeguita.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace ApiLaBodeguita.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // POST: api/usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            // Si el proveedor es "Manual", hasheamos la contraseña
            if (usuario.Proveedor == "Manual" && !string.IsNullOrEmpty(usuario.Contrasena))
            {
                usuario.Contrasena = HashearContrasena(usuario.Contrasena);
            }

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuarios), new { id = usuario.Id }, usuario);
        }

        // POST: api/usuarios/login
        [HttpPost("login")]
        public async Task<ActionResult<Usuario>> Login(LoginDTO login)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.UsuarioLogin == login.UsuarioLogin && u.Proveedor == "Manual");

            if (usuario == null)
                return Unauthorized("Usuario no encontrado.");

            if (usuario.Contrasena != HashearContrasena(login.Contrasena))
                return Unauthorized("Contraseña incorrecta.");

            return Ok(usuario);
        }

        // GET: api/usuarios/loginSocial?usuarioLogin=correo&proveedor=Google
        [HttpGet("loginSocial")]
        public async Task<ActionResult<Usuario>> LoginSocial(string usuarioLogin, string proveedor)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.UsuarioLogin == usuarioLogin && u.Proveedor == proveedor);

            if (usuario == null)
                return NotFound("Usuario no registrado con ese proveedor.");

            return Ok(usuario);
        }

        // PUT: api/usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
                return BadRequest();

            if (usuario.Proveedor == "Manual" && !string.IsNullOrEmpty(usuario.Contrasena))
            {
                usuario.Contrasena = HashearContrasena(usuario.Contrasena);
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Usuarios.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private string HashearContrasena(string contrasena)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(contrasena);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }

    public class LoginDTO
    {
        public string UsuarioLogin { get; set; }
        public string Contrasena { get; set; }
    }
}
