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
using FinalSurveyPractice.DTOs.Category;

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

        // JWT ----------------------------------------------------------------------------
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
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetUserDto>>>> GetUser()
        {
            var resp = new ServiceResponse<IEnumerable<GetUserDto>>();

            var usr = await _context.User
                .Include(r => r.Role)
                .ToListAsync();

            resp.Data = usr.Select(c => _mapper.Map<GetUserDto>(c)).ToList();

            return Ok(resp);
        }

        // GET: api/Auth/5
        [HttpGet("User/{id}")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> GetUserId(int id)
        {
            var resp = new ServiceResponse<GetUserDto>();
            var usr = await _context.User
                .Include(r => r.Role)
                .FirstOrDefaultAsync(c => c.IdUser == id);

            if (usr != null)
            {
                resp.Data = _mapper.Map<GetUserDto>(usr);
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
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetUserDto>>>> PutUser(UpdateUserDto user, int id)
        {
            var response = await _authService.UpdateUser(
                    new User
                    {
                        Name = user.Name,
                        FirstSurname = user.FirstSurname,
                        LastSurname = user.LastSurname,
                        Status = user.Status,
                        Photo = user.Photo,
                    }, user.Password, id
                );

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("userRole")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> AddUserRole(AddUserRoleDto newUserRole)
        {
            var serviceResp = new ServiceResponse<GetUserDto>();

            try
            {
                var user = await _context.User
                                .Include(r => r.Role)
                                .FirstOrDefaultAsync(u => u.IdUser == newUserRole.UsersIdUse);
                if (user == null)
                {
                    serviceResp.Success = false;
                    serviceResp.Message = "User No Encontrado";
                    return NotFound(serviceResp);
                }

                var role = await _context.Role
                                .FirstOrDefaultAsync(r => r.IdRole == newUserRole.RoleIdRole);
                if (role == null)
                {
                    serviceResp.Success = false;
                    serviceResp.Message = "Role No Encontrado";
                    return NotFound(serviceResp);
                }

                user.Role.Add(role);
                await _context.SaveChangesAsync();
                serviceResp.Data = _mapper.Map<GetUserDto>(user);
            }
            catch (Exception ex)
            {
                serviceResp.Success = false;
                serviceResp.Message = ex.Message;
            }

            return serviceResp;
        }

        // DELETE: api/Auth/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> DeleteUser(int id)
        {
            ServiceResponse<IEnumerable<GetUserDto>> serviceResponse = new();

            try
            {
                User usr = await _context.User.FirstOrDefaultAsync(c => c.IdUser.ToString().ToUpper() == id.ToString().ToUpper());

                if (usr != null)
                {
                    _context.User.Remove(usr);
                    await _context.SaveChangesAsync();

                    serviceResponse.Data = _context.User.Select(c => _mapper.Map<GetUserDto>(c)).ToList();
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "CUser no se encontró";

                    return NotFound(serviceResponse);
                }
            }
            catch (Exception ex)
            {

                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return Ok(serviceResponse);
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.IdUser == id);
        }
    }
}

