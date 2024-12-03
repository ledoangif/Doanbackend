using Doantour.DTO;
using Doantour.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Doantour.Controllers
{

    [ApiController]
        [Route("api/[controller]")]
        public class PaymentController : ControllerBase
        {
            private readonly IVnPayService _vnPayService;

            public PaymentController(IVnPayService vnPayService)
            {
                _vnPayService = vnPayService;
            }

            [HttpPost("create")]
            public IActionResult CreatePaymentUrl([FromBody] PaymentInformationModel model)
            {
                var url = _vnPayService.CreatePaymentUrl(model, HttpContext);
                return Ok(new { PaymentUrl = url });
            }


        //[HttpGet("vnpay_return")]
        //public IActionResult VnPayReturn()
        //{
        //    // Xử lý phản hồi từ VnPay
        //    var response = _vnPayService.PaymentExecute(Request.Query);

        //    if (response.Success)
        //    {

        //        return Ok(response);
        //    }
        //    else
        //    {
        //        return BadRequest("Transaction failed");
        //    }
        //}
        [HttpGet("vnpay_return")]
        public IActionResult VnPayReturn()
        {
            // Xử lý phản hồi từ VnPay
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (response.Success)
            {
                // Tạo URL để redirect đến trang frontend và thêm các tham số
                var redirectUrl = $"http://localhost:3000/paymentResponse?success=true&orderDescription={Uri.EscapeDataString(response.OrderDescription)}&transactionId={response.TransactionId}&orderId={response.OrderId}&paymentMethod={response.PaymentMethod}&paymentId={response.PaymentId}&vnPayResponseCode={response.VnPayResponseCode}";

                return Redirect(redirectUrl);
            }
            else
            {
                // Redirect đến trang thất bại nếu có lỗi
                var errorUrl = "http://localhost:3000/paymentResponse?success=false&vnPayResponseCode=" + response.VnPayResponseCode;
                return Redirect(errorUrl);
            }
        }


    }

}
