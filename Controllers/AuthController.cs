using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalSurveyPractice.Data;
using FinalSurveyPractice.Models;
using FinalSurveyPractice.Services;
using FinalSurveyPractice.DTOs.AuthUser;
using AutoMapper;

namespace FinalSurveyPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(DataContext context, IAuthService authService, IMapper mapper)
        {
            _context = context;
            _authService = authService;
            _mapper = mapper;
        }

        // JWT ----------------------------------------------------------------
        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request)
        {
            var resp = await _authService.Register(
                new User
                {
                    Name = request.Name,
                    FirstSurname = request.FirstSurname,
                    LastSurname = request.LastSurname,
                    Status = request.Status,
                    Photo = request.Photo,
                }, request.Password
            );

            if (!resp.Success)
            {
                return BadRequest(resp);
            }
            return Ok(resp);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDto request)
        {
            var response = await _authService.Login(request.User, request.Password);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

// Metodos por defecto ----------------------------------------------------------------

        // GET: api/Auth
        [HttpGet("User")]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            var response = new ServiceResponse<IEnumerable<GetUserDto>>();

            var user = await _context.User.ToListAsync();

            response.Data = user.Select(c => _mapper.Map<GetUserDto>(c)).ToList();

            return Ok(response);
        }

        // GET: api/Auth/5
        [HttpGet("User{Id}")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetUserDto>>>> GetUser(int id)
        {
            var resp = new ServiceResponse<GetUserDto>();
            var user = await _context.User.FirstOrDefaultAsync(c => c.IdUser == id);

            if (user != null)
            {
                resp.Data = _mapper.Map<GetUserDto>(user);
            }
            else
            {
                resp.Success = false;
                resp.Message = "User not found";

                return NotFound(resp);
            }

            return Ok(resp);
        }

        // PUT: api/Auth/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> PutUser(UpdateUserDto user, int id)
        {
            var resp = await _authService.UpdateUser(
                new User
                {
                    Name = user.Name,
                    FirstSurname = user.FirstSurname,
                    LastSurname = user.LastSurname,
                    Status = user.Status,
                    Photo = user.Photo,
                }, user.Password, id
            );

            if (!resp.Success)
            {
                return BadRequest(resp);
            }
            return Ok(resp);
        }

        // DELETE: api/Auth/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.IdUser == id);
        }
    }
}
