using Microsoft.EntityFrameworkCore;
using Tienda_Dominio._01_Clases;


namespace Tienda_Datos._01_DB
{
    public class TiendaContexto : DbContext
    {
        public TiendaContexto(DbContextOptions<TiendaContexto> options) : base(options) { }
        public DbSet<Articulo> Articulos { get; set; }
        public DbSet<Autorizacion_Afip> Autorizaciones_Afip { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Ciudad> Ciudades { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Color> Colores { get; set; }
        public DbSet<Comprobante> Comprobantes { get; set; }
        public DbSet<Condicion_Tributaria> Condiciones_Tributarias { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }
        public DbSet<Linea_Venta> Lineas_Venta { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Metodo_Pago> Metodos_Pago { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Pais> Paises { get; set; }
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<Punto_Venta> Puntos_Venta { get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Talle> Talles { get; set; }
        public DbSet<Tipo_Talle> Tipos_Talle { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<ComprobanteCondicion> ComprobanteCondicion { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ComprobanteCondicion>()
                .HasKey(x => new { x.Emite, x.Recibe });

            modelBuilder.Entity<Inventario>()
                .HasOne(i => i.Articulo)
                .WithMany(Articulo => Articulo.Inventarios)
                .HasForeignKey(i => i.Articulo_Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Linea_Venta>()
                .HasKey(l => new { l.Venta_Id, l.Item_num });

            modelBuilder.Entity<Linea_Venta>()
                .HasOne(l => l.Venta)
                .WithMany(Venta => Venta.Lineas_Ventas)
                .HasForeignKey(l => l.Venta_Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Punto_Venta>()
                .HasKey(p => new { p.Sucursal_Id, p.Numero_Punto_Venta });

            modelBuilder.Ignore<DatosTarjeta>();
            modelBuilder.Ignore<Configuraciones>();
            modelBuilder.Ignore<DatosAfip>();
            modelBuilder.Ignore<Factura>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
