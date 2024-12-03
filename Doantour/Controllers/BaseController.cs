using AutoMapper;
using Doantour.DTO;
using Doantour.Helpers.Page;
using Doantour.Models;
using Doantour.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Net;

namespace Doantour.Controllers
{
    public class BaseController<T, Y> where T : BaseEntity
        where Y : BaseDTO
    {
        private readonly IBaseRepository<T> _repository;
        protected readonly IMapper _mapper;

        public BaseController(IBaseRepository<T> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<List<T>> GenericSearch(Func<T?, bool> predicate)
        {
            var entities = await _repository.ToListAsync();
            var collection = await _repository.ToListAsync();
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            for (int i = 0; i < collection.Count; i++)
            {
                if (predicate(collection[i]))
                {
                    collection.Add(collection[i]);
                }
            }

            return collection;
        }

        public async Task<ActionResult<ResponseFormat>> GetAll()
        {
            var entities = await _repository.ToListAsync();

            var dtos = _mapper.Map<List<Y>>(entities);

            return new ResponseFormat(HttpStatusCode.OK, "Get List", dtos.ToList());
        }

        public async Task<ActionResult<ResponseFormat>> GetAllPagination(Pagination? pagination)
        {
            var entities = await _repository.ToListAsync();
            var result = PageResult<T>.ToPageResult(pagination, entities);
            pagination.TotalCount = entities.Count();
            //var dtos = _mapper.Map<List<Y>>(entities);
            //return new PageResult<T>(pagination, result);
            return new ResponseFormat(HttpStatusCode.OK, "Get List", new PageResult<T>(pagination, result));
        }


        public async Task<ActionResult<ResponseFormat>> FindAsync(int id)
        {
            var entity = await _repository.FindAsync(id);
            if (entity == null)
            {
                return new ResponseFormat(HttpStatusCode.NotFound, "Id Not Found", null);
            }

            var dto = _mapper.Map<Y>(entity);
            return new ResponseFormat(HttpStatusCode.OK, "Find By Id", dto);
        }


        public async Task<ResponseFormat> InsertAsync(T entity)
        {
            await _repository.InsertAsync(entity);
            var insertedDto = _mapper.Map<Y>(entity);
            return new ResponseFormat(HttpStatusCode.OK, "Insert Success", insertedDto);
        }

        public async Task<ResponseFormat> UpdateAsync(int id, Y dto)
        {
            var entity = await _repository.FindAsync(id);
            if (entity == null)
            {
                return new ResponseFormat(HttpStatusCode.NotFound, "Not Found!", null);
            }

            var updatedEntity = _mapper.Map(dto, entity);
            await _repository.UpdateAsync(updatedEntity);

            var updatedDto = _mapper.Map<Y>(updatedEntity);
            return new ResponseFormat(HttpStatusCode.Accepted, "Update Success!", updatedDto);
        }


        public async Task<ResponseFormat> DeleteAsync(int id)
        {
            var entity = await _repository.FindAsync(id);
            if (entity == null)
            {
                return new ResponseFormat(HttpStatusCode.NotFound, "Not Found!", null);
            }

            var result = await _repository.DeleteAsync(entity);

            return new ResponseFormat(HttpStatusCode.NoContent, "Delete Success!", result);
        }

        public async Task<ResponseFormat> DeleteManyAsync(List<T> entities)
        {
            await _repository.DeleteManyAsync(entities);
            return new ResponseFormat(HttpStatusCode.NoContent, "Delete Success!", null);
        }

        public async Task<bool> SelectAsync(Expression<Func<T, bool>> predicate)
        {
            return await _repository.SelectAsync(predicate);
        }

        public async Task<List<T>> SearchAsync(Expression<Func<T, bool>> predicate)
        {
            return await _repository.SearchAsync(predicate);
        }
    }
}