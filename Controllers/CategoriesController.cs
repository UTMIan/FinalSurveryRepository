using AutoMapper;
using FinalSurveyPractice.Data;
using FinalSurveyPractice.DTOs.Category;
using FinalSurveyPractice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalSurveyPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CategoriesController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetCategoryDto>>>> GetCategory()
        {
            var resp = new ServiceResponse<IEnumerable<GetCategoryDto>>();

            var category = await _context.Category.ToListAsync();

            resp.Data = category.Select(c => _mapper.Map<GetCategoryDto>(c)).ToList();

            return Ok(resp);
        }

        public async Task<ActionResult<ServiceResponse<GetCategoryDto>>> GetCategory(Guid id)
        {
            var resp = new ServiceResponse<GetCategoryDto>();
            var cat = await _context.Category.FirstOrDefaultAsync(c => c.IdCategory.ToString().ToUpper() == id.ToString().ToUpper());

            if (cat != null)
            {
                resp.Data = _mapper.Map<GetCategoryDto>(cat);
            }
            else
            {
                resp.Success = false;
                resp.Message = "Category not found";

                return NotFound(resp);
            }

            return Ok(resp);
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCategoryDto>>> PutCategory(UpdateCategoryDto category, Guid id)
        {
            ServiceResponse<GetCategoryDto> resp = new();
            try
            {
                var cat = await _context.Category.FindAsync(id);

                if (CategoryExists(id))
                {
                    _mapper.Map(category, cat);

                    await _context.SaveChangesAsync();

                    resp.Data = _mapper.Map<GetCategoryDto>(cat);
                }
                else
                {
                    resp.Success = false;
                    resp.Message = "Category not found";
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
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetCategoryDto>>>> PostCategory(AddCategoryDto category)
        {
            var serviceResponse = new ServiceResponse<IEnumerable<GetCategoryDto>>();

            Category cat = _mapper.Map<Category>(category);

            _context.Category.Add(cat);

            await _context.SaveChangesAsync();

            serviceResponse.Data = await _context.Category.Select(c => _mapper.Map<GetCategoryDto>(c)).ToListAsync();

            return Ok(serviceResponse);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCategoryDto>>> DeleteCategory(Guid id)
        {
            ServiceResponse<IEnumerable<GetCategoryDto>> serviceResponse = new();

            try
            {
                Category cat = await _context.Category.FirstOrDefaultAsync(c => c.IdCategory.ToString().ToUpper() == id.ToString().ToUpper());

                if (cat != null)
                {
                    _context.Category.Remove(cat);
                    await _context.SaveChangesAsync();

                    serviceResponse.Data = _context.Category.Select(c => _mapper.Map<GetCategoryDto>(c)).ToList();
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Categoria no se encontró";

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

        private bool CategoryExists(Guid id)
        {
            return _context.Category.Any(e => e.IdCategory == id);
        }
    }
}
