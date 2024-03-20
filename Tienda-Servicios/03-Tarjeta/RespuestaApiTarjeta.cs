namespace Tienda_Servicios._03_Tarjeta
{
    public class RespuestaApi1
    {
        public string id { get; set; }
        public string status { get; set; }
        public int card_number_length { get; set; }
        public string date_created { get; set; }
        public string bin { get; set; }
        public string last_four_digits { get; set; }
        public int security_code_length { get; set; }
        public int expiration_month { get; set; }
        public int expiration_year { get; set; }
        public string date_due { get; set; }
        public Cardholder cardholder { get; set; }
    }
    public class Cardholder
    {
        public Identification identification { get; set; }
        public string name { get; set; }
    }
    public class Identification
    {
        public string type { get; set; }
        public string number { get; set; }
    }
    public class RespuestaApi2
    {
        public int id { get; set; }
        public string site_transaction_id { get; set; }
        public int payment_method_id { get; set; }
        public string card_brand { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public string status { get; set; }
        public StatusDetails status_details { get; set; }
        public string date { get; set; }
        public object customer { get; set; }
        public string bin { get; set; }
        public int installments { get; set; }
        public object first_installment_expiration_date { get; set; }
        public string payment_type { get; set; }
        public List<object> sub_payments { get; set; }
        public string site_id { get; set; }
        public object fraud_detection { get; set; }
        public object aggregate_data { get; set; }
        public object establishment_name { get; set; }
        public object spv { get; set; }
        public object confirmed { get; set; }
        public object pan { get; set; }
        public object customer_token { get; set; }
        public string card_data { get; set; }
        public string token { get; set; }
    }
    public class StatusDetails
    {
        public string ticket { get; set; }
        public string card_authorization_code { get; set; }
        public string address_validation_code { get; set; }
        public object error { get; set; }
    }
}
