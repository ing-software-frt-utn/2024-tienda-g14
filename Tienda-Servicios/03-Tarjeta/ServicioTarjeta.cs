using RestSharp;
using Tienda_Dominio._01_Clases;
using Tienda_Dominio._02_Enumeraciones;
using Tienda_Dominio._03_Interfaces;
using Newtonsoft.Json;

namespace Tienda_Servicios._03_Tarjeta
{
    public class ServicioTarjeta : IServTarjeta
    {
        public void AutorizarVenta(Venta venta, DatosTarjeta datos, Pago pago)
        {
            var client = new RestClient("https://developers.decidir.com/api/v2/tokens");
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("apikey", "4ae76f00234843d1af5994ed4674fd76");
            request.AddParameter("application/json", "{\"card_number\":\"" + datos.numTarjeta + "\",\"card_expiration_month\":\"" + datos.mesVencimiento + "\",\"card_expiration_year\":\"" + datos.anioVencimiento + "\",\"security_code\":\"" + datos.codSeguridad + "\",\"card_holder_name\":\"" + datos.nombreTitular + "\",\"card_holder_identification\":{\"type\":\"dni\",\"number\":\"" + datos.dniTitular + "\"}}", ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            RespuestaApi1 respAPI1 = JsonConvert.DeserializeObject<RespuestaApi1>(response.Content.ToString());

            int montoVenta = (int)(venta.Monto * 100);

            var client2 = new RestClient("https://developers.decidir.com/api/v2/payments");
            var request2 = new RestRequest();
            request2.Method = Method.Post;
            request2.AddHeader("cache-control", "no-cache");
            request2.AddHeader("content-type", "application/json");
            request2.AddHeader("apikey", "3891f691dc4f40b6941a25a68d17c7f4");
            request2.AddParameter("application/json", "{\"site_transaction_id\":\"Venta: " + venta.Id+100 + "\",\"token\":\"" + respAPI1.id + "\",\"payment_method_id\":1,\"bin\":\"" + respAPI1.bin + "\",\"amount\":" + montoVenta.ToString() + ",\"currency\":\"ARS\",\"installments\":1,\"description\":\"\",\"payment_type\":\"single\",\"sub_payments\":[]}", ParameterType.RequestBody);
            RestResponse response2 = client2.Execute(request2);
            if (response2.IsSuccessful)
            {
                RespuestaApi2 respAPI2 = JsonConvert.DeserializeObject<RespuestaApi2>(response2.Content.ToString());

                pago.Ticket = respAPI2.status_details.ticket;

                switch (respAPI2.status)
                {
                    case "approved":
                        pago.Estado = (int)Estados.Aprobado;
                        pago.Observaciones = "APROBADA SIN OBSERVACIONES";
                        break;
                    case "preapproved":
                        pago.Estado = (int)Estados.Pendiente;
                        pago.Observaciones = respAPI2.status;
                        break;
                    case "review":
                        pago.Estado = (int)Estados.Aprobado_con_obs;
                        pago.Observaciones = respAPI2.status;
                        break;
                    case "rejected":
                        pago.Estado = (int)Estados.Rechazado;
                        pago.Observaciones = respAPI2.status;
                        break;
                    default:
                        pago.Estado = (int)Estados.Rechazado;
                        pago.Observaciones = respAPI2.status;
                        break;
                }
            }
            else
            {
                pago.Estado = (int)Estados.Rechazado;
                pago.Ticket = "DATOS ERRONEOS";
                pago.Observaciones = "DATOS ERRONEOS";
            }
        }
    }
}
