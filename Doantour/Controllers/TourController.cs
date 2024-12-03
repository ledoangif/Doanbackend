using Doantour.DTO;
using Doantour.Extention;
using Doantour.Helpers;
using Doantour.Helpers.Page;
using Doantour.Models;
using Doantour.Service;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Net;

namespace Doantour.Controllers
{
    public class TourController : BaseControllerGeneric<Tour, TourDTO>
    {
        private readonly TourService _tourService;
        private readonly SendMailService emailService;
        public TourController(ServiceFactory service) : base(service.TourService)
        {
            _tourService = service.TourService;
            emailService = service.SendMailService;
        }
        [HttpGet("SearchTour")]
        public async Task<ResponseFormat> SearchByAsync(string? placeStart, string? placeEnd, int? CountryId, DateTime? dateStart, DateTime? dateEnd, Decimal? Price, string? name)
        {
            Expression<Func<Tour, bool>> searchCondition = x => true;

            if (!string.IsNullOrEmpty(placeStart))
            {
                searchCondition = searchCondition.And(x => x.placeStart.Contains(placeStart));
            }
            if (!string.IsNullOrEmpty(name))
            {
                searchCondition = searchCondition.And(x => x.NameTour.Contains(name));
            }
            if (!string.IsNullOrEmpty(placeEnd))
            {
                searchCondition = searchCondition.And(x => x.placeEnd.Contains(placeEnd));
            }

            if (dateStart.HasValue)
            {
                searchCondition = searchCondition.And(x => x.DateStart >= dateStart.Value);
            }
            if (dateEnd.HasValue)
            {
                searchCondition = searchCondition.And(x => x.DateEnd <= dateEnd.Value);
            }
            if (Price.HasValue)
            {
                searchCondition = searchCondition.And(x => x.PriceSale <= Price.Value);
            }
            if (CountryId.HasValue)
            {
                searchCondition = searchCondition.And(x => x.CountryId == CountryId);
            }
            var today = DateTime.Today;
            searchCondition = searchCondition.And(x => x.DateStart != today.AddDays(1) && x.IsDeleted == false && x.DateStart > DateTime.Now);
            var result = await _service.SearchAsync(searchCondition);

            return new ResponseFormat(HttpStatusCode.OK, "Search Success!", result);
        }
        [HttpGet("SearchTourAdmin")]
        public async Task<ResponseFormat> SearchTourAdmin(string? placeStart, string? placeEnd, int? CountryId, DateTime? dateStart, DateTime? dateEnd, Decimal? Price, string? name)
        {
            Expression<Func<Tour, bool>> searchCondition = x => true;

            if (!string.IsNullOrEmpty(placeStart))
            {
                searchCondition = searchCondition.And(x => x.placeStart.Contains(placeStart));
            }
            if (!string.IsNullOrEmpty(name))
            {
                searchCondition = searchCondition.And(x => x.NameTour.Contains(name));
            }
            if (!string.IsNullOrEmpty(placeEnd))
            {
                searchCondition = searchCondition.And(x => x.placeEnd.Contains(placeEnd));
            }

            if (dateStart.HasValue)
            {
                searchCondition = searchCondition.And(x => x.DateStart >= dateStart.Value);
            }
            if (dateEnd.HasValue)
            {
                searchCondition = searchCondition.And(x => x.DateEnd <= dateEnd.Value);
            }
            if (Price.HasValue)
            {
                searchCondition = searchCondition.And(x => x.PriceSale <= Price.Value);
            }
            if (CountryId.HasValue)
            {
                searchCondition = searchCondition.And(x => x.CountryId == CountryId);
            }
            var today = DateTime.Today;
            searchCondition = searchCondition.And(x => x.IsFinish == Constants.NotFinishTour && x.IsDeleted == false);
            var result = await _service.SearchAsync(searchCondition);

            return new ResponseFormat(HttpStatusCode.OK, "Search Success!", result);
        }

