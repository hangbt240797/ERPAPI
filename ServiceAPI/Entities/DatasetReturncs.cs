using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAPI.Entities
{
   public class DatasetReturncs
    {
        public string id { get; set; }
        public string id_form_release { get; set; }
        public string invoice_number { get; set; }
        public short adjustment_type { get; set; }
        public string currency_code { get; set; }
        public decimal exchange_rate { get; set; }
        public string payment_option { get; set; }
        public string id_user_name { get; set; }
        public string user_name { get; set; }
        public string note { get; set; }
        public string department_code { get; set; }
        public string invoice_type_code { get; set; }
        public short no_print_convert { get; set; }
        public string inv_month { get; set; }
        public System.DateTime from_date { get; set; }
        public System.DateTime to_date { get; set; }
        public short is_crm_auto_value { get; set; }
        public decimal total_number { get; set; }
        public decimal total { get; set; }
        public decimal total_vat_5 { get; set; }
        public decimal total_vat_10 { get; set; }
        public decimal total_vat_x { get; set; }
        public decimal total_vat { get; set; }
        public decimal total_discount { get; set; }
        public decimal total_payment { get; set; }
        public string cus_id { get; set; }
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
        public string meter_name { get; set; }
        public string item_code { get; set; }
        public string item_name { get; set; }
        public decimal amount { get; set; }
        public decimal price { get; set; }
        public short previous_index { get; set; }
        public short current_index { get; set; }
        public short factor { get; set; }
        public string ord_unit_name { get; set; }
        public decimal money { get; set; }
        public decimal discount_percentage { get; set; }
        public decimal money_discount { get; set; }
        public decimal vat_percentage { get; set; }
        public decimal money_vat { get; set; }
        public decimal ord_total_payment { get; set; }
        public short ord_order { get; set; }
        public string receipts_no { get; set; }
    }
     
}
