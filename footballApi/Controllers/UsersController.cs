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
    public class UsersController : ControllerBase
    {
        private readonly DataContext _Context;
        private readonly ITokenService _tokenService;
        public UsersController(DataContext Context, ITokenService tokenService)
        {
            _Context = Context;
            _tokenService = tokenService;

        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _Context.Users.ToListAsync();
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _Context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        // POST api/users/Register
        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(UserRegisterDTO UserRegister)
        {

            if (UserRegister.Password != UserRegister.ConfPassword) return BadRequest("Password and ConfPassword don't match");


            if (await UserExists(UserRegister.UserName.ToLower())) return BadRequest("User name already exists");

            //hashing algorithm to create password hash
            //The using statement calls the Dispose method on the object in the correct way
            using var hmac = new HMACSHA512();

            //create new admin then added to DB
            var newUser = new User
            {
                UserName = UserRegister.UserName.ToLower(),
                FirstName = UserRegister.FirstName,
                LastName = UserRegister.LastName,
                Age = UserRegister.Age,
                Email = UserRegister.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF32.GetBytes(UserRegister.Password)),   //convert password from string to byte
                PasswordSalt = hmac.Key                                                            //HMACSHA512() initialize new instance with a random generated key
            };

            await _Context.Users.AddAsync(newUser);

            await _Context.SaveChangesAsync();

            return Ok("Created successfully");
        }

        // POST api/users/Login
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(UserLoginDTO userLogin)
        {
            var user = await _Context.Users
                .SingleOrDefaultAsync(ww => ww.UserName == userLogin.UserName.ToLower());

            if (user == null) return Unauthorized("user username or password is invalid");


            using var hmac = new HMACSHA512(user.GetPasswordSalt());

            var ComputeHash = hmac.ComputeHash(Encoding.UTF32.GetBytes(userLogin.Password));

            byte[] passHasg = user.GetPasswordHash();

            for (int i = 0; i < ComputeHash.Length; i++)
            {
                if (ComputeHash[i] != passHasg[i]) return Unauthorized("Invalid Password");
            }

            return new UserDTO
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _Context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _Context.Users.Remove(user);
            await _Context.SaveChangesAsync();

            return NoContent();
        }







        private async Task<bool> UserExists(int id)
        {
            return await _Context.Users.AnyAsync(e => e.Id == id);
        }

        private async Task<bool> UserExists(String UserNameRegistered)
        {

            return await _Context.Users.AnyAsync(e => e.UserName == UserNameRegistered);
        }
    }
}
