﻿using AutoMapper;
using Doantour.DTO;
using Doantour.Helpers;
using Doantour.Helpers.Page;
using Doantour.Models;
using Doantour.Repository;
using Doantour.Respository;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Doantour.Service
{
    public class BookingService : ControllerBase, IBaseService<Booking, BookingDTO>
    {

        private readonly BookingRepository _BookingRepository;
        private readonly TourRepository _TourRepository;
        private readonly CustomerRepository _customerRepository;

        

        private readonly IMapper _mapper;

        public BookingService(IMapper mapper, IServiceProvider serviceProvider)
        {
            var _repositoryFactory = serviceProvider.GetService<RepositoryFactory>();
            _BookingRepository = _repositoryFactory.BookingRepository;
            _TourRepository = _repositoryFactory.TourRepository;
            _customerRepository = _repositoryFactory.CustomerRepository;
            _mapper = mapper;
        }

        public async Task<List<BookingDTO>> ToListAsync()
        {
            var entities = await _BookingRepository.ToListAsync();

            var BookingListDTO = _mapper.Map<List<BookingDTO>>(entities).ToList();

            return BookingListDTO;
        }

        public async Task<List<BookingDTO>> SearchAsync(Expression<Func<Booking, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new BadHttpRequestException("Predicate is null!");
            }

            var searchResult = await _BookingRepository.SearchAsync(predicate);
            var resultDTO = _mapper.Map<List<BookingDTO>>(searchResult);
            return resultDTO;
        }
        public async Task<PageResult<Booking>> GetAllPagination(Pagination? pagination)
        {
            var entities = await _BookingRepository.ToListAsync();

            var threeMonthsAgo = DateTime.Now.AddMonths(-3);
            var filteredEntities = entities
                .Where(x => x.UpdateDate >= threeMonthsAgo && x.UpdateDate.Year == DateTime.Now.Year && x.IsDeleted == false)
                .OrderByDescending(x => x.StatusBill == Constants.Pending)
                .ToList();

            var result = PageResult<Booking>.ToPageResult(pagination, filteredEntities);
            pagination.TotalCount = filteredEntities.Count();
            return new PageResult<Booking>(pagination, result);
        }

        public async Task<BookingDTO> FindAsync(int id)
        {
            var entity = await _BookingRepository.FindAsync(id);
            if (entity == null)
            {
                return new BookingDTO();
            }

            var BookingDto = _mapper.Map<BookingDTO>(entity);

            if (BookingDto == null)
            {
                return new BookingDTO();
            }

            return BookingDto;
        }

        public async Task<BookingDTO> InsertAsync(BookingDTO booking)
        {

            int totalTicketsToDecrease = booking.Adult + booking.Child;
            var tour = await _TourRepository.SearchAsync(x => x.Id == booking.TourId);
            //Check số lượng vé còn lại : Return :Vé hiện không đủ để đặt
            if (tour[0].slot < totalTicketsToDecrease)
            {
                return null;
            }
            //tour[0].slot -= totalTicketsToDecrease;
            await _TourRepository.UpdateAsync(tour[0]);

            booking.UpdateDate = DateTime.Now;
            var objMap = _mapper.Map<Booking>(booking);
            await _BookingRepository.InsertAsync(objMap);
            return booking;
        }

        public async Task<BookingDTO> UpdateAsync(int id, BookingDTO obj)
        {
            var existingEntity = await _BookingRepository.FindAsync(id);
            if (existingEntity == null)
            {
                throw new BadHttpRequestException("Entity not found.");
            }

            if (id != obj.Id)
            {
                throw new BadHttpRequestException("Mismatched Id.");
            }

            obj.UpdateDate = DateTime.Now;
            var tour = await _TourRepository.FindAsync(existingEntity.TourId.Value);
            if (tour == null)
            {
                throw new BadHttpRequestException("Tour not found.");
            }

            int newTotalTickets = obj.Adult + obj.Child;

            // Xử lý các trường hợp thay đổi trạng thái
            switch (existingEntity.StatusBill)
            {
                case "Chờ xử lý":
                    // Nếu trạng thái mới là Chờ xử lý
                    if (obj.StatusBill == "Chờ xử lý")
                    {
                        if (existingEntity.Adult + existingEntity.Child == newTotalTickets) return obj; // Không thay đổi
                    }
                    // Nếu trạng thái mới là Đã thanh toán hoặc Đã đặt cọc
                    else if (obj.StatusBill == "Đã thanh toán" || obj.StatusBill == "Đã đặt cọc")
                    {
                        tour.slot -= newTotalTickets;
                    }
                    // Nếu trạng thái mới là Hủy hoặc Khách hàng hủy
                    else if (obj.StatusBill == "Hủy" || obj.StatusBill == "Khách hàng hủy")
                    {
                        return obj; // Không thay đổi slot
                    }
                    break;

                case "Đã thanh toán":
                    // Nếu trạng thái mới là Chờ xử lý, Hủy hoặc Khách hàng hủy
                    if (obj.StatusBill == "Chờ xử lý" || obj.StatusBill == "Hủy" || obj.StatusBill == "Khách hàng hủy")
                    {
                        tour.slot += newTotalTickets;
                    }
                    break;
                case "Đã đặt cọc":
                    // Nếu trạng thái mới là Chờ xử lý, Hủy hoặc Khách hàng hủy
                    if (obj.StatusBill == "Chờ xử lý" || obj.StatusBill == "Hủy" || obj.StatusBill == "Khách hàng hủy")
                    {
                        tour.slot += newTotalTickets;
                    }
                    break;

                case "Hủy":
                    if (obj.StatusBill == "Đã thanh toán" || obj.StatusBill == "Đã đặt cọc")
                    {
                        tour.slot -= newTotalTickets;
                    }
                    break;
                case "Khách hàng hủy":
                    if (obj.StatusBill == "Đã thanh toán" || obj.StatusBill == "Đã đặt cọc")
                    {
                        tour.slot -= newTotalTickets;
                    }
                    break;
            }

            // Đảm bảo slot không âm
            if (tour.slot < 0)
            {
                throw new BadHttpRequestException("Not enough slots available.");
            }


            // Cập nhật lại số lượng vé trong tour
            await _TourRepository.UpdateAsync(tour);
            _mapper.Map(obj, existingEntity);
            await _BookingRepository.UpdateAsync(existingEntity);
            return obj;
        }
        //public async Task<BookingDTO> UpdateStatusToUnpaidAsync(int id)
        //{
        //    // Tìm booking hiện có qua ID
        //    var existingEntity = await _BookingRepository.FindAsync(id);
        //    if (existingEntity == null)
        //    {
        //        throw new BadHttpRequestException("Entity not found.");
        //    }
        //    existingEntity.StatusBill = "Đã đặt cọc";
        //    existingEntity.UpdateDate = DateTime.Now;

        //    // Cập nhật lại trong cơ sở dữ liệu
        //    await _BookingRepository.UpdateAsync(existingEntity);


        //    // Trả về đối tượng DTO đã cập nhật
        //    return _mapper.Map<BookingDTO>(existingEntity);
        //}
        public async Task<(string email, string statusBill)> UpdateStatusToUnpaidAsync(int bookingId)
        {
            var booking = await _BookingRepository.FindAsync(bookingId);
            if (booking == null)
            {
                return (null, null); // Trả về null nếu không tìm thấy booking
            }
            var tour = await _TourRepository.FindAsync(booking.TourId.Value);
            int totalTicketsToDecrease = booking.Adult + booking.Child;

            if (tour.slot < totalTicketsToDecrease)
            {
                throw new Exception("Số lượng vé không đủ để cập nhật trạng thái thành Đã đặt cọc.");
            }

            booking.StatusBill = "Chờ xử lý"; // Cập nhật trạng thái
            booking.Paymented = 100000* totalTicketsToDecrease;
            await _BookingRepository.UpdateAsync(booking);
            //tour.slot -= totalTicketsToDecrease;
            await _TourRepository.UpdateAsync(tour);

            // Lấy thông tin khách hàng từ repository
            var customer = await _customerRepository.FindAsync(booking.CustomerId.Value);
            //var tour = await _TourRepository.FindAsync(booking.TourId.Value);
            // Trả về email và statusBill
            return (customer?.Email, booking.StatusBill);
        }

        public async Task<(string CustomerName, string TourName,int child,int adult, decimal DepositAmount)> GetBookingDetailsAsync(int bookingId)
        {
            var booking = await _BookingRepository.FindAsync(bookingId);
            if (booking == null)
            {
                return (null, null,0,0,0); // Trả về null nếu không tìm thấy booking
            }

            var customer = await _customerRepository.FindAsync(booking.CustomerId.Value);
            var tour = await _TourRepository.FindAsync(booking.TourId.Value);
            var child = booking.Child;
            var adult = booking.Adult;
            decimal depositAmount = (100000 * child) + (100000 * adult);
            return (customer?.NameCustomer, tour?.NameTour,child,adult,depositAmount); // Trả về tên khách hàng và tên tour
        }


        public async Task<BookingDTO> DeleteAsync(int id)
        {
            var existingEntity = await _BookingRepository.FindAsync(id);
            if (existingEntity == null)
            {
                throw new BadHttpRequestException("Entity not found.");
            }
            existingEntity.IsDeleted = true;
            existingEntity.UpdateDate = DateTime.Now;


            var item = await _BookingRepository.UpdateAsync(existingEntity);
            return _mapper.Map<BookingDTO>(item);
        }
        public async Task<int> CountPending()
        {
            var list = await _BookingRepository.SearchAsync(x => x.IsDeleted == false && x.StatusBill == Constants.Pending);
            var count = list.Count();
            return count;
        }
        public async Task<decimal[]> ToTalBillEachMonth(int year)
        {
            return await _BookingRepository.ToTalBillEachMonth(year);
        }
        public async Task<int> CountBookingCancelorNotCancel(string status)
        {
            var results = await _BookingRepository.CountBookingCancelorNotCancel(status);
            return results;
        }
        //public async Task<List<BookingDTO>> getTourPaying()
        //{
        //    var yesterday = DateTime.Today.AddDays(-1);

        //    var result = await _BookingRepository.SearchAsync(
        //        x => x.StatusBill == Constants.UnPaid
        //             && x.IsDeleted == false

        //    );
        //    var item = _mapper.Map<List<BookingDTO>>(result);
        //    return item;
        //}
        public async Task<TourDTO> resetSlotAfterCancel(int child, int adult, int tourId)
        {

            int totalticket = child + adult;
            var item = await _TourRepository.FindAsync(tourId);
            item.slot += totalticket;
            await _TourRepository.UpdateAsync(item);
            var result = _mapper.Map<TourDTO>(item);
            return result;

            return null;
        }



    }

}
