using Tienda_Dominio._01_Clases;
using System.Collections;
using NUnit.Framework.Internal.Execution;
using Tienda_Aplicacion._01_Venta;

namespace Tienda_Test
{
    public class Articulo_ActualizarInventarioTests
    {
        [SetUp]
        public void Setup()
        {
        }

        //No hay inventarios asociados al articulo
        //Deberia devolver mensaje de error
        [Test]
        public void InventariosEsNull_DevuelveError()
        {
            Articulo articulo = new Articulo();
            string mensajeEsperado = "Inventario igual a null.";

            Exception excepcion = Assert.Throws<Exception>(() => articulo.ActualizarInventario(new Venta()));

            // Assert
            Assert.That(mensajeEsperado, Is.EqualTo(excepcion.Message));
        }

        //Hay inventarios asociados al articulo, pero en una sucursal diferente a la que se está realizando la venta
        //Deberia devolver mensaje de error
        [Test]
        public void InventariosNoEsNull_InventariosQuedaVacio_DevuelveError()
        {
            Venta venta = new Venta { Sucursal_Id = 1 };
            Articulo articulo = new Articulo { Inventarios = new List<Inventario> { new Inventario { Sucursal_Id = 2, Cantidad = 15 } } };
            string mensajeEsperado = "Articulo sin inventario.";

            Exception excepcion = Assert.Throws<Exception>(() => articulo.ActualizarInventario(venta));

            // Assert
            Assert.That(mensajeEsperado, Is.EqualTo(excepcion.Message));
        }

        //Hay inventarios asociados al articulo en la sucursal donde se está realizando la venta
        //No hay lineas de venta
        //Deberia mantenerse sin modificaciones la cantidad
        [Test]
        public void InventariosNoEsNull_InventariosTieneElementos_LineasVentaEsNull_NoDecreceCantidad()
        {

            Venta venta = new Venta { Sucursal_Id = 1 };
            Articulo articulo = new Articulo { Inventarios = new List<Inventario> { new Inventario { Id = 1, Sucursal_Id = 1, Cantidad = 15 } } };
            long esperadoSinCambio = 15;

            articulo.ActualizarInventario(venta);

            // Assert
            Assert.That(esperadoSinCambio, Is.EqualTo(articulo.Inventarios.Single(inv => inv.Id == 1).Cantidad));
        }

        //Hay inventarios asociados al articulo en la sucursal donde se está realizando la venta
        //Hay lineas de venta que contienen a un inventario del articulo
        //Deberia verse afectada la cantidad del inventario
        [Test]
        public void InventariosNoEsNull_InventariosTieneElementos_LineasVentaContieneInventario_DecreceCantidad()
        {
            Venta venta = new Venta { Sucursal_Id = 1, Lineas_Ventas = new List<Linea_Venta> { new Linea_Venta { Inventario_Id = 2 , Cantidad = 2} } };
            Articulo articulo = new Articulo { Inventarios = new List<Inventario> { new Inventario { Id = 2, Sucursal_Id = 1, Cantidad = 15 } } };
            long esperadoConCambio = 13;

            articulo.ActualizarInventario(venta);
            // Assert
            Assert.That(esperadoConCambio, Is.EqualTo(articulo.Inventarios.Single(inv => inv.Id == 2).Cantidad));
        }

        //Hay inventarios asociados al articulo en la sucursal donde se está realizando la venta
        //Hay lineas de venta que no contienen a un inventario del articulo
        //Deberia mantenerse sin modificaciones la cantidad
        [Test]
        public void InventariosNoEsNull_InventariosTieneElementos_LineasVentaNoContieneInventario_NoDecreceCantidad()
        {
            Venta venta = new Venta { Sucursal_Id = 1, Lineas_Ventas = new List<Linea_Venta> { new Linea_Venta { Inventario_Id = 3, Cantidad = 2 } } };
            Articulo articulo = new Articulo { Inventarios = new List<Inventario> { new Inventario { Id = 2, Sucursal_Id = 1, Cantidad = 15 } } };
            long esperadoSinCambio = 15;

            articulo.ActualizarInventario(venta);

            // Assert
            Assert.That(esperadoSinCambio, Is.EqualTo(articulo.Inventarios.Single(inv => inv.Id == 2).Cantidad));
        }

        //Hay inventarios asociados al articulo en la sucursal donde se está realizando la venta
        //Hay lineas de venta que contienen a un inventario del articulo, pero no a todos
        //Deberia verse afectada la cantidad del inventario contenido en la linea de venta
        //Para el resto deberia mantenerse sin modificaciones la cantidad
        [Test]
        public void InventariosNoEsNull_InventariosTieneVariosElementos_LineasVentaContiene1Inventario_DecreceCantidadSoloEnEse()
        {
            Venta venta = new Venta { Sucursal_Id = 1, Lineas_Ventas = new List<Linea_Venta> { new Linea_Venta { Inventario_Id = 3, Cantidad = 2 } } };
            Articulo articulo = new Articulo { Inventarios = new List<Inventario> { new Inventario { Id = 2, Sucursal_Id = 1, Cantidad = 15 }, new Inventario { Id = 3, Sucursal_Id = 1, Cantidad = 10 } } };
            long esperadoSinCambio = 15;
            long esperadoConCambio = 8;

            articulo.ActualizarInventario(venta);

            // Assert
            Assert.That(esperadoSinCambio, Is.EqualTo(articulo.Inventarios.Single(inv => inv.Id == 2).Cantidad));

            Assert.That(esperadoConCambio, Is.EqualTo(articulo.Inventarios.Single(inv => inv.Id == 3).Cantidad));
        }
    }
}
