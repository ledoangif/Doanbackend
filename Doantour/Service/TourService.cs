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
    public class TourService : ControllerBase, IBaseService<Tour, TourDTO>
    {
        private readonly TourRepository _TourRepository;
        private readonly IMapper _mapper;

        public TourService(IMapper mapper, IServiceProvider serviceProvider)
        {
            var _repositoryFactory = serviceProvider.GetService<RepositoryFactory>();
            _TourRepository = _repositoryFactory.TourRepository;

            _mapper = mapper;
        }

        public async Task<List<TourDTO>> ToListAsync()
        {
            var entities = await _TourRepository.ToListAsync();

            if (entities == null || entities.Count == 0)
            {
                Console.WriteLine("No tours found in the repository.");
                return new List<TourDTO>();
            }

            var tourListDTO = _mapper.Map<List<TourDTO>>(entities);

            var updatedTourListDTO = await UpdateIsFinishTour(tourListDTO);

            var listIsFinish = updatedTourListDTO.Where(t => t.IsFinish == Constants.NotFinishTour).ToList();

            return listIsFinish;
        }

        public async Task<List<TourDTO>> TourOndeDayBeforeStart()
        {
            // Fetch all tours from the repository
            var entities = await _TourRepository.ToListAsync();

            if (entities == null || entities.Count == 0)
            {
                Console.WriteLine("No tours found in the repository.");
                return new List<TourDTO>();
            }

            // Fetch tours that are not finished and not deleted
            var listisFinish = await SearchAsync(x => x.DateStart > DateTime.Now && x.IsDeleted == false);

            // Filter the tours based on the start date condition
            var today = DateTime.Today;
            var filteredEntities = listisFinish.Where(tour => tour.DateStart == today.AddDays(1)).ToList();
            foreach (var tour in filteredEntities)
            {
                listisFinish.Remove(tour);
            }

            // Map the filtered entities to DTOs
            var TourListDTO = _mapper.Map<List<TourDTO>>(listisFinish);

            return TourListDTO;
        }


        public async Task<List<TourDTO>> ToListAll()
        {
            var ListDTO = await _TourRepository.ToListAsync();
            var Tourlist = _mapper.Map<List<TourDTO>>(ListDTO);
            return Tourlist;
        }
        public async Task<List<TourDTO>> Top6Tour()
        {
            var entities = await TourOndeDayBeforeStart();

            var Tourlist = _mapper.Map<List<TourDTO>>(entities).Take(6).ToList();

            return Tourlist;
        }
        public async Task<List<TourDTO>> UpdateIsFinishTour(List<TourDTO> tours)
        {
            if (tours == null || tours.Count == 0)
            {
                return new List<TourDTO>();
            }

            for (int i = 0; i < tours.Count; i++)
            {
                if (!tours[i].DateEnd.HasValue)
                {
                    throw new ArgumentException("Tour must have a DateEnd.");
                }

                tours[i].IsFinish = (tours[i].DateEnd.Value < DateTime.Now)
                    ? Constants.IsFinishTour
                    : Constants.NotFinishTour;

                var existingTour = await _TourRepository.FindAsync(tours[i].Id);
                if (existingTour != null)
                {
                    _mapper.Map(tours[i], existingTour);
                    await _TourRepository.UpdateAsync(existingTour);
                }
            }
            return tours;
        }


        public async Task<List<TourDTO>> SearchAsync(Expression<Func<Tour, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new BadHttpRequestException("Predicate is null!");
            }

            var searchResult = await _TourRepository.SearchAsync(predicate);
            var resultDTO = _mapper.Map<List<TourDTO>>(searchResult);
            return resultDTO;
        }

        public async Task<TourDTO> FindAsync(int id)
        {
            var entity = await _TourRepository.FindAsync(id);
            if (entity == null)
            {
                return new TourDTO();
            }

            var TourDto = _mapper.Map<TourDTO>(entity);

            if (TourDto == null)
            {
                return new TourDTO();
            }

            return TourDto;
        }
        public async Task<PageResult<Tour>> GetAllPagination(Pagination? pagination)
        {
            var entities = await ToListAsync();
            var Tour = _mapper.Map<List<Tour>>(entities);
            var result = PageResult<Tour>.ToPageResult(pagination, Tour);
            pagination.TotalCount = entities.Count();
            return new PageResult<Tour>(pagination, result);
        }
        public async Task<PageResult<Tour>> GetAllPaginationBeforeStartOneDay(Pagination? pagination)
        {
            var entities = await TourOndeDayBeforeStart();
            var Tour = _mapper.Map<List<Tour>>(entities);
            var result = PageResult<Tour>.ToPageResult(pagination, Tour);
            pagination.TotalCount = entities.Count();
            return new PageResult<Tour>(pagination, result);
        }
        public async Task<TourDTO> InsertAsync(TourDTO dto)
        {
            // Check if all required fields are filled
            if (string.IsNullOrEmpty(dto.NameTour) ||
                !dto.DateStart.HasValue ||
                !dto.DateEnd.HasValue ||
                dto.Cost == 0 ||
                dto.Seat == 0 ||
                string.IsNullOrEmpty(dto.Place) ||
                string.IsNullOrEmpty(dto.MeetingPoint) 
                )
            {
                throw new BadHttpRequestException("Thông tin này không được để trống");
            }
            decimal priceSale = 0;
            int numOfDay = 0;
            int slot = 0;

            if (dto.Discount.HasValue && dto.DateEnd.HasValue && dto.DateStart.HasValue)
            {
                priceSale = (decimal)(dto.Cost - (dto.Cost * dto.Discount / 100));
                numOfDay = (int)(dto.DateEnd - dto.DateStart)?.TotalDays;
            }
            
            dto.UpdateDate = DateTime.Now;
            slot = dto.Seat;
            string isFinish =
              Constants.NotFinishTour;
            TourDTO tour = new TourDTO
            {
                Id = dto.Id,
                NameTour = dto.NameTour,
                DateEnd = dto.DateEnd,
                DateStart = dto.DateStart,
                IsLocal = dto.IsLocal,
                Image = dto.Image,
                Cost = dto.Cost,
                PriceBuy= dto.PriceBuy,
                Discount = dto.Discount,
                Seat = dto.Seat,
                slot = slot,
                PriceSale = priceSale,
                NumOfDay = numOfDay,
                Place = dto.Place,
                timeEnd = dto.timeEnd,
                timeStart = dto.timeStart,
                placeEnd = dto.placeEnd,
                placeStart = dto.placeStart,
                Plan = dto.Plan,
                MeetingPoint = dto.MeetingPoint,
                IsFinish = isFinish,
                ServiceNotInclude = dto.ServiceNotInclude,
                ServiceInclude = dto.ServiceInclude,
                Descripttion = dto.Descripttion,
                CountryId = dto.CountryId,
                AppUserId = dto.AppUserId,
                
                UpdateDate = dto.UpdateDate,

            };
            var objMap = _mapper.Map<Tour>(tour);
            await _TourRepository.InsertAsync(objMap);
            return tour;
        }
        public async Task<List<TourDTO>> SearchTourDifferent(string name, DateTime date)
        {
            var list = await _TourRepository.SearchAsync(x => x.NameTour == name && x.DateStart != date && x.IsFinish == Constants.NotFinishTour);
            var ListTour = _mapper.Map<List<TourDTO>>(list);
            return ListTour;
        }

        public async Task<TourDTO> UpdateAsync(int id, TourDTO obj)
        {
            var existingEntity = await _TourRepository.FindAsync(id);
            if (existingEntity == null)
            {
                throw new BadHttpRequestException("Entity not found.");
            }

            if (id != obj.Id)
            {
                throw new BadHttpRequestException("Mismatched Id.");
            }
            decimal priceSale = 0;
            int numOfDay = 0;
            int slot = 0;
            obj.UpdateDate = DateTime.Now;
           
            if (obj.Discount.HasValue && obj.DateEnd.HasValue && obj.DateStart.HasValue)
            {
                priceSale = (decimal)(obj.Cost - (obj.Cost * obj.Discount / 100));
                numOfDay = (int)(obj.DateEnd - obj.DateStart)?.TotalDays;
            }
            slot = obj.Seat;
            string isFinish =
              Constants.NotFinishTour;
            TourDTO tour = new TourDTO
            {
                Id = obj.Id,
                NameTour = obj.NameTour,
                DateEnd = obj.DateEnd,
                DateStart = obj.DateStart,
                IsLocal = obj.IsLocal,
                Image = obj.Image,
                PriceBuy = obj.PriceBuy,
                Cost = obj.Cost,
                Discount = obj.Discount,
                Seat = obj.Seat,
                slot = slot,
                PriceSale = priceSale,
                NumOfDay = numOfDay,
                Place = obj.Place,
                timeEnd = obj.timeEnd,
                timeStart = obj.timeStart,
                placeEnd = obj.placeEnd,
                placeStart = obj.placeStart,
                Plan = obj.Plan,
                IsFinish = isFinish,
                MeetingPoint = obj.MeetingPoint,
                ServiceNotInclude = obj.ServiceNotInclude,
                ServiceInclude = obj.ServiceInclude,
                Descripttion = obj.Descripttion,
                CountryId = obj.CountryId,
                AppUserId = obj.AppUserId,
                
                UpdateDate = obj.UpdateDate,
            };

            _mapper.Map(tour, existingEntity);
            await _TourRepository.UpdateAsync(existingEntity);
            return tour;
        }
        public async Task<string> UploadFile([FromForm] IFormFile ImageFile)
        {
            string url = await Upload.Upfile(ImageFile);
            return url;
        }

        public async Task<TourDTO> DeleteAsync(int id)
        {
            var existingEntity = await _TourRepository.FindAsync(id);
            if (existingEntity == null)
            {
                throw new BadHttpRequestException("Entity not found.");
            }
            existingEntity.IsDeleted = true;
            existingEntity.UpdateDate = DateTime.Now;
            var item = await _TourRepository.UpdateAsync(existingEntity);
            return _mapper.Map<TourDTO>(item);
        }
        public async Task<List<CustomerByTour>> GetCustomerByTour(int id)
        {
            var result = await _TourRepository.GetCustomerByTour(id);
            return result;
        }
        private string RemoveLeadingNumber(string name)
        {
            return System.Text.RegularExpressions.Regex.Replace(name, @"^\d+\.\s*", "");
        }


    }
}
