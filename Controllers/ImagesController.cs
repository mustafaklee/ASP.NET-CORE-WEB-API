using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFirstApiProject.Models.Domain;
using MyFirstApiProject.Models.DTO;
using MyFirstApiProject.Repositories;


namespace MyFirstApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository ımageRepository;
        public ImagesController(IImageRepository ımageRepository)
        {
            this.ımageRepository = ımageRepository;
        }

        //POST:api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto ımageUploadRequestDto)
        {
            ValidateFileUpload(ımageUploadRequestDto);

            if (ModelState.IsValid)
            {
                var imageDomainModel = new Image
                {
                    File = ımageUploadRequestDto.File,
                    FileExtension = Path.GetExtension(ımageUploadRequestDto.File.FileName),
                    FileSizeInBytes = ımageUploadRequestDto.File.Length,
                    FileName = ımageUploadRequestDto.FileName,
                    FileDescription = ımageUploadRequestDto.FileDescription,
                };

                //user repository to upload image
                await ımageRepository.Upload(imageDomainModel);

                return Ok(imageDomainModel);

            }
            return BadRequest(ModelState);

        }


        private void ValidateFileUpload(ImageUploadRequestDto ımageUploadRequestDto)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(ımageUploadRequestDto.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }

            if (ımageUploadRequestDto.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "file size more than 10MB , please upload a smaller size file");
            }

        }
    }
}
