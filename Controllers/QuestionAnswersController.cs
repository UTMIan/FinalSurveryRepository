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
using FinalSurveyPractice.DTOs.QuestionAnswer;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace FinalSurveyPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionAnswersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public QuestionAnswersController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/QuestionAnswer
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetQuestionAnswerDto>>>> GetQuestionAnswer()
        {
            var resp = new ServiceResponse<IEnumerable<GetQuestionAnswerDto>>();

            var questionAnswer = await _context.QuestionAnswer.ToListAsync();

            resp.Data = questionAnswer.Select(c => _mapper.Map<GetQuestionAnswerDto>(c)).ToList();

            return Ok(resp);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetQuestionAnswerDto>>> GetQuestionAnswer(Guid id)
        {
            var resp = new ServiceResponse<GetQuestionAnswerDto>();
            var qstAns = await _context.QuestionAnswer.FirstOrDefaultAsync(c => c.IdQuestionAnswer.ToString().ToUpper() == id.ToString().ToUpper());

            if (qstAns != null)
            {
                resp.Data = _mapper.Map<GetQuestionAnswerDto>(qstAns);
            }
            else
            {
                resp.Success = false;
                resp.Message = "Respuesta de pregunta no encontrada";

                return NotFound(resp);
            }

            return Ok(resp);
        }

        // PUT: api/QuestionAnswer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<GetQuestionAnswerDto>>> PutQuestionAnswer(UpdateQuestionAnswerDto questAnswer, Guid id)
        {
            ServiceResponse<GetQuestionAnswerDto> resp = new ServiceResponse<GetQuestionAnswerDto>();
            try
            {
                var qstAns = await _context.Question.FindAsync(id);

                if (QuestionAnswerExists(id))
                {
                    _mapper.Map(questAnswer, qstAns);

                    await _context.SaveChangesAsync();

                    resp.Data = _mapper.Map<GetQuestionAnswerDto>(qstAns);
                }
                else
                {
                    resp.Success = false;
                    resp.Message = "Respuesta de pregunta no encontrada";
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

        // POST: api/QuestionAnswer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Admin")]

        public async Task<ActionResult<ServiceResponse<IEnumerable<GetQuestionAnswerDto>>>> PostQuestionAnswer(AddQuestionAnswerDto questAnswer)
        {
            var serviceResponse = new ServiceResponse<IEnumerable<GetQuestionAnswerDto>>();

            QuestionAnswer qstAns = _mapper.Map<QuestionAnswer>(questAnswer);

            _context.QuestionAnswer.Add(qstAns);

            await _context.SaveChangesAsync();

            serviceResponse.Data = await _context.QuestionAnswer.Select(c => _mapper.Map<GetQuestionAnswerDto>(c)).ToListAsync();

            return Ok(serviceResponse);
        }

        // DELETE: api/QuestionAnswers/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<GetQuestionAnswerDto>>> DeleteQuestionAnswer(Guid id)
        {
            ServiceResponse<IEnumerable<GetQuestionAnswerDto>> serviceResponse = new ServiceResponse<IEnumerable<GetQuestionAnswerDto>>();

            try
            {
                QuestionAnswer qstAns = await _context.QuestionAnswer.FirstOrDefaultAsync(c => c.IdQuestionAnswer.ToString().ToUpper() == id.ToString().ToUpper());

                if (qstAns != null)
                {
                    _context.QuestionAnswer.Remove(qstAns);
                    await _context.SaveChangesAsync();

                    serviceResponse.Data = _context.QuestionAnswer.Select(c => _mapper.Map<GetQuestionAnswerDto>(c)).ToList();
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Respuesta de pregunta no encontrada";

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

        private bool QuestionAnswerExists(Guid id)
        {
            return _context.QuestionAnswer.Any(e => e.IdQuestionAnswer == id);
        }
    }
}
