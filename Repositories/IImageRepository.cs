using MyFirstApiProject.Models.Domain;
using System.Net;

namespace MyFirstApiProject.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
