using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_153504_Klimkovich.Domain;
using WEB_153504_Klimkovich.Services.ProductService;

namespace WEB_153504_Klimkovich.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly Cart _cart;
        public CartController(IProductService productService, Cart cart)
        {
            _productService = productService;
            _cart = cart;
        }


        [Authorize]
        [Route("[controller]/add/{id:int}")]
        public async Task<ActionResult> Add(int id, string returnUrl)
        {
            var data = await _productService.GetProductByIdAsync(id);
            if (data.Success)
            {
                _cart.AddToCart(data.Data);
            }
            return Redirect(returnUrl);
        }
    }
}
