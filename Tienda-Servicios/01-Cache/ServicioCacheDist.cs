using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using Tienda_Dominio._03_Interfaces;

namespace Tienda_Servicios._01_Cache
{
    public class ServicioCacheDist : IServCache
    {
        private IDistributedCache _distributedCache;
        public ServicioCacheDist(IDistributedCache distributedCache) 
        { 
            _distributedCache = distributedCache;
        }
        public void GuardarCache(string clave, object dato, string id_user = "")
        {
            String cacheKey = id_user.IsNullOrEmpty() ? $"{clave}" : $"{clave}_{id_user}";

            var cacheEntryOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            var config = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };

            _distributedCache.SetString(cacheKey, JsonSerializer.Serialize(dato, config), cacheEntryOptions);
        }

        // RECUPERA DE CACHE EL DATO CON LA CLAVE INDICADA
        public T RecuperarCache<T>(string clave, string id_user = "")
        {
            String cacheKey = id_user.IsNullOrEmpty() ? $"{clave}" : $"{clave}_{id_user}";

            String datoCache = _distributedCache.GetString(cacheKey);

            if (!_distributedCache.GetString(cacheKey).IsNullOrEmpty()) { return JsonSerializer.Deserialize<T>(datoCache); } { return default(T); ; }
        }

        public void LimpiarCache(string id_user)
        {
            _distributedCache.Remove($"Venta_{id_user}");
            _distributedCache.Remove($"Articulo_{id_user}");
        }
    }
}
