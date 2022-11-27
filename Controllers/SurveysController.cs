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
using FinalSurveyPractice.DTOs.Survey;

namespace FinalSurveyPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveysController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public SurveysController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Survey
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetSurveyDto>>>> GetSurvey()
        {
            var resp = new ServiceResponse<IEnumerable<GetSurveyDto>>();

            var survey = await _context.Survey.ToListAsync();

            resp.Data = survey.Select(c => _mapper.Map<GetSurveyDto>(c)).ToList();

            return Ok(resp);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetSurveyDto>>> GetSurvey(Guid id)
        {
            var resp = new ServiceResponse<GetSurveyDto>();
            var surv = await _context.Survey.FirstOrDefaultAsync(c => c.IdSurvey.ToString().ToUpper() == id.ToString().ToUpper());

            if (surv != null)
            {
                resp.Data = _mapper.Map<GetSurveyDto>(surv);
            }
            else
            {
                resp.Success = false;
                resp.Message = "Encuesta no encontrada";

                return NotFound(resp);
            }

            return Ok(resp);
        }

        // PUT: api/Surveys/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<GetSurveyDto>>> PutSurvey(UpdateSurveyDto survey, int id)
        {
            ServiceResponse<GetSurveyDto> resp = new ServiceResponse<GetSurveyDto>();
            try
            {
                var cat = await _context.Survey.FindAsync(id);

                if (SurveyExists(id))
                {
                    _mapper.Map(survey, cat);

                    await _context.SaveChangesAsync();

                    resp.Data = _mapper.Map<GetSurveyDto>(cat);
                }
                else
                {
                    resp.Success = false;
                    resp.Message = "Encuesta no encontrada";
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

        // POST: api/Survey
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetSurveyDto>>>> PostSurvey(AddSurveyDto survey)
        {
            var serviceResponse = new ServiceResponse<IEnumerable<GetSurveyDto>>();

            Survey cat = _mapper.Map<Survey>(survey);

            _context.Survey.Add(cat);

            await _context.SaveChangesAsync();

            serviceResponse.Data = await _context.Survey.Select(c => _mapper.Map<GetSurveyDto>(c)).ToListAsync();

            return Ok(serviceResponse);
        }

        // DELETE: api/Surveys/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<GetSurveyDto>>> DeleteSurvey(Guid id)
        {
            ServiceResponse<IEnumerable<GetSurveyDto>> serviceResponse = new ServiceResponse<IEnumerable<GetSurveyDto>>();

            try
            {
                Survey surv = await _context.Survey.FirstOrDefaultAsync(c => c.IdSurvey.ToString().ToUpper() == id.ToString().ToUpper());

                if (surv != null)
                {
                    _context.Survey.Remove(surv);
                    await _context.SaveChangesAsync();

                    serviceResponse.Data = _context.Survey.Select(c => _mapper.Map<GetSurveyDto>(c)).ToList();
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

        private bool SurveyExists(int id)
        {
            return _context.Survey.Any(e => e.IdSurvey == id);
        }
    }
}
