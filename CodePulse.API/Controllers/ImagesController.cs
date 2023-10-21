﻿using CodePulse.API.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {

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
            }
        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtension = new string[] { "jpg", "jpeg", "png" };

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