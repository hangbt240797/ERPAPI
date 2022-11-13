using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAPI.Entities
{
    public class DataReturn
    {
        List<item> data { get; set; }
    }
    public class item
    {
        public invoice invoice { get; set; }
        public customer customer { get; set; }

        public List<order> order { get; set; }
        public List<file> files { get; set; }
    }
    public class invoice
    {
        public string id { get; set; }
        public string id_form_release { get; set; }
        public string invoice_number { get; set; }
        public int adjustment_type { get; set; }
        public string currency_code { get; set; }
        public decimal exchange_rate { get; set; }
        public string payment_option { get; set; }
        public string id_user_name { get; set; }
        public string user_name { get; set; }
        public string note { get; set; }
        public string department_code { get; set; }
        public string invoice_type_code { get; set; }
        public int no_print_convert { get; set; }
        public string month { get; set; }
        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }
        public int is_crm_auto_value { get; set; }
        public decimal total_number { get; set; }
        public decimal total { get; set; }
        public decimal total_vat_5 { get; set; }
        public decimal total_vat_10 { get; set; }
        public decimal total_vat_x { get; set; }
        public decimal total_vat { get; set; }
        public decimal total_discount { get; set; }
        public decimal total_payment { get; set; }
        public string receipts_no { get; set; }
    }
    public class order_in
    {
        public orders order { get; set; } 
    }
    public class orders
    { 
        public string id { get; set; }
        public string person_name { get; set; }
        public string template_code { get; set; }
        public string content { get; set; }
        public decimal money { get; set; }
        public string time { get; set; }
        public string item_code { get; set; }
        public string attach_file_name { get; set; }
        public string attach_file { get; set; }
        public string link_file { get; set; }
        public string type { get; set; }
        public string type_name { get; set; }
        public string symbol { get; set; }
        public string crm { get; set; }
    }
    public class customer
    {
        public string id { get; set; }
        public string code { get; set; }
        public string tax_number { get; set; }
        public string unit_name { get; set; }
        public string unit_type { get; set; }
        public string address { get; set; }
        public string contact { get; set; }
        public string email { get; set; }
        public string clock_code { get; set; }
        public string people_num { get; set; }
        public string point_num { get; set; }
    }
    public class order
    {
        public string meter_name { get; set; }
        public string item_code { get; set; }
        public string item_name { get; set; }
        public decimal amount { get; set; }
        public decimal price { get; set; }
        public int previous_index { get; set; }
        public int current_index { get; set; }
        public int factor { get; set; }
        public string unit_name { get; set; }
        public decimal money { get; set; }
        public decimal discount_percentage { get; set; }
        public decimal money_discount { get; set; } 
        public decimal vat_percentage { get; set; }
        public decimal money_vat { get; set; }
        public decimal total_payment { get; set; }
        public int stt { get; set; }
    }
    public class file
    {

    }
}
