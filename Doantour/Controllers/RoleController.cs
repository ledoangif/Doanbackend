using Doantour.DTO;
using Doantour.Models;
using Doantour.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Doantour.Controllers
{
    public class RoleController : BaseControllerGeneric<Role, RoleDTO>
    {
        public RoleController(ServiceFactory service) : base(service.RoleService)
        {
        }

        [HttpGet("search/{name}")]
        public async Task<ActionResult<ResponseFormat>> SearchAll(string name)
        {
            var entities = await _service.SearchAsync(x => x.RoleName == name);
            return new ResponseFormat(HttpStatusCode.OK, "Get List", entities);
        }

    }

}
