using Microsoft.EntityFrameworkCore;
using Tienda_Datos._01_DB;
using Tienda_Dominio._01_Clases;
using Tienda_Dominio._03_Interfaces;

namespace Tienda_Datos
{
    public class Tienda_DB_Repositorio : IServDatos
    {
        private TiendaContexto _contexto;
        public Tienda_DB_Repositorio(TiendaContexto contexto)
        {
            _contexto = contexto;
        }
        public void actualizarInventario(long id_inventario, long cantidad)
        {
            try
            {
                Inventario inv = _contexto.Inventarios.Single(inv => inv.Id == id_inventario);
                inv.Cantidad -= cantidad;
                _contexto.SaveChanges();
            }
            catch
            {
                throw new Exception("Error al actualizar el inventario.");
            }
        }
        public void guardarAutorizacion(Autorizacion_Afip autorizacion)
        {
            try
            {
                _contexto.Autorizaciones_Afip.Add(autorizacion);
                _contexto.SaveChanges();
            }
            catch
            {
                throw new Exception("Error al guardar la autorización de Afip.");
            }
        }
        public void guardarLineaVenta(Linea_Venta lineaventa)
        {
            try
            {
                _contexto.Entry(lineaventa).State = EntityState.Added;
                _contexto.SaveChanges();
            }
            catch
            {
                throw new Exception("Error al guardar la linea de venta.");
            }
        }
        public void guardarPago(Pago pago)
        {
            try
            {
                _contexto.Pagos.Add(pago);
                _contexto.SaveChanges();
            }
            catch
            {
                throw new Exception("Error al guardar el pago.");
            }
        }
        public long guardarVenta(Venta venta)
        {
            try
            {
                _contexto.Entry(venta).State = EntityState.Added;
                _contexto.SaveChanges();
            return venta.Id;
            }
            catch
            {
                throw new Exception("Error al guardar la venta.");
            }
        }
        public Venta obtenerVenta(long id_venta)
        {
            try
            {
                return _contexto.Ventas.Single(vta => vta.Id == id_venta);
            }
            catch
            {
                throw new Exception("Error al obtener la venta.");
            }
        }
        public void guardarCambios()
        {
            try
            {
                _contexto.SaveChanges();
            }
            catch
            {
                throw new Exception("Error al guardar los cambios.");
            }
        }
        public Articulo obtenerArticulo(long codigo_articulo)
        {
            try
            {
                return _contexto.Articulos
                .AsNoTracking()
                .Include(art => art.Inventarios).ThenInclude(inv => inv.Color)
                .Include(art => art.Inventarios).ThenInclude(inv => inv.Talle)
                .Include(art => art.Inventarios).ThenInclude(inv => inv.Sucursal)
                .Include(art => art.Tipo_Talle)
                .Include(art => art.Marca)
                .Include(art => art.Categoria)
                .Single(art => art.Codigo == codigo_articulo);
            }
            catch
            {
                throw new Exception("Error al obtener el artículo.");
            }
        }
        public Cliente obtenerClientexDoc(long doc_cliente)
        {
            try
            {
                return _contexto.Clientes
                    .AsNoTracking()
                    .Single(cli => cli.NumeroDocumento == doc_cliente);
            }
            catch
            {
                throw new Exception("Error al obtener el cliente.");
            }
        }
        public Comprobante obtenerComprobante(long id_cond_emite, long id_cond_recibe)
        {
            try
            {
                long id_comprobante = _contexto.ComprobanteCondicion
                    .AsNoTracking()
                    .Single(x => (x.Emite == id_cond_emite && x.Recibe == id_cond_recibe)).Comprobante;
                return _contexto.Comprobantes
                    .AsNoTracking()
                    .Single(comp => comp.Id == id_comprobante);
            }
            catch
            {
                throw new Exception("Error al obtener el comprobante.");
            }
        }
        public Sucursal obtenerSucursal(long id_empleado)
        {
            try
            {
                return _contexto.Empleados
                .AsNoTracking()
                .Include(emp => emp.Sucursal)
                .Single(emp => emp.Id == id_empleado).Sucursal;
            }
            catch
            {
                throw new Exception("Error al obtener la sucursal.");
            }
        }
    }
}
