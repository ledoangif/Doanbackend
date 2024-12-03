using AutoMapper;
using Doantour.DTO;
using Doantour.Helpers.Page;
using Doantour.Models;
using Doantour.Repository;
using Doantour.Respository;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Doantour.Service
{
    public class CustomerService : ControllerBase, IBaseService<Customer, CustomerDTO>
    {

        private readonly CustomerRepository _CustomerRepository;
        private readonly BookingRepository bookingRepository;

        private readonly IMapper _mapper;

        public CustomerService(IMapper mapper, IServiceProvider serviceProvider)
        {
            var _repositoryFactory = serviceProvider.GetService<RepositoryFactory>();
            _CustomerRepository = _repositoryFactory.CustomerRepository;
            bookingRepository = _repositoryFactory.BookingRepository;
            _mapper = mapper;
        }

        public async Task<List<CustomerDTO>> ToListAsync()
        {
            var entities = await _CustomerRepository.ToListAsync();

            var CustomerListDTO = _mapper.Map<List<CustomerDTO>>(entities).ToList();

            return CustomerListDTO;
        }

        public async Task<List<CustomerDTO>> SearchAsync(Expression<Func<Customer, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new BadHttpRequestException("Predicate is null!");
            }

            var searchResult = await _CustomerRepository.SearchAsync(predicate);
            var resultDTO = _mapper.Map<List<CustomerDTO>>(searchResult);
            return resultDTO;
        }
        public async Task<PageResult<Customer>> GetAllPagination(Pagination? pagination)
        {
            var entities = await _CustomerRepository.ToListAsync();
            var result = PageResult<Customer>.ToPageResult(pagination, entities);
            pagination.TotalCount = entities.Count();
            return new PageResult<Customer>(pagination, result);
        }
        public async Task<CustomerDTO> FindAsync(int id)
        {
            var entity = await _CustomerRepository.FindAsync(id);
            if (entity == null)
            {
                return new CustomerDTO();
            }

            var CustomerDto = _mapper.Map<CustomerDTO>(entity);

            if (CustomerDto == null)
            {
                return new CustomerDTO();
            }

            return CustomerDto;
        }
        public async Task<CustomerDTO> InsertAsync(CustomerDTO obj)
        {
            // Kiểm tra rỗng
            if (string.IsNullOrWhiteSpace(obj.NameCustomer)
                ||string.IsNullOrWhiteSpace(obj.PhoneNumber)
                || string.IsNullOrWhiteSpace(obj.Address)
                || string.IsNullOrWhiteSpace(obj.Email))
            {
                throw new BadHttpRequestException("Thông tin này không được để trống");
            }
            if (!Regex.IsMatch(obj.NameCustomer ?? "", @"^[A-Za-zÀ-ỹ\s]+$"))
            {
                throw new BadHttpRequestException("Họ tên chỉ được chứa ký tự chữ cái.");
            }

            // Số điện thoại phải đúng định dạng
            if (!Regex.IsMatch(obj.PhoneNumber ?? "", @"^((03|05|07|08|09|01[2|6|8|9])\d{8}|\+84\d{9})$"))
            {
                throw new BadHttpRequestException("Số điện thoại không đúng.");
            }

            // Kiểm tra nếu email bị thiếu hoặc không hợp lệ
            if (!Regex.IsMatch(obj.Email ?? "", @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new BadHttpRequestException("Vui lòng nhập một email hợp lệ.");
            }

            var existingCustomer = await _CustomerRepository.SelectAsync(r => r.Email == obj.Email || r.PhoneNumber == obj.PhoneNumber);
            if (existingCustomer)
            {
                return null;
            }

            
            obj.UpdateDate = DateTime.Now;

            var customerEntity = _mapper.Map<Customer>(obj);

            var insertedCustomerEntity = await _CustomerRepository.InsertAsync(customerEntity);

            return obj;
        }
        public async Task<CustomerDTO> UpdateAsync(int id, CustomerDTO obj)
        {
            // Kiểm tra rỗng
            if (string.IsNullOrWhiteSpace(obj.NameCustomer)
                || string.IsNullOrWhiteSpace(obj.PhoneNumber)
                || string.IsNullOrWhiteSpace(obj.Address)
                || string.IsNullOrWhiteSpace(obj.Email))
            {
                throw new BadHttpRequestException("Thông tin này không được để trống");
            }
            if (!Regex.IsMatch(obj.NameCustomer ?? "", @"^[A-Za-zÀ-ỹ\s]+$"))
            {
                throw new BadHttpRequestException("Họ tên chỉ được chứa ký tự chữ cái.");
            }

            // Số điện thoại phải đúng định dạng
            if (!Regex.IsMatch(obj.PhoneNumber ?? "", @"^((03|05|07|08|09|01[2|6|8|9])\d{8}|\+84\d{9})$"))
            {
                throw new BadHttpRequestException("Số điện thoại không đúng.");
            }

            // Kiểm tra nếu email bị thiếu hoặc không hợp lệ
            if (!Regex.IsMatch(obj.Email ?? "", @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new BadHttpRequestException("Vui lòng nhập một email hợp lệ.");
            }

            if (_CustomerRepository == null) throw new InvalidOperationException("Customer repository is not initialized.");
            if (_mapper == null) throw new InvalidOperationException("Mapper is not initialized.");

            var existingEntity = await _CustomerRepository.FindAsync(id);
            if (existingEntity == null)
            {
                throw new BadHttpRequestException("Entity not found.");
            }

            if (id != obj.Id)
            {
                throw new BadHttpRequestException("Mismatched Id.");
            }

            obj.UpdateDate = DateTime.Now;
            
            _mapper.Map(obj, existingEntity);
            await _CustomerRepository.UpdateAsync(existingEntity);
            return obj;
        }
        public async Task<CustomerDTO> DeleteAsync(int id)
        {
            var existingEntity = await _CustomerRepository.FindAsync(id);
            if (existingEntity == null)
            {
                throw new BadHttpRequestException("Entity not found.");
            }
            var listBookingByCustomer = await bookingRepository.SearchAsync(x => x.CustomerId == existingEntity.Id);
            foreach (var booking in listBookingByCustomer)
            {
                booking.IsDeleted = true;
                booking.UpdateDate = DateTime.Now;
                await bookingRepository.UpdateAsync(booking);

            }
            existingEntity.IsDeleted = true;
            existingEntity.UpdateDate = DateTime.Now;
            var item = await _CustomerRepository.UpdateAsync(existingEntity);
            return _mapper.Map<CustomerDTO>(item);
        }
    }

}
