using Doantour.DTO;
using Doantour.Models;
using Doantour.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Doantour.Controllers
{
    public class CountryController : BaseControllerGeneric<Country, CountryDTO>
    {
        private readonly CountryService countryService;
        public CountryController(ServiceFactory service) : base(service.CountryService)
        {
            countryService = service.CountryService;
        }

        [HttpGet("search/{name}")]
        public async Task<ActionResult<ResponseFormat>> SearchAll(string name)
        {
            var entities = await _service.SearchAsync(x => x.CountryName.Contains(name));
            return new ResponseFormat(HttpStatusCode.OK, "Get List", entities);
        }

        [HttpGet("SearchCountryNameAdmin/{name}")]
        public async Task<ActionResult<ResponseFormat>> SearchCountryNameAdmin(string name)
        {
            var entities = await _service.SearchAsync(x => x.CountryName.Contains(name) && x.IsDeleted == false);
            return new ResponseFormat(HttpStatusCode.OK, "Get List", entities);
        }
        [HttpPost("InsertCountry")]
        public async Task<ActionResult<ResponseFormat>> Insert([FromForm] CountryDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CountryName) ||
                string.IsNullOrWhiteSpace(dto.CountryImage) ||
                string.IsNullOrWhiteSpace(dto.ContinentName))
            {
                return BadRequest("Thông tin này không được để trống");
            }
            var insertResult = await _service.InsertAsync(dto);
            return new ResponseFormat(HttpStatusCode.OK, "Thêm thành công", insertResult);
        }
        [HttpGet("Top4Country")]
        public async Task<ActionResult<ResponseFormat>> Top4Country()
        {
            var insertResult = await countryService.Top4Country();
            return new ResponseFormat(HttpStatusCode.OK, " Success", insertResult);
        }
        [HttpGet("Top6Country")]
        public async Task<ActionResult<ResponseFormat>> Top6Country()
        {
            var insertResult = await countryService.Next6Countries();
            return new ResponseFormat(HttpStatusCode.OK, " Success", insertResult);
        }
        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile ImageFile)
        {
            return Ok(await countryService.UploadFile(ImageFile));
        }

    }
}
