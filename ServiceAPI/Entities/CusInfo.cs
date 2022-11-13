using System.ComponentModel.DataAnnotations;
namespace ServiceAPI.Entities
{
    public class CusInfo
    {

        [MaxLength(15)]
        [Required]
        public string so_ct_web { get; set; }
        [MaxLength(128)]
        public string ten_kh { get; set; }
        [MaxLength(11)]
        [Required]
        public string dien_thoai { get; set; }
        [MaxLength(128)]
        public string e_mail { get; set; }
        [MaxLength(128)]
        public string dia_chi { get; set; }
        public System.DateTime ngay_gh { get; set; }
        public decimal gt_voucher { get; set; }
        public decimal tk_tichluy { get; set; }
        public decimal phi_vc { get; set; }
        public System.DateTime ngay_dh { get; set; }
        public string ten_kh2 { get; set; }
        public string dia_chi2 { get; set; }
        public string ma_so_thue2 { get; set; }
        public int tk_online { get; set; }
        public int phan_vung { get; set; }
    }
}