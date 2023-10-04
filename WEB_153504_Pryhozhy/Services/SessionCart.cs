using System.Text.Json.Serialization;
using WEB_153504_Pryhozhy.Domain.Entities;
using WEB_153504_Pryhozhy.Extensions;

namespace WEB_153504_Pryhozhy.Services
{
    public class SessionCart : Cart
    {
        public static Cart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()
            .HttpContext?.Session;
            SessionCart cart = session?.Get<SessionCart>("CartViewComponent") ?? new SessionCart();
            cart.Session = session;
            return cart;
        }

        [JsonIgnore]
        public ISession? Session { get; set; }

        public override void AddToCart(Pizza pizza)
        {
            base.AddToCart(pizza);
            Session?.Set("CartViewComponent", this);
        }

        public override void RemoveItems(int id)
        {
            base.RemoveItems(id);
            Session?.Set("CartViewComponent", this);
        }

        public override void ClearAll()
        {
            base.ClearAll();
            Session?.Remove("CartViewComponent");
        }
    }
}
