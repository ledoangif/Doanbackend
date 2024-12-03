using Doantour.Helpers.Page;
using Doantour.Models;
using Doantour.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Doantour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseControllerGeneric<TEntity, TDto>(IBaseService<TEntity, TDto> service)
        : ControllerBase
    {
        protected readonly IBaseService<TEntity, TDto> _service = service;

        [HttpGet]
        public async Task<ActionResult<ResponseFormat>> GetAll()
        {
            var entities = await _service.ToListAsync();
            return new ResponseFormat(HttpStatusCode.OK, "Get List", entities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseFormat>> FindAsync(int id)
        {
            var entityDto = await _service.FindAsync(id);

            return new ResponseFormat(HttpStatusCode.OK, "Find By Id", entityDto);
        }

        [HttpPost]
        public virtual async Task<ResponseFormat> InsertAsync([FromForm] TDto dto)
        {

            var insertResult = await _service.InsertAsync(dto);
            if (insertResult == null)
            {
                return new ResponseFormat(HttpStatusCode.BadRequest, "Exist", insertResult);
            }

            return new ResponseFormat(HttpStatusCode.OK, "Insert Success", insertResult);
        }

        [HttpPut("{id}")]
        public virtual async Task<ResponseFormat> UpdateAsync(int id, TDto obj)
        {
            var result = await _service.UpdateAsync(id, obj);

            return new ResponseFormat(HttpStatusCode.Accepted, "Cập nhật thành công!", result);
        }

        [HttpDelete("{id}")]
        public virtual async Task<ResponseFormat> DeleteAsync(int id)
        {
            var result = await _service.DeleteAsync(id);
            return new ResponseFormat(HttpStatusCode.NoContent, "Xóa thành công!", result);
        }


        [HttpGet("GetAllPagination")]
        public async Task<ActionResult<ResponseFormat>> GetAllPagination([FromQuery] Pagination? pagination)
        {
            try
            {
                var result = await _service.GetAllPagination(pagination);
                return new ResponseFormat(HttpStatusCode.NoContent, "Get all pagination Success!", result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}