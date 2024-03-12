using IdentityModel;
using Mango.Web.Models;
using Mango.Web.Models.Dto;
using Mango.Web.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly IProductService _productService;
        private readonly ICartService _cartService;
        public HomeController(IProductService productService, ILogger<HomeController> logger,ICartService  cartService)
		{
			_productService = productService;
            _logger = logger;
            _cartService = cartService;
        }


        public async Task<IActionResult> Index()
        {
			List<ProductDto> list = new();
			ResponseDto? response = await _productService.GetAllProductsAsync();

			if (response != null && response.IsSuccess)
			{
				list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
				return View(list);
			}
			else
			{
				TempData["error"] = response?.Message;
				return RedirectToAction("index", "home");
			}
		}

        [Authorize]
        public async Task<IActionResult> ProductDetails(int productId)
        {
            ProductDto product = new();
            ResponseDto? response = await _productService.GetProductByIdAsync(productId);

            if (response != null && response.IsSuccess)
            {
                product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(product);
            }
            else
            {
                TempData["error"] = response?.Message;
                return RedirectToAction("index", "home");
            }
        }

        [Authorize]
        [HttpPost]
        [ActionName("ProductDetails")]
        public async Task<IActionResult> ProductDetails(ProductDto productDto)
        {
           CartDto cartDto= new CartDto()
           {
               CartHeader=new CartHeaderDto {
               UserId=User.Claims.Where(u=>u.Type== JwtClaimTypes.Subject)?.FirstOrDefault()?.Value
               }
           };

            CartDetailsDto cartDetails = new CartDetailsDto()
            {
                Count = (int) productDto.Count,
                ProductId =(int) productDto.ProductId,
            };
            List<CartDetailsDto> cartDetailsDtos= new() { cartDetails};
            cartDto.CartDetails = cartDetailsDtos;

            ResponseDto? response =await _cartService.UpsertCartAsync(cartDto);

            if(response != null && response.IsSuccess) {
                TempData["success"] = "Item has been added to the Shopping";
                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(productDto);
           
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}