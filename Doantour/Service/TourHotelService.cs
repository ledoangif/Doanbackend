using AutoMapper;
using Doantour.DTO;
using Doantour.Helpers.Page;
using Doantour.Models;
using Doantour.Repository;
using Doantour.Respository;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Doantour.Service
{
    public class TourHotelService : ControllerBase, IBaseService<TourHotel, TourHotelDTO>
    {
        private readonly TourHotelRepository _TourHotelRepository;
        private readonly IMapper _mapper;

        public TourHotelService(IMapper mapper, IServiceProvider serviceProvider)
        {
            var _repositoryFactory = serviceProvider.GetService<RepositoryFactory>();
            _TourHotelRepository = _repositoryFactory.TourHotelRepository;
            _mapper = mapper;
        }

        public async Task<List<TourHotelDTO>> ToListAsync()
        {
            var entities = await _TourHotelRepository.ToListAsync();

            var TourHotelListDTO = _mapper.Map<List<TourHotelDTO>>(entities).ToList();

            return TourHotelListDTO;
        }

        public async Task<List<TourHotelDTO>> SearchAsync(Expression<Func<TourHotel, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new BadHttpRequestException("Predicate is null!");
            }

            var searchResult = await _TourHotelRepository.SearchAsync(predicate);
            var resultDTO = _mapper.Map<List<TourHotelDTO>>(searchResult);
            return resultDTO;
        }
        public async Task<PageResult<TourHotel>> GetAllPagination2(Pagination? pagination, int id)
        {
            var entities = await _TourHotelRepository.SearchAsync(x => x.TourId == id);
            var result = PageResult<TourHotel>.ToPageResult(pagination, entities);
            pagination.TotalCount = entities.Count();
            return new PageResult<TourHotel>(pagination, result);
        }
        public async Task<TourHotelDTO> FindAsync(int id)
        {
            var entity = await _TourHotelRepository.FindAsync(id);
            if (entity == null)
            {
                return new TourHotelDTO();
            }

            var TourHotelDto = _mapper.Map<TourHotelDTO>(entity);

            if (TourHotelDto == null)
            {
                return new TourHotelDTO();
            }

            return TourHotelDto;
        }

        public async Task<TourHotelDTO> InsertAsync(TourHotelDTO obj)
        {
            var item = await _TourHotelRepository.SelectAsync(x => x.HotelId == obj.HotelId && x.TourId == obj.TourId);
            if (item)
            {
                return null;
            }
            // Set create and update dates
            
            obj.UpdateDate = DateTime.Now;

            // Map the DTO to the TourHotel entity
            var tourHotelEntity = _mapper.Map<TourHotel>(obj);

            // Insert the new tourHotel entity into the repository
            var insertedTourHotelEntity = await _TourHotelRepository.InsertAsync(tourHotelEntity);

            // Map the inserted entity back to the DTO, including the new ID
            var resultDto = _mapper.Map<TourHotelDTO>(insertedTourHotelEntity);

            return resultDto;
        }


        public async Task<TourHotelDTO> UpdateAsync(int id, TourHotelDTO obj)
        {
            var existingEntity = await _TourHotelRepository.FindAsync(id);
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
            await _TourHotelRepository.UpdateAsync(existingEntity);
            return obj;
        }

        public async Task<TourHotelDTO> DeleteAsync(int id)
        {
            var existingEntity = await _TourHotelRepository.FindAsync(id);
            if (existingEntity == null)
            {
                throw new BadHttpRequestException("Entity not found.");
            }
            existingEntity.IsDeleted = true;
            existingEntity.UpdateDate = DateTime.Now;
            var item = await _TourHotelRepository.UpdateAsync(existingEntity);
            return _mapper.Map<TourHotelDTO>(item);
        }

        public async Task<PageResult<TourHotel>> GetAllPagination(Pagination? pagination)
        {
            var entities = await _TourHotelRepository.ToListAsync();
            var result = PageResult<TourHotel>.ToPageResult(pagination, entities);
            pagination.TotalCount = entities.Count();
            return new PageResult<TourHotel>(pagination, result);
        }
    }
}
