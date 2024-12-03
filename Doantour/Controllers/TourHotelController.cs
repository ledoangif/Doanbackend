using Doantour.DTO;
using Doantour.Helpers.Page;
using Doantour.Models;
using Doantour.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Doantour.Controllers
{
    public class TourHotelController : BaseControllerGeneric<TourHotel, TourHotelDTO>
    {
        private readonly TourHotelService _TourHotelService;
        public TourHotelController(ServiceFactory
            service) : base(service.TourHotelService)
        {

            _TourHotelService = service.TourHotelService;
        }

        
        [HttpGet("Pagination")]
        public async Task<ActionResult<ResponseFormat>> GetAllPagination([FromQuery] Pagination? pagination, [FromQuery] int id)
        {
            try
            {
                var result = await _TourHotelService.GetAllPagination2(pagination, id);
                return new ResponseFormat(HttpStatusCode.NoContent, "Get all pagination Success!", result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
