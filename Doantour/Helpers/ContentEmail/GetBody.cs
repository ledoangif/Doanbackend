using Doantour.DTO;

namespace Doantour.Helpers.ContentEmail
{
    public class GetBody
    {
        public static string GetEmailBodyForBookingStatus(string bookingStatus, MailDTO tourDetails)
        {

            string formattedDateStart = tourDetails.DateStart.ToString("dd/MM/yyyy");
            string formattedDateEnd = tourDetails.DateEnd.ToString("dd/MM/yyyy");
            string formattedPriceSale = tourDetails.PriceSale.ToString("#,##0") + " VND";
            string formattedPaymentTime = tourDetails.PaymentTime.ToString("dd/MM/yyyy HH:mm:ss");
            string formattedPaymentTimePlusOneDay = tourDetails.PaymentTime.AddDays(1).ToString("dd/MM/yyyy HH:mm:ss");
            string formattedPaymentTimePlusTwoHours = tourDetails.PaymentTime.AddHours(24).ToString("dd/MM/yyyy HH:mm:ss");
            decimal remainingAmount = tourDetails.PriceSale - 100000*(tourDetails.Adult + tourDetails.Child); // Trừ 100,000 VND từ trị giá booking
            string formattedRemainingAmount = remainingAmount.ToString("#,##0") + " VND";
            
            switch (bookingStatus)
            {
                case Constants.Success:
                    return $@"
                <html>
                <body style='font-family: Arial, sans-serif; color: #333;'>
                 <div>
                   <table cellpadding=""0"" cellspacing=""0"" width=""760"" border=""0"" style=""font-family: Arial, sans-serif;"">
                   <tr>
            <td colspan=""2"">
                <div style=""text-align:center;font-weight:bold;text-transform:uppercase;color:#000;font-size:24px;padding-top:20px;padding-bottom:20px;border-bottom:1px dotted #ccc;border-top:1px dotted #ccc;margin-bottom:30px"">Booking của quý khách</div>
            </td>
        </tr>
        <tr>
            <td colspan=""2"">
                <div style=""font-weight:bold;text-transform:uppercase;color:#c50000;margin-bottom:10px;font-size:16px"">I. Phiếu xác nhận booking:</div>
                <div style=""background:#f1f1f1;padding:15px;height:auto;margin-bottom:20px"">
                    <div style=""width:100%;float:left"">
                        <div style=""padding:0 15px 0 15px"">
                            
                            <div style=""float:left;width:100%;margin-bottom:7px"">
                                <div style=""float:left;width:20%;font-weight:bold;color:#333"">Tên tour:</div>
                                <div style=""float:left;width:80%"">{tourDetails.NameTour}</div>
                            </div>
                           
                            <div style=""float:left;width:100%;margin-bottom:7px"">
                                <div style=""float:left;width:20%;font-weight:bold;color:#333"">Ngày đi:</div>
                                <div style=""float:left;width:30%"">{formattedDateStart} {tourDetails.timeStart}</div>
                                <div style=""float:left;width:20%;font-weight:bold;color:#333"">Ngày về:</div>
                                <div style=""float:left;width:30%"">{formattedDateEnd} {tourDetails.timeEnd}</div>
                            </div>
                            <div style=""float:left;width:100%;margin-bottom:7px"">
                                <div style=""float:left;width:20%;font-weight:bold;color:#333"">Nơi tập trung:</div>
                                <div style=""float:left;width:30%"">{tourDetails.MeetingPoint}</div>
                                <div style=""float:left;width:20%;font-weight:bold;color:#333"">Điểm khởi hành:</div>
                                <div style=""float:left;width:30%"">{tourDetails.placeStart}</div>
                            </div>
                        </div>
                    </div>
                    <div style=""clear:both""></div>
                </div>
                <div style=""line-height:22px;text-align:justify;margin-bottom:25px"">
                    <strong>Ghi chú:</strong> Tour không hoàn phí đặt cọc, quý khách vui lòng đọc kĩ lưu ý khi hủy chuyến của chúng tôi.
                </div>
            </td>
        </tr>
        <tr>
            <td colspan=""2"" class=""m_4931299018104403581chitietbooking"">
                <div style=""font-weight:bold;text-transform:uppercase;color:#c50000;margin-bottom:10px;font-size:16px"">II. Chi tiết booking:</div>
                <table width=""100%"" style=""margin-bottom:20px"">
                    <tr>
                        <td class=""m_4931299018104403581td-left"" style=""padding:5px 0;"">Số booking:</td>
                        <td class=""m_4931299018104403581td-right"" style=""padding:5px 0;"">
                        <span style=""font-weight:bold"">{tourDetails.BookingID}</span>&nbsp;&nbsp;
                            <span style=""font-style:italic"">Quý khách vui lòng nhớ số booking (Booking No) để thuận tiện cho các giao dịch sau này.</span>
                        </td>
                    </tr>
                    <tr>
                        <td class=""m_4931299018104403581td-left"" style=""padding:5px 0;"">Trị giá booking:</td>
                        <td class=""m_4931299018104403581td-right"" style=""padding:5px 0;"">
                            <span style=""font-weight:bold;color:#c50000"">{formattedPriceSale}</span>
                        </td>
                    </tr>
 <tr>
                        <td class=""m_4931299018104403581td-left"" style=""padding:5px 0;"">Tình trạng:</td>
                        <td class=""m_4931299018104403581td-right"" style=""padding:5px 0;"">
                            <span style=""font-weight:bold;color:red"">Đặt tour thành công </span>
                        </td>
                    </tr>
                 
                </table>
            </td>
        </tr>
        <tr>
            <td colspan=""2"" class=""m_4931299018104403581thongtinlienlac"">
                <div style=""font-weight:bold;text-transform:uppercase;color:#c50000;margin-bottom:10px;font-size:16px"">III. Thông tin liên lạc:</div>
                <table width=""100%"" style=""margin-bottom:20px"">
                    <tr>
                        <td class=""m_4931299018104403581td-left"" style=""padding:5px 0;"">Họ và tên:</td>
                        <td class=""m_4931299018104403581td-right"" style=""padding:5px 0;"">
                          {tourDetails.CustomerName}
                        </td>
                    </tr>
                    <tr>
                        <td class=""m_4931299018104403581td-left"" style=""padding:5px 0;"">Địa chỉ:</td>
                        <td class=""m_4931299018104403581td-right"" style=""padding:5px 0;"">{tourDetails.Address}</td>
                    </tr>
                    <tr>
                        <td class=""m_4931299018104403581td-left"" style=""padding:5px 0;"">Điện thoại:</td>
                        <td class=""m_4931299018104403581td-right"" style=""padding:5px 0;"">
                           {tourDetails.CustomerPhone}
                        </td>
                    </tr>
                    <tr>
                        <td class=""m_4931299018104403581td-left"" style=""padding:5px 0;"">Email:</td>
                        <td class=""m_4931299018104403581td-right"" style=""padding:5px 0;"">
                            {tourDetails.Email}
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan=""2"" class=""m_4931299018104403581tour"">
                <div style=""font-weight:bold;text-transform:uppercase;color:#c50000;margin-bottom:10px;font-size:16px"">IV. Thông tin hành khách trong đơn đặt tour:</div>
                <table width=""100%"" style=""margin-bottom:20px"">
                    <tr>
                       {tourDetails.customerInTours}
                    </tr>
                    
                </table>
            </td>
        </tr>
        <tr>
            <td colspan=""2"" class=""m_4931299018104403581lienhe"">
                <div style=""font-weight:bold;text-transform:uppercase;color:#c50000;margin-bottom:10px;font-size:16px"">V. Liên hệ:</div>
                <table width=""100%"">
                    <tr>
                        <td style=""padding:5px 0;"">
                            Quý khách có thắc mắc cần hỗ trợ, vui lòng liên hệ bộ phận hỗ trợ khách hàng của Hachu Travel:<br>
                            Điện thoại: <strong>1900 1818</strong><br>
                            Email: <strong>support@hachutrveltour.com</strong>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>

                </body>
                </html>";
                case Constants.Cancel:
                    return $@"
                <html>
                <body style='font-family: Arial, sans-serif; color: #333;'>
                    
                  <table border=""0"" cellspacing=""0"" cellpadding=""0"" align=""left"" width=""100%"" style=""width:100.0%"">
    <tbody>
        <tr>
            <td style=""padding:0cm 0cm 0cm 0cm"">
                <p class=""MsoNormal"">
                    <span style=""color:black"">&nbsp;<u></u><u></u></span>
                </p>
            </td>
        </tr>
        <tr>
            <td style=""padding:0cm 0cm 0cm 0cm"">
                <p class=""MsoNormal"" style=""text-align:justify;text-indent:36.0pt"">
                    <a name=""m_435424650249380749_OLE_LINK55""></a>
                    <a name=""m_435424650249380749_OLE_LINK56""></a>
                    <a name=""m_435424650249380749_OLE_LINK57""></a>&nbsp;<u></u><u></u>
                </p>
                <p class=""MsoNormal"" style=""text-align:justify"">
                    Kính gửi Quý khách !<u></u><u></u>
                </p>
                <p class=""MsoNormal"" style=""text-align:justify"">&nbsp;<u></u><u></u></p>
                <p class=""MsoNormal"" style=""text-align:justify"">
                    Thay mặt <b>HachuTravel</b>, kính gửi đến&nbsp;Quý khách lời chúc sức khỏe và lời&nbsp;cảm ơn chân thành vì đã quan tâm, tin tưởng sử dụng dịch vụ của chúng tôi. <u></u><u></u>
                </p>
                <p class=""MsoNormal"" style=""text-align:justify"">&nbsp;<u></u><u></u></p>
                <p class=""MsoNormal"" style=""text-align:justify"">
                    <i>HachuTravel có liên hệ với số điện thoại đã đăng ký giữ chỗ nhưng đều bị từ chối cuộc gọi. Vì lý do booking cần xác nhận dịch vụ nhưng không nhận được phản hồi từ quý khách nên hệ thống đã tự động hủy booking.<u></u><u></u></i>
                </p>
                <p class=""MsoNormal"" style=""text-align:justify""><i><u></u>&nbsp;<u></u></i></p>
                <p class=""MsoNormal"" style=""text-align:justify"">
                    Nếu gia đình mình còn kế hoạch tham gia tour, vui lòng đặt tour lại trên website
                    <span style=""font-size:11.0pt;font-family:&quot;Calibri&quot;,sans-serif"">
                        <a href=""http://www.hachutravel.com.vn"" >
                            <span style=""font-size:12.0pt;font-family:&quot;Times New Roman&quot;,serif"">www.travel.com.vn</span>
                        </a>
                    </span> hoặc liên hệ Tổng đài 1900 1818 để đặt tour.<u></u><u></u>
                </p>
                <p class=""MsoNormal"" style=""text-align:justify"">&nbsp;<u></u><u></u></p>
                <p class=""MsoNormal"">
                    <b><span style=""font-family:&quot;Roboto&quot;,serif;color:#c50000;text-transform:uppercase"">Chi tiết booking:<u></u><u></u></span></b>
                </p>
                <table border=""1"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""width:100.0%;border-collapse:collapse;border:none"">
                    <tbody>
                        <tr>
                            <td width=""170"" style=""width:127.2pt;border-top:solid #f4f5f6 1.0pt;border-left:solid #f4f5f6 1.0pt;border-bottom:solid white 1.0pt;border-right:none;background:#f4f5f6;padding:3.75pt 7.5pt 3.75pt 7.5pt"">
                                <p class=""MsoNormal"" align=""right"" style=""text-align:right"">
                                    <span style=""font-family:&quot;Roboto&quot;,serif"">Số booking:<u></u><u></u></span>
                                </p>
                            </td>
                            <td width=""1142"" style=""width:856.75pt;border:solid #f4f5f6 1.0pt;border-left:none;padding:3.75pt 7.5pt 3.75pt 7.5pt"">
                                <p class=""MsoNormal"" style=""line-height:15.0pt"">
                                    <b><span style=""font-family:&quot;Roboto&quot;,serif"">{tourDetails.BookingID}</span></b><span style=""font-family:&quot;Roboto&quot;,serif"">&nbsp;&nbsp;<a href=""https://travel.com.vn/confirm-booking/240228504626"" target=""_blank"" data-saferedirecturl=""https://www.google.com/url?q=https://travel.com.vn/confirm-booking/240228504626&amp;source=gmail&amp;ust=1716732861117000&amp;usg=AOvVaw3QdgY0xvs2kZY_Jeq6EK5x""><i><span style=""color:#306eb7"">(Xem chi tiết)</span></i></a><br><i>Quý khách vui lòng nhớ số booking (Booking No) để thuận tiện cho các giao dịch sau này.</i> <u></u><u></u></span></p>
                            </td>
                        </tr>
                        <tr>
                            <td width=""170"" style=""width:127.2pt;border-top:none;border-left:solid #f4f5f6 1.0pt;border-bottom:solid white 1.0pt;border-right:none;background:#f4f5f6;padding:3.75pt 7.5pt 3.75pt 7.5pt"">
                                <p class=""MsoNormal"" align=""right"" style=""text-align:right"">
                                    <span style=""font-family:&quot;Roboto&quot;,serif"">Trị giá booking:<u></u><u></u></span>
                                </p>
</td>
                            <td width=""1142"" style=""width:856.75pt;border-top:none;border-left:none;border-bottom:solid #f4f5f6 1.0pt;border-right:solid #f4f5f6 1.0pt;padding:3.75pt 7.5pt 3.75pt 7.5pt"">
                                <p class=""MsoNormal"" style=""line-height:15.0pt"">
                                    <b><span style=""font-family:&quot;Roboto&quot;,serif;color:#c50000"">{formattedPriceSale}</span></b><span style=""font-family:&quot;Roboto&quot;,serif""> <u></u><u></u></span></p>
                            </td>
                        </tr>
                        <tr>
                            <td width=""170"" style=""width:127.2pt;border-top:none;border-left:solid #f4f5f6 1.0pt;border-bottom:solid white 1.0pt;border-right:none;background:#f4f5f6;padding:3.75pt 7.5pt 3.75pt 7.5pt"">
                                <p class=""MsoNormal"" align=""right"" style=""text-align:right"">
                                    <span style=""font-family:&quot;Roboto&quot;,serif"">Ngày đăng ký:<u></u><u></u></span>
                                </p>
                            </td>
                            <td width=""1142"" style=""width:856.75pt;border-top:none;border-left:none;border-bottom:solid #f4f5f6 1.0pt;border-right:solid #f4f5f6 1.0pt;padding:3.75pt 7.5pt 3.75pt 7.5pt"">
                                <p class=""MsoNormal"" style=""line-height:15.0pt"">
                                    <span style=""font-family:&quot;Roboto&quot;,serif"">{tourDetails.PaymentTime} (Theo giờ Việt Nam) <u></u><u></u></span>
                                </p>
                            </td>
                        </tr>
                        <tr>
                            <td width=""170"" style=""width:127.2pt;border-top:none;border-left:solid #f4f5f6 1.0pt;border-bottom:solid white 1.0pt;border-right:none;background:#f4f5f6;padding:3.75pt 7.5pt 3.75pt 7.5pt"">
                                <p class=""MsoNormal"" align=""right"" style=""text-align:right"">
                                    <span style=""font-family:&quot;Roboto&quot;,serif"">Tình trạng:<u></u><u></u></span>
                                </p>
                            </td>
                            <td width=""1142"" style=""width:856.75pt;border-top:none;border-left:none;border-bottom:solid #f4f5f6 1.0pt;border-right:solid #f4f5f6 1.0pt;padding:3.75pt 7.5pt 3.75pt 7.5pt"">
                                <p class=""MsoNormal"" style=""line-height:15.0pt"">
                                    <b><span style=""font-family:&quot;Roboto&quot;,serif;color:#c50000"">Booking đã hủy do không nhận được phản hồi <u></u><u></u></span></b></p>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <p class=""MsoNormal"" style=""text-align:justify"">&nbsp;<a name=""m_435424650249380749_OLE_LINK58""></a>&nbsp;<u></u><u></u></p>
                <p class=""MsoNormal"" style=""text-align:justify"">Cần thêm thông tin quý khách vui lòng liên hệ Tổng đài HachuTravel 1900 1818 để được hỗ trợ<u></u><u></u></p>
                <p class=""MsoNormal"" style=""text-align:justify"">&nbsp;<u></u><u></u></p>
                <p class=""MsoNormal"" style=""text-align:justify"">Trân trọng,<span style=""color:black""><u></u><u></u></span></p>
            </td>
        </tr>
        <tr>
            <td style=""padding:0cm 0cm 0cm 0cm"">
                <p class=""MsoNormal"" style=""text-align:justify""><span style=""color:black"">&nbsp;<u></u><u></u></span></p>
            </td>
        </tr>
        <tr>
            <td style=""padding:0cm 0cm 0cm 0cm"">
                <p class=""MsoNormal""><span style=""color:black"">&nbsp;<u></u><u></u></span></p>
            </td>
        </tr>
    </tbody>
</table>

                </body>
                </html>";

                case Constants.Deposited:
                    string paymentDeadline = tourDetails.timeStart == Constants.Cash ? formattedPaymentTimePlusOneDay : formattedPaymentTimePlusTwoHours;
                    return $@"
                 <html>
                <body style='font-family: Arial, sans-serif; color: #333;'>
                 <div>
                   <table cellpadding=""0"" cellspacing=""0"" width=""760"" border=""0"" style=""font-family: Arial, sans-serif;"">
                  
                   <tr>
            <td colspan=""2"">
                <div style=""text-align:center;font-weight:bold;text-transform:uppercase;color:#000;font-size:24px;padding-top:20px;padding-bottom:20px;border-bottom:1px dotted #ccc;border-top:1px dotted #ccc;margin-bottom:30px"">Booking của quý khách</div>
            </td>
        </tr>
        <tr>
            <td colspan=""2"">
                <div style=""font-weight:bold;text-transform:uppercase;color:#c50000;margin-bottom:10px;font-size:16px"">I. Phiếu xác nhận booking:</div>
                <div style=""background:#f1f1f1;padding:15px;height:auto;margin-bottom:20px"">
                    <div style=""width:100%;float:left"">
                        <div style=""padding:0 15px 0 15px"">
                            <div style=""float:left;width:100%;margin-bottom:7px"">
                                <div style=""float:left;width:20%;font-weight:bold;color:#333"">Tên tour:</div>
                                <div style=""float:left;width:80%"">{tourDetails.NameTour}</div>
                            </div>
                           
                            <div style=""float:left;width:100%;margin-bottom:7px"">
                                <div style=""float:left;width:20%;font-weight:bold;color:#333"">Ngày đi:</div>
                                <div style=""float:left;width:30%"">{formattedDateStart} {tourDetails.timeStart}</div>
                                <div style=""float:left;width:20%;font-weight:bold;color:#333"">Ngày về:</div>
                                <div style=""float:left;width:30%"">{formattedDateEnd} {tourDetails.timeEnd}</div>
                            </div>
                            <div style=""float:left;width:100%;margin-bottom:7px"">
                                <div style=""float:left;width:20%;font-weight:bold;color:#333"">Nơi tập trung:</div>
                                <div style=""float:left;width:30%"">{tourDetails.MeetingPoint}</div>
                                <div style=""float:left;width:20%;font-weight:bold;color:#333"">Điểm khởi hành:</div>
                                <div style=""float:left;width:30%"">{tourDetails.placeStart}</div>
                            </div>
                        </div>
                    </div>
                    <div style=""clear:both""></div>
                </div>
                <div style=""line-height:22px;text-align:justify;margin-bottom:25px"">
                    <strong>Ghi chú:</strong> Tour không hoàn phí đặt cọc, quý khách vui lòng đọc kĩ lưu ý khi hủy chuyến của chúng tôi.
                </div>
            </td>
        </tr>
        <tr>
            <td colspan=""2"" class=""m_4931299018104403581chitietbooking"">
                <div style=""font-weight:bold;text-transform:uppercase;color:#c50000;margin-bottom:10px;font-size:16px"">II. Chi tiết booking:</div>
                <table width=""100%"" style=""margin-bottom:20px"">
                    <tr>
                        <td class=""m_4931299018104403581td-left"" style=""padding:5px 0;"">Số booking:</td>
                        <td class=""m_4931299018104403581td-right"" style=""padding:5px 0;"">
                        <span style=""font-weight:bold"">{tourDetails.BookingID}</span>&nbsp;&nbsp;
                            <span style=""font-style:italic"">Quý khách vui lòng nhớ số booking (Booking No) để thuận tiện cho các giao dịch sau này.</span>
                        </td>
                    </tr>
                    <tr>
                        <td class=""m_4931299018104403581td-left"" style=""padding:5px 0;"">Trị giá booking:</td>
                        <td class=""m_4931299018104403581td-right"" style=""padding:5px 0;"">
                            <span style=""font-weight:bold;color:#c50000"">{formattedPriceSale}</span>
                        </td>
                    </tr>
                    <tr>
                        <td class=""m_4931299018104403581td-left"" style=""padding:5px 0;"">Ngày đăng ký:</td>
                        <td class=""m_4931299018104403581td-right"" style=""padding:5px 0;"">
                            <span>{formattedPaymentTime} (Theo giờ Việt Nam) </span>
                        </td>
                    </tr>
                    <tr>
                        <td class=""m_4931299018104403581td-left"" style=""padding:5px 0;"">Số tiền cần phải thanh toán:</td>
                        <td class=""m_4931299018104403581td-right"" style=""padding:5px 0;"">
                            <span style=""font-weight:bold;color:#c50000"">{formattedRemainingAmount}</span>
                        </td>
                    </tr>
                    <tr>
                        <td class=""m_4931299018104403581td-left"" style=""padding:5px 0;"">Thời hạn thanh toán:</td>
                        <td class=""m_4931299018104403581td-right"" style=""padding:5px 0;"">
                       {paymentDeadline} (Theo giờ Việt Nam)<br>
                            <span style=""font-style:italic"">Nếu quá thời hạn trên mà quý khách chưa thanh toán, HachuTravel sẽ hủy booking này.</span>
                        </td>
                    </tr>
                    <tr> 
                        <td class=""m_4931299018104403581td-left"" style=""padding: 5px 0;"">
                            Tình trạng:
                        </td>
                        <td class=""m_4931299018104403581td-right"" style=""padding: 5px 0; color: #c50000; font-weight: bold;"">
                            Đặt cọc thành công
                        </td>
                    </tr>
                        <tr>
                        <td></td>
                        <td class=""m_4931299018104403581td-right"" style=""padding: 5px 0; color: #000000;"">
                            <span style=""font-style: italic; font-weight: normal;"">
                                Quý khách vui lòng thanh toán số tiền còn lại để chuyển sang bước tiếp theo.
                            </span>
                        </td>
                     </tr>
                    <tr>
                        <td class=""m_4931299018104403581td-left"" style=""padding: 5px 0; white-space: nowrap;"">
                            Hướng dẫn thanh toán:
                        </td>
                        <td class=""m_4931299018104403581td-right"" style=""padding: 5px 0; color: #000000; font-weight: normal;"">
                            Quý khách vui lòng chuyển khoản qua số tài khoản: 
                            <span style=""font-weight: bold; color: #c50000;"">12345678999</span> Vietcombank Công ty TNHH Hachutravel, 
                            với nội dung: <span style=""font-style: italic;"">Thanhtoantour_bookingId</span> 
                            (<i>bookingId</i> là số booking của quý khách).
                        </td>
                    </tr>

                </table>
            </td>
        </tr>
        <tr>
            <td colspan=""2"" class=""m_4931299018104403581thongtinlienlac"">
                <div style=""font-weight:bold;text-transform:uppercase;color:#c50000;margin-bottom:10px;font-size:16px"">III. Thông tin liên lạc:</div>
                <table width=""100%"" style=""margin-bottom:20px"">
                    <tr>
                        <td class=""m_4931299018104403581td-left"" style=""padding:5px 0;"">Họ và tên:</td>
                        <td class=""m_4931299018104403581td-right"" style=""padding:5px 0;"">
                          {tourDetails.CustomerName}
                        </td>
                    </tr>
                    <tr>
                        <td class=""m_4931299018104403581td-left"" style=""padding:5px 0;"">Địa chỉ:</td>
                        <td class=""m_4931299018104403581td-right"" style=""padding:5px 0;"">{tourDetails.Address}</td>
                    </tr>
                    <tr>
                        <td class=""m_4931299018104403581td-left"" style=""padding:5px 0;"">Điện thoại:</td>
                        <td class=""m_4931299018104403581td-right"" style=""padding:5px 0;"">
                           {tourDetails.CustomerPhone}
                        </td>
                    </tr>
                    <tr>
                        <td class=""m_4931299018104403581td-left"" style=""padding:5px 0;"">Email:</td>
                        <td class=""m_4931299018104403581td-right"" style=""padding:5px 0;"">
                            {tourDetails.Email}
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan=""2"" class=""m_4931299018104403581tour"">
                <div style=""font-weight:bold;text-transform:uppercase;color:#c50000;margin-bottom:10px;font-size:16px"">IV. Thông tin hành khách trong đơn đặt tour:</div>
                <table width=""100%"" style=""margin-bottom:20px"">
                    <tr>
                       {tourDetails.customerInTours}
                    </tr>
                    
                </table>
            </td>
        </tr>
        <tr>
            <td colspan=""2"" class=""m_4931299018104403581lienhe"">
                <div style=""font-weight:bold;text-transform:uppercase;color:#c50000;margin-bottom:10px;font-size:16px"">V. Liên hệ:</div>
                <table width=""100%"">
                    <tr>
                        <td style=""padding:5px 0;"">
                            Quý khách có thắc mắc cần hỗ trợ, vui lòng liên hệ bộ phận hỗ trợ khách hàng của HaChu Travel:<br>
                            Điện thoại: <strong>1900 1818</strong><br>
                            Email: <strong>support@HaChuTravel.com</strong>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
                <h3 style=""color: #c50000; font-weight: bold;"">VI. Lưu ý hủy chuyến</h3>
                <p style=""margin-bottom: 10px;"">
                    Sau khi hoàn tất thanh toán, nếu Quý khách muốn hủy hoặc chuyển tour, vui lòng liên hệ trực tiếp qua điện thoại hoặc email để yêu cầu hủy tour. Hachutravel sẽ hỗ trợ Quý khách xử lý thủ tục hủy tour và áp dụng phí hủy theo quy định.
                </p>
                <ul style=""margin-bottom: 10px; padding-left: 20px; list-style-type: disc;"">
                    <li>Hủy tour sau khi đặt cọc: <b style=""color: #c50000;"">mất 100% tiền cọc</b></li>
                    <li>
                        Hủy tour khi đã thanh toán 100% tổng giá trị tour sẽ tính phí theo thời điểm thông báo hủy (<i>Không tính ngày lễ và chủ nhật</i>) như sau:
                        <ul style=""margin-top: 5px; padding-left: 20px; list-style-type: circle;"">
                            <li>Sau khi đặt tour, hủy tour: <b>mất 10% tổng giá trị tour</b></li>
                            <li>Từ 11 đến 15 ngày trước ngày khởi hành: <b>chịu phí 30% tổng giá trị tour</b></li>
                            <li>Từ 6 đến 10 ngày trước ngày khởi hành: <b>chịu phí 50% tổng giá trị tour</b></li>
                            <li>Từ 2 đến 5 ngày trước ngày khởi hành: <b>chịu phí 70% tổng giá trị tour</b></li>
                            <li>Từ 1 ngày trước ngày khởi hành: <b style=""color: #c50000;"">chịu phí 100% tổng giá trị tour</b></li>
                        </ul>
                    </li>
                </ul>
                </body>
                </html>";
                case Constants.Customercancel:

                    return $@"
    <html>
        <head>
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    color: #333;
                    background-color: #f9f9f9;
                    padding: 20px;
                    margin: 0;
                    width: 100%;
                }}
                table {{
                    width: 100%;
                    padding: 15px;
                    border-collapse: collapse;
                }}
                td {{
                    font-size: 14px;
                    line-height: 1.6;
                    padding: 10px;
                }}
                .header {{
                    font-size: 18px;
                    font-weight: bold;
                    color: #c50000;
                    text-align: center;
                    padding-bottom: 20px;
                }}
                .content {{
                    background-color: #fff;
                    border: 1px solid #ddd;
                    padding: 20px;
                    border-radius: 5px;
                }}
                .content p {{
                    font-size: 16px;
                    color: #333;
                }}
                .important {{
                    color: #e74c3c;
                    font-weight: bold;
                }}
                .contact-info {{
                    font-size: 16px;
                    padding-top: 20px;
                }}
                a {{
                    color: #c50000;
                    text-decoration: none;
                }}
            </style>
        </head>
        <body>
            <table>
                <tr>
                    <td class=""header"">
                        Thông Báo Hủy Tour
                    </td>
                </tr>
                <tr>
                    <td class=""content"">
                        <p style=""font-size: 16px; font-weight: bold;"">
                            Kính gửi Quý khách,
                        </p>
                        <p>
                            Chúng tôi rất tiếc khi nhận được yêu cầu hủy tour của Quý khách.
                            <br><br>
                            <span class=""important"">Hủy tour của Quý khách đã được thực hiện thành công.</span>
                            <br><br>
                            Chúng tôi rất tiếc về sự bất tiện này và hy vọng sẽ có cơ hội được phục vụ Quý khách trong những chuyến đi sau.
                            <br><br>
                            Nếu Quý khách có bất kỳ câu hỏi nào liên quan đến thủ tục hủy tour hoặc cần hỗ trợ thêm, vui lòng liên hệ với chúng tôi qua điện thoại hoặc email dưới đây.
                        </p>
                        <p style=""font-size: 16px; font-weight: bold;"">
                            Xin cảm ơn Quý khách đã lựa chọn dịch vụ của Hachutravel.
                        </p>
                    </td>
                </tr>
                <tr>
                    <td class=""contact-info"">
                        <b>Thông tin liên hệ:</b><br>
                        <b>Hachutravel</b><br>
                        <i>Email:</i> <a href=""mailto:support@hachutravel.com"">support@hachutravel.com</a><br>
                        <i>Hotline:</i> <a href=""tel:19001234"">1900-1234</a>
                    </td>
                </tr>
            </table>
        </body>
    </html>";

                default:
                    return $@"
                <html>
                <body style='font-family: Arial, sans-serif; color: #333;'>
                    <h2 style='color: #2E86C1;'>Your booking status has been updated</h2>
                    <p>Your booking status has been updated. Please check your booking details for more information.</p>
                </body>
                </html>";

            }
        }
    }
}
