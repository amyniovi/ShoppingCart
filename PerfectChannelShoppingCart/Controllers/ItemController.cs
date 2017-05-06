using System.Linq;
using System.Web.Http;
using PerfectChannelShoppingCart.PChannel.Repositories;

namespace PerfectChannelShoppingCart.Controllers
{
    [RoutePrefix("api/item")]
    public class ItemController : ApiController
    {
        private readonly IItemRepo _itemRepo;

        public ItemController()
        {
            _itemRepo = new ItemRepo();
        }

        public ItemController(IItemRepo itemRepo)
        {
            _itemRepo = itemRepo;
        }

        public IHttpActionResult Get()
        {
            var items = _itemRepo.Get().ToList();
            items.ForEach(item=>item.Uri = Request.RequestUri.ToString() + "/" +item.Id);
            return Ok(items);
        }

        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            return  Ok(_itemRepo.GetbyId(id));
        }
    }
}
