using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using WEB_153504_Klimkovich.Domain;
using WEB_153504_Klimkovich.Domain.Entities;
using WEB_153504_Klimkovich.Extensions;

namespace WEB_153504_Klimkovich.Services
{
    public class SessionCart : Cart
    {
        [JsonIgnore]
        public ISession? Session { get; set; }

        public static Cart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()
            .HttpContext?.Session;
            SessionCart cart = session?.Get<SessionCart>("Cart") ?? new SessionCart();
            cart.Session = session;
            return cart;
        }
        public override void AddToCart(Electronics product)
        {
            base.AddToCart(product);
            Session?.Set("Cart", this);
        }
        public override void RemoveItem(int id)
        {
            base.RemoveItem(id);
            Session?.Set("Cart", this);
        }
        public override void ClearAll()
        {
            base.ClearAll();
            Session?.Remove("Cart");
        }
    }

}
