using footballApi.Context;
using footballApi.Interfaces;
using footballApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace footballApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public TeamsController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
       
        //Get : api/teams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams()
        {
            return await _context.Teams.ToListAsync();
        }

        // GET: api/teams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeam(int id)
        {
            var Team = await _context.Teams.FindAsync(id);

            if (Team == null) return NotFound();

            return Team;
        }

        // POST api/teams
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<Team>> PostTeam(Team team)
        {
            //String authHeaders = Request.Headers["Authorization"].FirstOrDefault();

            //var authUser = _tokenService.GetJWTClams(authHeaders);
            //if (authHeaders == null) return Unauthorized("Only Admin can post ");

            //if (authUser.IsAdmin == false) return Unauthorized("Only Admin can post");

            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT api/teams/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam(int id, Team team)
        {
            if (id != team.Id)
                return BadRequest();

            _context.Entry(team).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE api/teams/id                  
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
                return NotFound();


            var playersInTeam = await _context.Players.Where(p => p.TeamId == id).ToListAsync();

            foreach (var TeamPlyaer in playersInTeam)
            {
                TeamPlyaer.TeamId = null;
                _context.Update(TeamPlyaer);

            }


            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return NoContent();
        }



        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }
    }
}
