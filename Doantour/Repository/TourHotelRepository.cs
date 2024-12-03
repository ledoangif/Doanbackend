using Doantour.Models;
using Doantour.Repository;

namespace Doantour.Respository
{
    public class TourHotelRepository : BaseRepository<TourHotel>
    {
        public TourHotelRepository(Hachutravelcontext context) : base(context)
        {
        }
    }
}
