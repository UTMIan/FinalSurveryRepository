using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalSurveyPractice.Data;
using FinalSurveyPractice.Models;
using FinalSurveyPractice.DTOs.UserAnswer;
using AutoMapper;

namespace FinalSurveyPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAnswersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserAnswersController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/UserAnswers
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetUserAnswerDto>>>> GetUserAnswer()
        {
            var resp = new ServiceResponse<IEnumerable<GetUserAnswerDto>>();

            var userAnswer = await _context.UserAnswer.ToListAsync();

            resp.Data = userAnswer.Select(c => _mapper.Map<GetUserAnswerDto>(c)).ToList();

            return Ok(resp);
        }

        public async Task<ActionResult<ServiceResponse<GetUserAnswerDto>>> GetUserAnswer(Guid id)
        {
            var resp = new ServiceResponse<GetUserAnswerDto>();
            var usresp = await _context.UserAnswer.FirstOrDefaultAsync(c => c.IdUserAnswer.ToString().ToUpper() == id.ToString().ToUpper());

            if (usresp != null)
            {
                resp.Data = _mapper.Map<GetUserAnswerDto>(usresp);
            }
            else
            {
                resp.Success = false;
                resp.Message = "Respuesta de usuario no encontrada";

                return NotFound(resp);
            }

            return Ok(resp);
        }

        // PUT: api/UserAnswers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<GetUserAnswerDto>>> PutUserAnswer(UpdateUserAnswerDto userAnswer, Guid id)
        {
            ServiceResponse<GetUserAnswerDto> resp = new ServiceResponse<GetUserAnswerDto>();
            try
            {
                var usans = await _context.UserAnswer.FindAsync(id);

                if (UserAnswerExists(id))
                {
                    _mapper.Map(userAnswer, usans);

                    await _context.SaveChangesAsync();

                    resp.Data = _mapper.Map<GetUserAnswerDto>(usans);
                }
                else
                {
                    resp.Success = false;
                    resp.Message = "Respuesta de usuario no encontrada";
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

        // POST: api/UserAnswers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetUserAnswerDto>>>> PostUserAnswer(AddUserAnswerDto category)
        {
            var serviceResponse = new ServiceResponse<IEnumerable<GetUserAnswerDto>>();

            UserAnswer usans = _mapper.Map<UserAnswer>(category);

            _context.UserAnswer.Add(usans);

            await _context.SaveChangesAsync();

            serviceResponse.Data = await _context.UserAnswer.Select(c => _mapper.Map<GetUserAnswerDto>(c)).ToListAsync();

            return Ok(serviceResponse);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<GetUserAnswerDto>>> DeleteUserAnswer(Guid id)
        {
            ServiceResponse<IEnumerable<GetUserAnswerDto>> serviceResponse = new ServiceResponse<IEnumerable<GetUserAnswerDto>>();

            try
            {
                UserAnswer usans = await _context.UserAnswer.FirstOrDefaultAsync(c => c.IdUserAnswer.ToString().ToUpper() == id.ToString().ToUpper());

                if (usans != null)
                {
                    _context.UserAnswer.Remove(usans);
                    await _context.SaveChangesAsync();

                    serviceResponse.Data = _context.UserAnswer.Select(c => _mapper.Map<GetUserAnswerDto>(c)).ToList();
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Categoria No Encontrado";

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

        private bool UserAnswerExists(Guid id)
        {
            return _context.UserAnswer.Any(e => e.IdUserAnswer == id);
        }
    }
}
