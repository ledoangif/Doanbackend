using Doantour.DTO;
using Doantour.Models;
using Doantour.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Doantour.Controllers
{
    public class CustomerController : BaseControllerGeneric<Customer, CustomerDTO>
    {
        private readonly CustomerService customerService;
        public CustomerController(ServiceFactory service) : base(service.CustomerService)
        {
            customerService = service.CustomerService;
        }
        [HttpGet("search")]
        public async Task<ActionResult<ResponseFormat>> SearchAll(string email, string phone)
        {
            var entities = await _service.SearchAsync(x => x.Email == email || x.PhoneNumber == phone);
            return new ResponseFormat(HttpStatusCode.OK, "Get List", entities);
        }
        [HttpGet("searchByPhone")]
        public async Task<ActionResult<ResponseFormat>> searchByPhone(string phone)
        {
            var entities = await _service.SearchAsync(x => x.PhoneNumber == phone);
            return new ResponseFormat(HttpStatusCode.OK, "Get List", entities);
        }

        
        //[HttpPost("Insert")]
        //public async Task<ResponseFormat> InsertCustomer([FromForm] CustomerDTO dto)
        //{
        //    await _service.InsertAsync(dto);
        //    return new ResponseFormat(HttpStatusCode.OK, "Insert Success", dto);

        //}
        [HttpPost("Insert")]
        public async Task<ResponseFormat> InsertCustomer([FromForm] CustomerDTO dto)
        {

            // Tiến hành gọi service để insert dữ liệu vào cơ sở dữ liệu nếu không có trường rỗng
            await _service.InsertAsync(dto);

            // Trả về phản hồi thành công nếu dữ liệu hợp lệ
            return new ResponseFormat(HttpStatusCode.OK, "Insert Success", dto);
        }
    }
}