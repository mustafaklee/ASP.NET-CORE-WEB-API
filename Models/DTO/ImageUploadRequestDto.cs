using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MyFirstApiProject.Models.DTO
{
    public class ImageUploadRequestDto
    {
        [Required]
        public IFormFile File { get; set; }

        [Required]
        public string FileName { get; set; }

        public string? FileDescription { get; set; }


    }
}
