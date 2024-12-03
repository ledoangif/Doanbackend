using Doantour.DTO;
using Doantour.Helpers;
using Doantour.Models;
using Doantour.Repository;
using Microsoft.EntityFrameworkCore;

namespace Doantour.Respository
{
    public class BookingRepository : BaseRepository<Booking>
    {
        private readonly Hachutravelcontext _context;
        public BookingRepository(Hachutravelcontext context) : base(context)
        {
            _context = context;
        }
        public async Task<MailDTO> InfoToSendMail(int id)
        {
            var item = await (from a in _context.Tour
                              join b in _context.Booking on a.Id equals b.TourId
                              join c in _context.Customer on b.CustomerId equals c.Id
                              //join d in _context.Country on a.CountryId equals d.Id
                              
                              where b.Id == id
                              select new MailDTO
                              {
                                  NameTour = a.NameTour,
                                  DateEnd = (DateTime)a.DateEnd,
                                  DateStart = (DateTime)a.DateStart,
                                  Place = a.Place,
                                  timeStart = a.timeStart,
                                  timeEnd = a.timeEnd,
                                  placeStart = a.placeStart,
                                  TourId = a.Id,
                                  placeEnd = a.placeEnd,
                                  NumOfDay = a.NumOfDay,
                                  PriceSale = a.PriceSale,
                                  Email = c.Email,
                                  Address = c.Address,
                                  //CountryName = d.CountryName,
                                  CustomerName = c.NameCustomer,
                                  CustomerPhone = c.PhoneNumber,
                                  TotalBill = b.TotalBill,
                                  Paymented = b.Paymented,
                                  //PaymentBy = b.PaymentBy,
                                  PaymentTime = b.PaymentTime,
                                  MeetingPoint = a.MeetingPoint,
                                  Child = b.Child,
                                  Adult = b.Adult,
                                  customerInTours = b.customerInTours,
                                  BookingID = b.Id,

                                  Plan = a.Plan,
                              }).SingleOrDefaultAsync();

            return item;
        }
        public async Task<decimal[]> ToTalBillEachMonth(int year)
        {
            // Lấy dữ liệu cho năm nào với các trạng thái Success, CustomerCancel, Cancel
            var bookingsForYear = _context.Booking
                .Where(order =>
                    (order.StatusBill == Constants.Success ||
                     order.StatusBill == Constants.Customercancel ||
                     order.StatusBill == Constants.Cancel||
                     order.StatusBill == Constants.Deposited)
                    && !order.IsDeleted
                    && order.UpdateDate.Year == year);

            // Tính tổng hóa đơn cho mỗi tháng của năm
            var monthlyTotal = await bookingsForYear
                .GroupBy(order => order.UpdateDate.Month)
                .Select(group => new
                {
                    Month = group.Key,
                    TotalSuccess = group.Where(order => order.StatusBill == Constants.Success).Sum(order => order.Paymented),
                    TotalCustomerCancel = group.Where(order => order.StatusBill == Constants.Customercancel).Sum(order => order.Paymented),
                    TotalCancel = group.Where(order => order.StatusBill == Constants.Cancel).Sum(order => order.Paymented),
                    TotalDeposited = group.Where(order => order.StatusBill == Constants.Deposited).Sum(order => order.Paymented)
                })
                .ToListAsync();

            // Khởi tạo mảng tổng hóa đơn của mỗi tháng và gán giá trị mặc định là 0 cho tất cả các tháng
            decimal[] totalBills = new decimal[12];
            decimal totalOverall = 0; // Tổng giá trị của tất cả các trạng thái trong năm

            // Duyệt qua kết quả nhóm để lấy tổng hóa đơn của mỗi tháng và tổng chung
            foreach (var item in monthlyTotal)
            {
                // Tính tổng cho từng tháng (bao gồm tất cả các trạng thái)
                decimal monthlySum = item.TotalSuccess + item.TotalCustomerCancel + item.TotalCancel +item.TotalDeposited;

                // Gán tổng tháng vào mảng tổng hóa đơn
                totalBills[item.Month - 1] = monthlySum;

                // Cộng vào tổng toàn bộ
                totalOverall += monthlySum;
            }
            return totalBills;
        }

        public async Task<int> CountBookingCancelorNotCancel(string status)
        {
            var results = _context.Booking.Where(x => x.StatusBill == status && x.IsDeleted == false && x.UpdateDate.Year == DateTime.Now.Year).Count();
            return results;
        }


    }
}

