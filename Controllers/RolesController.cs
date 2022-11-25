using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalSurveyPractice.Data;
using FinalSurveyPractice.Models;
using AutoMapper;
using FinalSurveyPractice.DTOs.Role;

namespace FinalSurveyPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public RolesController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetRoleDto>>>> GetRole()
        {
            var resp = new ServiceResponse<IEnumerable<GetRoleDto>>();

            var role = await _context.Role.ToListAsync();

            resp.Data = role.Select(c => _mapper.Map<GetRoleDto>(c)).ToList();

            return Ok(resp);
        }

        public async Task<ActionResult<ServiceResponse<GetRoleDto>>> GetRole(Guid id)
        {
            var resp = new ServiceResponse<GetRoleDto>();
            var rol = await _context.Role.FirstOrDefaultAsync(c => c.IdRole.ToString().ToUpper() == id.ToString().ToUpper());

            if (rol != null)
            {
                resp.Data = _mapper.Map<GetRoleDto>(rol);
            }
            else
            {
                resp.Success = false;
                resp.Message = "Rol no encotrado";

                return NotFound(resp);
            }

            return Ok(resp);
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<GetRoleDto>>> PutRole(UpdateRoleDto role, Guid id)
        {
            ServiceResponse<GetRoleDto> resp = new ServiceResponse<GetRoleDto>();
            try
            {
                var rol = await _context.Role.FindAsync(id);

                if (RoleExists(id))
                {
                    _mapper.Map(role, rol);

                    await _context.SaveChangesAsync();

                    resp.Data = _mapper.Map<GetRoleDto>(rol);
                }
                else
                {
                    resp.Success = false;
                    resp.Message = "Rol no encotrado";
                }
            }
            catch (DbUpdateException ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
            }

            if (resp.Data == null)
            {
                return NotFound(resp);
            }

            return Ok(resp);
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetRoleDto>>>> PostCategory(AddRoleDto role)
        {
            var serviceResponse = new ServiceResponse<IEnumerable<GetRoleDto>>();

            Role rol = _mapper.Map<Role>(role);

            _context.Role.Add(rol);

            await _context.SaveChangesAsync();

            serviceResponse.Data = await _context.Role.Select(c => _mapper.Map<GetRoleDto>(c)).ToListAsync();

            return Ok(serviceResponse);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<GetRoleDto>>> DeleteRole(Guid id)
        {
            ServiceResponse<IEnumerable<GetRoleDto>> serviceResponse = new ServiceResponse<IEnumerable<GetRoleDto>>();

            try
            {
                Role rol = await _context.Role.FirstOrDefaultAsync(c => c.IdRole.ToString().ToUpper() == id.ToString().ToUpper());

                if (rol != null)
                {
                    _context.Role.Remove(rol);
                    await _context.SaveChangesAsync();

                    serviceResponse.Data = _context.Role.Select(c => _mapper.Map<GetRoleDto>(c)).ToList();
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Rol No Encontrado";

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

        private bool RoleExists(Guid id)
        {
            return _context.Role.Any(e => e.IdRole == id);
        }
    }
}
