using footballApi.Context;
using footballApi.Interfaces;
using footballApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace footballApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public PlayersController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        //Get : api/players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            return await _context.Players.ToListAsync();
        }

        // GET: api/players/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            var Player = await _context.Players.FindAsync(id);

            if (Player == null) return NotFound();

            return Player;
        }

        //Delete: api/players/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Player>> DeletePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
                return NotFound();

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //update: api/players/id
        //[HttpPut]
        //public async Task<ActionResult<Player>> UpdatePlayer(int id, Player player)
        //{
        //    if (id != player.Id)
        //        return BadRequest();

        //    _context.Entry(player).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PlayerExists(id))
        //            return NotFound();
        //        else
        //            throw;
        //    }

        //    return NoContent();
        //}

        //private bool PlayerExists(int id)
        //{
        //    return _context.Players.Any(e => e.Id == id);
        //}
    }
}
