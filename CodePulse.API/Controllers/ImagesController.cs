using CodePulse.API.Models.Domain;
using CodePulse.API.Models.Dtos;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await _imageRepository.GetAll();

            var response = new List<BlogImageDto>();
            foreach (var image in images)
            {
                response.Add(new BlogImageDto()
                {
                    Id = image.Id,
                    CreatedDate = image.CreatedDate,
                    FileExtension = image.FileExtension,
                    FileName = image.FileName,
                    Title = image.Title,
                    Url = image.Url
                });
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName, [FromForm] string title)
        {
            ValidateFileUpload(file);
            if (ModelState.IsValid)
            {
                var blogImage = new BlogImage()
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName,
                    Title = title,
                    CreatedDate = DateTime.Now
                };

                blogImage = await _imageRepository.Upload(file, blogImage);

                var response = new BlogImageDto()
                {
                    Id = blogImage.Id,
                    CreatedDate = blogImage.CreatedDate,
                    FileExtension = blogImage.FileExtension,
                    FileName = blogImage.FileName,
                    Title = blogImage.Title,
                    Url = blogImage.Url
                };

                return Ok(response);
            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtension = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtension.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "unsupported file format");
            }

            if (file.Length > 1048576)
            {
                ModelState.AddModelError("file", "file size cannot be more than 10MB");
            }
        }
    }
}
