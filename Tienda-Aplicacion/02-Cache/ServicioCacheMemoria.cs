using Microsoft.Extensions.Caching.Memory;
using Tienda_Dominio._03_Interfaces;

namespace Tienda_Aplicacion._02_Cache
{
    public class ServicioCacheMemoria : IServCache
    {
        private readonly IMemoryCache _memoryCache;
        public ServicioCacheMemoria(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public void GuardarCache(string clave, object dato, string id_user = "")
        {
            String cacheKey = string.IsNullOrEmpty(id_user) ? $"{clave}" : $"{clave}_{id_user}";

            _memoryCache.Set(cacheKey, dato, TimeSpan.FromMinutes(5));
        }

        // RECUPERA DE CACHE EL DATO CON LA CLAVE INDICADA
        public T RecuperarCache<T>(string clave, string id_user = "")
        {
            String cacheKey = string.IsNullOrEmpty(id_user) ? $"{clave}" : $"{clave}_{id_user}";

            if (_memoryCache.TryGetValue(cacheKey, out T dato)) { return dato; } { return default(T); }
        }

        public void LimpiarCache(string id_user)
        {
            _memoryCache.Remove($"Venta_{id_user}");
            _memoryCache.Remove($"Articulo_{id_user}");
        }
    }
}
