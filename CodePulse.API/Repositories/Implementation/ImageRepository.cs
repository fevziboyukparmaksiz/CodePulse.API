using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories.Interface;

namespace CodePulse.API.Repositories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _dbContext;

        public ImageRepository(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext)
        {
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            //Upload the image to API/Images
            var localPath = Path.Combine(_environment.ContentRootPath, "Images", $"{blogImage.FileName}{blogImage.FileExtension}");
            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            //Upload to Database
            //https://codepulse.com/images/somefilename.jpg
            var request = _httpContextAccessor.HttpContext.Request;
            var urlPath =
                $"{request.Scheme}://{request.Host}/Images/{blogImage.FileName}{blogImage.FileExtension}";

            blogImage.Url = urlPath;

            await _dbContext.BlogImages.AddAsync(blogImage);
            await _dbContext.SaveChangesAsync();

            return blogImage;
        }
    }
}
