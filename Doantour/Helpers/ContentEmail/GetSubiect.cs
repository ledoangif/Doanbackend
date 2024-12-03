using System.Reflection.Metadata;

namespace Doantour.Helpers.ContentEmail
{
    public class GetSubject
    {
        public static string GetEmailSubjectForBookingStatus(string bookingStatus)
        {
            return bookingStatus switch
            {
                Constants.Success => "Thông tin đặt tour trên HachuTravel.",
                Constants.Cancel => "Booking bị hủy do không nhận được phản hồi!",
                Constants.Pending => "Tour đang xử lý!",
                Constants.Customercancel => "Booking hủy theo yêu cầu của quý khách",
                Constants.Deposited=> "Đặt cọc thành công"
            };
        }
    }
}
