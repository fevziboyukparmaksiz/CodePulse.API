using System.Collections;
using AutoMapper;
using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.Dtos;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryController(IMapper mapper, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto createCategoryRequestDto)
        {
            var category = _mapper.Map<Category>(createCategoryRequestDto);
            await _categoryRepository.CreateAsync(category);

            return Ok(category);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            return Ok(categoryDtos);
        }

    }
}
