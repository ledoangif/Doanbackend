namespace Doantour.Models
{
    public class ReturnPaymentModel
    {
        public string TxnRef { get; set; } // Mã tham chiếu giao dịch
        public string HashSecret { get; set; } // Khóa bí mật
    }
}
