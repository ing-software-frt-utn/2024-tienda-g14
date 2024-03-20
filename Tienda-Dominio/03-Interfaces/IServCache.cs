namespace Tienda_Dominio._03_Interfaces
{
    public interface IServCache
    {
        void GuardarCache(string clave, object dato, string id_user = "");
        T RecuperarCache<T>(string clave, string id_user = "");
        void LimpiarCache(string id_user);

    }
}
