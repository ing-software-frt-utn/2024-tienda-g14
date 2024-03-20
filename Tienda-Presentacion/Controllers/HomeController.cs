using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Diagnostics;
using Tienda_Presentacion.Models;

namespace Tienda_Presentacion.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IDistributedCache _distributedCache;

        public HomeController(ILogger<HomeController> logger, SignInManager<IdentityUser> signInManager, IDistributedCache distributedCache)
        {
            _logger = logger;
            _signInManager = signInManager;
            _distributedCache = distributedCache;
        }

        public IActionResult Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                string nombre = _signInManager.UserManager.GetUserName(User);
                string userId = _signInManager.UserManager.GetUserId(User);
                var cacheKey = $"DatosUsuario_{userId}"; 
                var cacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
                };
                _distributedCache.SetString(cacheKey, JsonConvert.SerializeObject(nombre), cacheEntryOptions);
                return View();
            }
            else
            {
                return Redirect("Identity/Account/Login");
            }
            
        }

        public IActionResult Privacy()
        {
            string userId = _signInManager.UserManager.GetUserId(User);
            var cachedData = _distributedCache.GetString($"DatosUsuario_{userId}");
            if (cachedData != null)
            {
                var datosUsuarioRecuperados = JsonConvert.DeserializeObject<string>(cachedData);
                ViewBag.Nombre = datosUsuarioRecuperados;
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
