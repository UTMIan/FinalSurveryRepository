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
using FinalSurveyPractice.DTOs.Question;

namespace FinalSurveyPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public QuestionsController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Question
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetQuestionDto>>>> GetQuestion()
        {
            var resp = new ServiceResponse<IEnumerable<GetQuestionDto>>();

            var question = await _context.Question.ToListAsync();

            resp.Data = question.Select(c => _mapper.Map<GetQuestionDto>(c)).ToList();

            return Ok(resp);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetQuestionDto>>> GetQuestion(Guid id)
        {
            var resp = new ServiceResponse<GetQuestionDto>();
            var question = await _context.Category.FirstOrDefaultAsync(c => c.IdCategory.ToString().ToUpper() == id.ToString().ToUpper());

            if (question != null)
            {
                resp.Data = _mapper.Map<GetQuestionDto>(question);
            }
            else
            {
                resp.Success = false;
                resp.Message = "Pregunta no encontrada";

                return NotFound(resp);
            }

            return Ok(resp);
        }

        // PUT: api/Questions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<GetQuestionDto>>> PutQuestion(UpdateQuestionDto question, Guid id)
        {
            ServiceResponse<GetQuestionDto> resp = new ServiceResponse<GetQuestionDto>();
            try
            {
                var quest = await _context.Question.FindAsync(id);

                if (QuestionExists(id))
                {
                    _mapper.Map(question, quest);

                    await _context.SaveChangesAsync();

                    resp.Data = _mapper.Map<GetQuestionDto>(quest);
                }
                else
                {
                    resp.Success = false;
                    resp.Message = "Pregunta no encontrada";
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

        // POST: api/Question
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetQuestionDto>>>> PostCategory(AddQuestionDto question)
        {
            var serviceResponse = new ServiceResponse<IEnumerable<GetQuestionDto>>();

            Question quest = _mapper.Map<Question>(question);

            _context.Question.Add(quest);

            await _context.SaveChangesAsync();

            serviceResponse.Data = await _context.Question.Select(c => _mapper.Map<GetQuestionDto>(c)).ToListAsync();

            return Ok(serviceResponse);
        }

        // DELETE: api/Questions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<GetQuestionDto>>> DeleteQuestion(Guid id)
        {
            ServiceResponse<IEnumerable<GetQuestionDto>> serviceResponse = new ServiceResponse<IEnumerable<GetQuestionDto>>();

            try
            {
                Question quest = await _context.Question.FirstOrDefaultAsync(c => c.IdQuestion.ToString().ToUpper() == id.ToString().ToUpper());

                if (quest != null)
                {
                    _context.Question.Remove(quest);
                    await _context.SaveChangesAsync();

                    serviceResponse.Data = _context.Question.Select(c => _mapper.Map<GetQuestionDto>(c)).ToList();
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Pregunta no encotrada";

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

        private bool QuestionExists(Guid id)
        {
            return _context.Question.Any(e => e.IdQuestion == id);
        }
    }
}
