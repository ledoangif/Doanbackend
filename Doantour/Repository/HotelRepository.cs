using Doantour.Models;
using Doantour.Repository;

namespace Doantour.Respository
{
    public class HotelRepository : BaseRepository<Hotel>
    {
        public HotelRepository(Hachutravelcontext context) : base(context)
        {
        }
    }
}
