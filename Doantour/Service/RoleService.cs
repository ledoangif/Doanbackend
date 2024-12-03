using AutoMapper;
using Doantour.DTO;
using Doantour.Helpers.Page;
using Doantour.Models;
using Doantour.Repository;
using Doantour.Respository;
using System.Linq.Expressions;

namespace Doantour.Service
{
    public class RoleService : IBaseService<Role, RoleDTO>
    {
        private readonly RoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IMapper mapper, IServiceProvider serviceProvider)
        {
            var _repositoryFactory = serviceProvider.GetService<RepositoryFactory>();
            _roleRepository = _repositoryFactory.RoleRepository;
            _mapper = mapper;
        }

        public async Task<List<RoleDTO>> ToListAsync()
        {
            var entities = await _roleRepository.ToListAsync();

            var roleListDTO = _mapper.Map<List<RoleDTO>>(entities).ToList();

            return roleListDTO;
        }

        public async Task<List<RoleDTO>> SearchAsync(Expression<Func<Role, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new BadHttpRequestException("Predicate is null!");
            }

            var searchResult = await _roleRepository.SearchAsync(predicate);
            var resultDTO = _mapper.Map<List<RoleDTO>>(searchResult);
            return resultDTO;
        }

        public async Task<RoleDTO> FindAsync(int id)
        {
            var entity = await _roleRepository.FindAsync(id);
            if (entity == null)
            {
                return new RoleDTO();
            }

            var roleDto = _mapper.Map<RoleDTO>(entity);

            if (roleDto == null)
            {
                return new RoleDTO();
            }

            return roleDto;
        }

        public async Task<RoleDTO> InsertAsync(RoleDTO obj)
        {
            var existingRoleName = await _roleRepository.SelectAsync(r => r.RoleName == obj.RoleName);
            if (existingRoleName)
            {
                return null;
            }

            var objMap = _mapper.Map<Role>(obj);
            await _roleRepository.InsertAsync(objMap);
            return obj;
        }

        public async Task<RoleDTO> UpdateAsync(int id, RoleDTO obj)
        {
            var existingEntity = await _roleRepository.SelectAsync(x => x.Id == id);
            if (!existingEntity)
            {
                throw new BadHttpRequestException("Entity not found.");
            }

            if (id != obj.Id)
            {
                throw new BadHttpRequestException("Mismatched Id.");
            }

            var updateEntity = _mapper.Map<Role>(obj);
            await _roleRepository.UpdateAsync(updateEntity);
            return obj;
        }

        public async Task<RoleDTO> DeleteAsync(int id)
        {
            var existingEntity = await _roleRepository.FindAsync(id);
            if (existingEntity == null)
            {
                throw new BadHttpRequestException("Entity not found.");
            }
            var item = _roleRepository.UpdateAsync(existingEntity);
            return _mapper.Map<RoleDTO>(item);
        }

        public Task<PageResult<Role>> GetAllPagination(Pagination? pagination)
        {
            throw new NotImplementedException();
        }
    }


}
