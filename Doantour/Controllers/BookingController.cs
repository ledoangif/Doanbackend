using Doantour.DTO;
using Doantour.Hubs;
using Doantour.Models;
using Doantour.Service;
using Doantour.Helpers;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;

namespace Doantour.Controllers
{
    public class BookingController : BaseControllerGeneric<Booking, BookingDTO>
    {
        private readonly IHubContext<BookingHub> _hubContext;
        private readonly BookingService bookingService;
        private readonly SendMailService emailService;
        private readonly CustomerService customerService;
        public BookingController(IHubContext<BookingHub> hubContext, ServiceFactory service) : base(service.BookingService)
        {
            _hubContext = hubContext;
            emailService = service.SendMailService;
            bookingService = service.BookingService;
            customerService = service.CustomerService;

        }


        [HttpPost("TestSendMailByStatus")]
        public async Task<IActionResult> TestSendMailByStatus([FromForm] string to, [FromForm] string status, int id)
        {
            BackgroundJob.Schedule(() => emailService.SendBookingStatusEmailAsync(to, status, id), TimeSpan.FromSeconds(1));
            //return Ok(new { message = "Email scheduled to be sent in 1 seconds" });
            return Ok(new { message = "Email sẽ được lên lịch gửi" });
        }
        [HttpGet("GetTourDetail")]
        public virtual async Task<ResponseFormat> GetTourDetail(int bookingID)
        {
            var booking = await emailService.GetTourDetailsAsync(bookingID);
            return new ResponseFormat(HttpStatusCode.OK, "Get  Success", booking);
        }
        [HttpPost("InsertBooking")]
        public virtual async Task<ResponseFormat> InsertBooking([FromForm] BookingDTO dto)
        {
            var insertResult = await bookingService.InsertAsync(dto);

            if (insertResult == null)
            {
                return new ResponseFormat(HttpStatusCode.BadRequest, "Sold out ticket", null);
            }

            return new ResponseFormat(HttpStatusCode.OK, "Insert Success", insertResult);
        }
        [HttpGet("GetBookingall")]
        public async Task<ResponseFormat> GetBookingall()
        {
            var result = await _service.SearchAsync(x => x.StatusBill != Constants.Save && x.IsDeleted == false);
            return new ResponseFormat(HttpStatusCode.OK, "Search  Success", result);
        }
        [HttpGet("SearchBookingByid")]
        public virtual async Task<ResponseFormat> SearchBookingByid(int id)
        {
            var insertResult = await bookingService.FindAsync(id);

            if (insertResult == null)
            {
                return new ResponseFormat(HttpStatusCode.BadRequest, "Search fail", null);
            }

            return new ResponseFormat(HttpStatusCode.OK, "Search Success", insertResult);
        }
        
        [HttpGet("GetBookingPending")]
        public async Task<ResponseFormat> GetBookingPending()
        {
            var result = await _service.SearchAsync(x => x.StatusBill== Constants.Pending && x.IsDeleted == false);
            return new ResponseFormat(HttpStatusCode.OK, "Search  Success", result);

        }
        [HttpGet("SearchBooking")]
        public virtual async Task<ResponseFormat> SearchBooking(int id)
        {
            var insertResult = await bookingService.FindAsync(id);

            if (insertResult == null)
            {
                return new ResponseFormat(HttpStatusCode.BadRequest, "Search fail", null);
            }

            return new ResponseFormat(HttpStatusCode.OK, "Search Success", insertResult);
        }
        [HttpGet("SearchBookingcustomer")]
        public virtual async Task<ResponseFormat> SearchBookingcustomer(int id)
        {
            var insertResult = await bookingService.SearchAsync(x => x.CustomerId == id && x.StatusBill != Constants.Save && x.IsDeleted == false);
            if (insertResult == null)
            {
                return new ResponseFormat(HttpStatusCode.BadRequest, "Search fail", null);
            }

            return new ResponseFormat(HttpStatusCode.OK, "Search Success", insertResult);
        }
        [HttpGet("SearchBookingnew")]
        public virtual async Task<ResponseFormat> SearchBookingnew(int id)
        {
            var insertResult = await bookingService.SearchAsync(x => x.Id ==id && x.StatusBill != Constants.Save && x.IsDeleted == false);
            if (insertResult == null)
            {
                return new ResponseFormat(HttpStatusCode.BadRequest, "Search fail", null);
            }

            return new ResponseFormat(HttpStatusCode.OK, "Search Success", insertResult);
        }
        [HttpGet("SearchBooking2")]
        public virtual async Task<ResponseFormat> SearchBooking2(string email)
        {
            var insertResult = await customerService.SearchAsync(x => x.Email == email);
            if (insertResult == null)
            {
                return new ResponseFormat(HttpStatusCode.BadRequest, "Search fail", null);
            }

            return new ResponseFormat(HttpStatusCode.OK, "Search Success", insertResult);
        }
        [HttpGet("SearchBookingbyEmail")]
        public virtual async Task<ResponseFormat> SearchBookingbyEmail(int customerId)
        {
            var insertResult = await bookingService.SearchAsync(x => x.CustomerId == customerId);
            if (insertResult == null)
            {
                return new ResponseFormat(HttpStatusCode.BadRequest, "Search fail", null);
            }

            return new ResponseFormat(HttpStatusCode.OK, "Search Success", insertResult);
        }
        [HttpGet("CountPending")]
        public virtual async Task<ResponseFormat> CountPending()
        {
            var insertResult = await bookingService.CountPending();

            if (insertResult == null)
            {
                return new ResponseFormat(HttpStatusCode.BadRequest, "Search fail", null);
            }

            return new ResponseFormat(HttpStatusCode.OK, "Search Success", insertResult);
        }

