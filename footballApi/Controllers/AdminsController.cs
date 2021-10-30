using footballApi.Context;
using footballApi.DTO;
using footballApi.Interfaces;
using footballApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace footballApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly DataContext _Context;
        private readonly ITokenService _tokenService;
        public AdminsController(DataContext Context , ITokenService tokenService)
        {
            _Context = Context;
            _tokenService = tokenService;
           
        }

        // GET: api/admins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admin>>> GetAdmins()
        {
            return await _Context.Admins.ToListAsync();
        }

        // GET api/admins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Admin>> GetAdmin(int id)
        {
            var admin = await  _Context.Admins.FindAsync(id) ;
            if (admin == null)
            {
                return NotFound();
            }
            return admin;
        }

        // POST api/admins/Register
        [HttpPost("Register")]
        public async Task<ActionResult<Admin>> Register(AdminRegisterDTO AdminRegister)
        {

            if (AdminRegister.Password != AdminRegister.ConfPassword) return BadRequest("Password and ConfPassword don't match");


            if (await AdminExists(AdminRegister.UserName.ToLower())) return BadRequest("Admin name already exists");

            //hashing algorithm to create password hash
            //The using statement calls the Dispose method on the object in the correct way
            using var hmac = new HMACSHA512();

            //create new admin then added to DB
            var newAdmin = new Admin
            {
                UserName = AdminRegister.UserName.ToLower(),
                FirstName = AdminRegister.FirstName,
                LastName = AdminRegister.LastName,
                Age = AdminRegister.Age,
                Email = AdminRegister.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF32.GetBytes(AdminRegister.Password)),   //convert password from string to byte
                PasswordSalt = hmac.Key                                                            //HMACSHA512() initialize new instance with a random generated key
            };

            await _Context.Admins.AddAsync(newAdmin);

            await _Context.SaveChangesAsync();

            return Ok("Created successfully");
        }

        // POST api/admins/Login
        [HttpPost("Login")]
        public async Task<ActionResult<AdminDTO>> Login(AdminLoginDTO adminLogin)
        {
            var admin = await _Context.Admins
                .SingleOrDefaultAsync(ww => ww.UserName == adminLogin.UserName.ToLower());

            if (admin == null) return Unauthorized("admin username or password is invalid");

  
            using var hmac = new HMACSHA512(admin.GetPasswordSalt());

            var ComputeHash = hmac.ComputeHash(Encoding.UTF32.GetBytes(adminLogin.Password));

            byte[] passHasg = admin.GetPasswordHash();

            for (int i = 0; i < ComputeHash.Length; i++)
            {
                if (ComputeHash[i] != passHasg[i]) return Unauthorized("Invalid Password");
            }

            return new AdminDTO
            {
                UserName = admin.UserName,
                Token = _tokenService.CreateToken(admin)
            };

        }

        // DELETE: api/admins/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            var admin = await _Context.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }

            _Context.Admins.Remove(admin);
            await _Context.SaveChangesAsync();

            return NoContent();
        }







        private async Task<bool> AdminExists(int id)
        {
            return await _Context.Admins.AnyAsync(e => e.Id == id);
        }

        private async Task<bool> AdminExists(String UserNameRegistered)
        {

            return await _Context.Admins.AnyAsync(e => e.UserName == UserNameRegistered);
        }
    }
}