        [HttpGet("search/{name}")]
        public async Task<ActionResult<ResponseFormat>> Search(string name)
        {
            var entities = await _service.SearchAsync(x => x.NameTour.Contains(name));
            return new ResponseFormat(HttpStatusCode.OK, "Get List", entities);
        }
        [HttpPost("InsertTour")]
        public async Task<ResponseFormat> InsertTour([FromForm] TourDTO dto)
        {
            await _tourService.InsertAsync(dto);
            return new ResponseFormat(HttpStatusCode.OK, "Thêm thành công", dto);

        }
        [HttpGet("GetTourIsLocal")]
        public async Task<ResponseFormat> GetTourIsLocal()
        {
            var result = await _service.SearchAsync(x => x.IsLocal == true);
            return new ResponseFormat(HttpStatusCode.OK, "Search  Success", result);

        }
        [HttpGet("GetTourNotIsLocal")]
        public async Task<ResponseFormat> GetTourNotIsLocal()
        {
            var result = await _service.SearchAsync(x => x.IsLocal == false);
            return new ResponseFormat(HttpStatusCode.OK, "Search Success", result);

        }
        [HttpGet("GetToursByCountry")]
        public async Task<ResponseFormat> GetToursByCountry(int countryId)
        {
            var result = await _service.SearchAsync(x => x.CountryId == countryId);

            return new ResponseFormat(HttpStatusCode.OK, "Search Success", result);
        }
        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile ImageFile)
        {
            if (ImageFile == null || ImageFile.Length == 0)
            {
                return BadRequest("Invalid file.");
            }

            return Ok(await _tourService.UploadFile(ImageFile));
        }

        [HttpPut("UpdateIsFinishTour")]
        public async Task<ResponseFormat> UpdateIsFinishTour(List<TourDTO> tour)
        {
            var item = await _tourService.UpdateIsFinishTour(tour);
            return new ResponseFormat(HttpStatusCode.OK, "Cập nhật thành công", null);
        }
        [HttpGet("Top6TourNewUpdate")]
        public async Task<ActionResult<ResponseFormat>> Top6Tour()
        {
            var listTour = await _tourService.Top6Tour();
            return new ResponseFormat(HttpStatusCode.OK, "Get Success", listTour);
        }
        [HttpGet("GetTourNotFinish")]
        public async Task<ActionResult<ResponseFormat>> GetTourNotFinish()
        {
            var listTour = await _tourService.ToListAsync();
            return new ResponseFormat(HttpStatusCode.OK, "Get Success", listTour);
        }
        [HttpGet("GetAllTour")]
        public async Task<ActionResult<ResponseFormat>> GetAllTour()
        {
            var listTour = await _tourService.ToListAll();
            return new ResponseFormat(HttpStatusCode.OK, "Get Success", listTour);
        }
        [HttpGet("GetAllTourOndeDaybeforeStart")]
        public async Task<ActionResult<ResponseFormat>> GetAllTourOndeDaybeforeStart()
        {
            var listTour = await _tourService.TourOndeDayBeforeStart();
            return new ResponseFormat(HttpStatusCode.OK, "Get Success", listTour);
        }
        [HttpGet("GetTourDifferent")]
        public async Task<ActionResult<ResponseFormat>> GetTourDifferent(string name, DateTime date)
        {
            var listTour = await _tourService.SearchTourDifferent(name, date);
            return new ResponseFormat(HttpStatusCode.OK, "Get Success", listTour);
        }
        [HttpGet("GetCustomerByTour")]
        public async Task<ActionResult<ResponseFormat>> GetCustomerByTour(int id)
        {
            var listResult = await _tourService.GetCustomerByTour(id);
            return new ResponseFormat(HttpStatusCode.OK, "Get Success", listResult);
        }

        [HttpGet("GetAllPaginationBeforeStartOneDay")]
        public async Task<ActionResult<ResponseFormat>> GetAllPaginationBeforeStartOneDay([FromQuery] Pagination? pagination)
        {
            try
            {
                var result = await _tourService.GetAllPaginationBeforeStartOneDay(pagination);
                return new ResponseFormat(HttpStatusCode.NoContent, "Get all pagination Success!", result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
