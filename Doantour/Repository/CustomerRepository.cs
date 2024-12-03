using Doantour.Models;
using Doantour.Repository;

namespace Doantour.Respository
{
    public class CustomerRepository : BaseRepository<Customer>
    {
        public CustomerRepository(Hachutravelcontext context) : base(context)
        {
        }
    }
}
