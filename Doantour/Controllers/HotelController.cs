using AutoMapper;
using Doantour.DTO;
using Doantour.Models;
using Doantour.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Doantour.Controllers
{
    public class HotelController : BaseControllerGeneric<Hotel, HotelDTO>
    {
        public HotelController(ServiceFactory
            service, IMapper mapper) : base(service.HotelService)
        {
        }
        [HttpGet("search/{name}")]
        public async Task<ActionResult<ResponseFormat>> SearchAll(string name)
        {
            var entities = await _service.SearchAsync(x => x.HotelName.Contains(name));
            return new ResponseFormat(HttpStatusCode.OK, "Get List", entities);
        }
    }
}
