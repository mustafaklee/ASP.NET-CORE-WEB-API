using Microsoft.AspNetCore.Mvc;
using NZWalkUI.Models.DTO;
using System.Threading.Tasks;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }



        public async Task<IActionResult> Index()
        {
            List<RegionDto> response = new List<RegionDto>();

            //get all regions from web api
            try
            {
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("https://localhost:7208/api/regions");

                httpResponseMessage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());
               
            }
            catch(Exception e)
            {
                return BadRequest("Something went wrong");
            }

            return View(response);
        }
    }
}
