using AutoMapper;
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
    public class CountryService : IBaseService<Country, CountryDTO>
    {
        private readonly CountryRepository _CountryRepository;
        private readonly TourRepository _TourRepository;

        private readonly IMapper _mapper;

        public CountryService(IMapper mapper, IServiceProvider serviceProvider)
        {
            var _repositoryFactory = serviceProvider.GetService<RepositoryFactory>();
            _CountryRepository = _repositoryFactory.CountryRepository;
            _TourRepository = _repositoryFactory.TourRepository;
            _mapper = mapper;
        }

        public async Task<List<CountryDTO>> ToListAsync()
        {
            var entities = await _CountryRepository.ToListAsync();

            var CountryListDTO = _mapper.Map<List<CountryDTO>>(entities).ToList();

            return CountryListDTO;
        }
        public async Task<List<CountryDTO>> Top4Country()
        {
            var entities = await _CountryRepository.ToListAsync();
            var CountryListDTO = _mapper.Map<List<CountryDTO>>(entities).Take(4).ToList();
            return CountryListDTO;
        }
        public async Task<List<CountryDTO>> Next6Countries()
        {
            var entities = await _CountryRepository.ToListAsync();

            var CountryListDTO = _mapper.Map<List<CountryDTO>>(entities).Skip(4).Take(6).ToList();

            return CountryListDTO;
        }


        public async Task<List<CountryDTO>> SearchAsync(Expression<Func<Country, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new BadHttpRequestException("Predicate is null!");
            }

            var searchResult = await _CountryRepository.SearchAsync(predicate);
            var resultDTO = _mapper.Map<List<CountryDTO>>(searchResult);
            return resultDTO;
        }
        public async Task<PageResult<Country>> GetAllPagination(Pagination? pagination)
        {
            var entities = await _CountryRepository.ToListAsync();
            var result = PageResult<Country>.ToPageResult(pagination, entities);
            pagination.TotalCount = entities.Count();
            return new PageResult<Country>(pagination, result);
        }
        public async Task<CountryDTO> FindAsync(int id)
        {
            var entity = await _CountryRepository.FindAsync(id);
            if (entity == null)
            {
                return new CountryDTO();
            }

            var CountryDto = _mapper.Map<CountryDTO>(entity);

            if (CountryDto == null)
            {
                return new CountryDTO();
            }

            return CountryDto;
        }
        public async Task<CountryDTO> InsertAsync(CountryDTO obj)
        {
            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrWhiteSpace(obj.CountryName) ||
                string.IsNullOrWhiteSpace(obj.CountryImage) ||
                string.IsNullOrWhiteSpace(obj.ContinentName))
            {
                throw new BadHttpRequestException("Thông tin này không được để trống");
            }

            // Kiểm tra trùng lặp
            var existingCountryName = await _CountryRepository.SelectAsync(r => r.CountryName == obj.CountryName && r.IsDeleted == false);
            if (existingCountryName)
            {
                return null;
            }

            // Gán giá trị mặc định
            
            obj.UpdateDate = DateTime.Now;

            // Map DTO thành entity và thêm vào cơ sở dữ liệu
            var objMap = _mapper.Map<Country>(obj);
            await _CountryRepository.InsertAsync(objMap);

            return obj;
        }

        public async Task<CountryDTO> UpdateAsync(int id, CountryDTO obj)
        {
            // Kiểm tra rỗng các trường bắt buộc
            if (string.IsNullOrWhiteSpace(obj.CountryName) ||
                string.IsNullOrWhiteSpace(obj.CountryImage) ||
                string.IsNullOrWhiteSpace(obj.ContinentName))
            {
                throw new BadHttpRequestException("Thông tin này không được để trống.");
            }
            var existingEntity = await _CountryRepository.FindAsync(id);
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
            await _CountryRepository.UpdateAsync(existingEntity);
            return obj;
        }
        public async Task<CountryDTO> DeleteAsync(int id)
        {
            // Kiểm tra xem entity có tồn tại hay không
            var existingEntity = await _CountryRepository.FindAsync(id);
            if (existingEntity == null)
            {
                throw new BadHttpRequestException("Id không tồn tại.");
            }
            existingEntity.IsDeleted = true;
            existingEntity.UpdateDate = DateTime.Now;
            var item = await _CountryRepository.UpdateAsync(existingEntity);
            return _mapper.Map<CountryDTO>(item);
        }
        public async Task<string> UploadFile([FromForm] IFormFile ImageFile)
        {
            //// Kiểm tra file có rỗng hay không
            //if (ImageFile == null || ImageFile.Length == 0)
            //{
            //    throw new BadHttpRequestException("Thông tin này không được để trống.");
            //}

            // Kiểm tra định dạng file (chỉ chấp nhận jpg và png)
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(ImageFile.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new BadHttpRequestException("Chỉ chấp nhận các định dạng ảnh JPG, PNG");
            }
            string url = await Upload.Upfile(ImageFile);
            return url;
        }

    }
}
