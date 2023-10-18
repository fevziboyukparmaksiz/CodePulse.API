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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoriesController(IMapper mapper, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            return Ok(categoryDtos);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var category = await _categoryRepository.GetById(id);

            if (category is null)
            {
                return NotFound();
            }

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return Ok(categoryDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto createCategoryRequestDto)
        {
            var category = _mapper.Map<Category>(createCategoryRequestDto);
            await _categoryRepository.CreateAsync(category);

            return Ok(category);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, UpdateCategoryRequestDto updateCategoryRequestDto)
        {
            var category = new Category()
            {
                Id = id,
                Name = updateCategoryRequestDto.Name,
                UrlHandle = updateCategoryRequestDto.UrlHandle
            };
            var response = await _categoryRepository.UpdateAsync(category);

            if (response is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CategoryDto>(category));
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var category = await _categoryRepository.DeleteAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return NoContent();

        }
    }
}
