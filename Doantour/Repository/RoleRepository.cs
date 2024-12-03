using Doantour.Models;
using Doantour.Repository;

namespace Doantour.Respository
{
    public class RoleRepository : BaseRepository<Role>
    {
        public RoleRepository(Hachutravelcontext context) : base(context)
        {
        }
    }
}