        [HttpGet("ToTalBillEachMonth")]
        public virtual async Task<ResponseFormat> ToTalBillEachMonth(int year)
        {
            var insertResult = await bookingService.ToTalBillEachMonth(year);

            if (insertResult == null)
            {
                return new ResponseFormat(HttpStatusCode.BadRequest, "Search fail", null);
            }

            return new ResponseFormat(HttpStatusCode.OK, "Search Success", insertResult);
        }
        [HttpGet("CountBookingCancelorNotCancel")]
        public virtual async Task<ResponseFormat> CountBookingCancelorNotCancel(string status)
        {
            var insertResult = await bookingService.CountBookingCancelorNotCancel(status);

            if (insertResult == null)
            {
                return new ResponseFormat(HttpStatusCode.BadRequest, "Search fail", null);
            }

            return new ResponseFormat(HttpStatusCode.OK, "Search Success", insertResult);
        }
        //[HttpGet("getTourPaying")]
        //public virtual async Task<ResponseFormat> getTourPayinggetTourPaying()
        //{
        //    var insertResult = await bookingService.getTourPaying();

        //    if (insertResult == null)
        //    {
        //        return new ResponseFormat(HttpStatusCode.BadRequest, "Search fail", null);
        //    }

        //    return new ResponseFormat(HttpStatusCode.OK, "Search Success", insertResult);
        //}
        [HttpPost("resetSlotAfterCancel")]
        public virtual async Task<ResponseFormat> resetSlotAfterCancel(int child, int adult, int tourId)
        {
            var insertResult = await bookingService.resetSlotAfterCancel(child, adult, tourId);

            if (insertResult == null)
            {
                return new ResponseFormat(HttpStatusCode.BadRequest, " fail", null); 
            }

            return new ResponseFormat(HttpStatusCode.OK, " Success", insertResult);
        }
        //[HttpPut("UpdateBookingStatus")]
        //public async Task<IActionResult> UpdateBookingStatus(int id)
        //{
        //    try
        //    {
        //        var updatedBooking = await bookingService.UpdateStatusToUnpaidAsync(id);
        //        return Ok(updatedBooking);
        //    }
        //    catch (BadHttpRequestException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //Cập nhật trạng thái khi đặt cọc thành công
        [HttpPut("UpdateStatusToUnpaid")]
        public async Task<IActionResult> UpdateStatusToUnpaid(int bookingId)
        {
            // Gọi service để cập nhật trạng thái và lấy thông tin email và statusBill
            var (email, statusBill) = await bookingService.UpdateStatusToUnpaidAsync(bookingId);

            if (email == null || statusBill == null)
            {
                return BadRequest("Cập nhật trạng thái thất bại hoặc không tìm thấy thông tin.");
            }

            // Trả về thông tin email và statusBill
            return Ok(new
            {
                email,
                statusBill,
                
            });
        }
        // Lấy thông tin khách hàng khi đặt cọc thành công
        [HttpGet("GetBookingDetails")]
        public async Task<IActionResult> GetBookingDetails(int bookingId)
        {
            // Gọi service để lấy thông tin khách hàng và tên tour
            var (customerName, tourName,child,adult, depositAmount) = await bookingService.GetBookingDetailsAsync(bookingId);

            if (customerName == null || tourName == null)
            {
                return NotFound("Không tìm thấy thông tin booking.");
            }

            // Trả về thông tin tên khách hàng và tên tour
            return Ok(new
            {
                CustomerName = customerName,
                TourName = tourName,
                Child = child,
                Adult = adult,
                DepositAmount = depositAmount
            });
        }







    }
}
