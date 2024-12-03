using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Doantour.Request
{
    public class DepositRequest
    {
        [Required]
        public decimal Amount { get; set; } // Số tiền đặt cọc

        [Required]
        public string OrderDescriptionription { get; set; } // Mô tả đơn hàng
    }
}
