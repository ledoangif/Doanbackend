using Doantour.DTO;
using Doantour.Models;
using Doantour.Repository;

namespace Doantour.Respository
{
    public class CountryRepository : BaseRepository<Country>
    {
        private Hachutravelcontext _context;
        public CountryRepository(Hachutravelcontext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<CountryDTO>> Top4Country()
        {
            var query = from b in _context.Booking
                        join t in _context.Tour on b.TourId equals t.Id
                        join c in _context.Country on t.CountryId equals c.Id
                        group c by new { c.Id, c.CountryName, c.CountryImage } into countryGroup
                        orderby countryGroup.Count() descending
                        select new CountryDTO()
                        {
                            Id = countryGroup.Key.Id,
                            CountryName = countryGroup.Key.CountryName,
                            CountryImage = countryGroup.Key.CountryImage,
                        };

            var top4Countries = query.Take(4).ToList();
            return top4Countries;
        }
    }


}
