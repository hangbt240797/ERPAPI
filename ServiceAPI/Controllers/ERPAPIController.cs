using _3SERP.Web;
using DataAccess;
using System;
using System.Net.Http;
using System.Web.Http;
using BusinessLogic;
using Helper;
using ServiceAPI.Entities;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Authentication;
using Dapper;
using Npgsql;
using Newtonsoft.Json;
using System.IO;
namespace ServiceAPI.Controllers
{
    //[AuthenOpenAPI(IPRequired = true)]
    [AuthenOpenAPI]
    public class ERPAPIController : ApiController
    {
        // Lay danh sach vat tu
        [HttpGet]
        [ActionName("GetDSVT")]
        public HttpResponseMessage GetDSVT()
        {
            var result = Dependency.Resolve<IRepositoryDapper>("", new { connectString = PublicBLL.connectionString })
               .SqlQuery<ServiceAPI.Entities.dmvt>("select ma_vt, ten_vt from dmvt where status ='1'", null);
                var kq = new
                {
                    status = "success",
                    code = "VS001",
                    data = new
                    {
                        dsvt = result
                    }
                };
                return new HttpResponseMessage()
                {
                    Content = new StringContent(kq.ToJson(), System.Text.Encoding.UTF8, "application/json")
                };
           
        }

        // Lay danh sach khach hang
        [HttpGet]
        [ActionName("GETDSKH")]
        public HttpResponseMessage GETDSKH(string ckey)
        {
            string sql_query = string.Empty;
            sql_query = @"SELECT * FROM dmkh (@_ckey)";
            DynamicParameters par = new DynamicParameters();
            par.Add("_ckey", ckey.Convert_ToString(), DbType.String);

            var result = Dependency.Resolve<IRepositoryDapper>("", new { connectString = PublicBLL.connectionString })
               .SqlQuery<ServiceAPI.Entities.dmkh>(sql_query, par);

            var kq = new
            {
                status = "success",
                code = "VS001",
                data = new
                {
                    dskh = result
                }
            };
            return new HttpResponseMessage()
            {
                Content = new StringContent(kq.ToJson(), System.Text.Encoding.UTF8, "application/json")
            };
        }

        // Lay danh sach acquy
        [HttpGet]
        [ActionName("GETDSAQ")]
        public HttpResponseMessage GETDSAQ(string ckey)
        {
            string sql_query = string.Empty;
            sql_query = @"SELECT * FROM apilistinfoaccquy (@_cma_sp)";
            DynamicParameters par = new DynamicParameters();
            par.Add("_cma_sp", ckey.Convert_ToString(), DbType.String);

            var result = Dependency.Resolve<IRepositoryDapper>("", new { connectString = PublicBLL.connectionString })
               .SqlQuery<ServiceAPI.Entities.AccquyInfo>(sql_query, par);

            var kq = new
            {
                status = "success",
                code = "VS001",
                data = new
                {
                    dsAccquy = result
                }
            };
            return new HttpResponseMessage()
            {
                Content = new StringContent(kq.ToJson(), System.Text.Encoding.UTF8, "application/json")
            };
        }

        /// <summary>
        ///  Them moi don hang tu website 
        /// </summary>
        /// <param name="customer_info">Thong tin khach hang tu website day ve</param>
        /// <returns>Tra ve ID khach hang duoc them moi</returns>
        [HttpPost]
        [ActionName("AddSO")]
        public HttpResponseMessage AddSO(SOInfo so_info)
        {
            #region ALL
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(new { StatusCode = 400, Message = "Kiểu dữ liệu hoặc độ dài của tham số đầu vào không hợp lệ" }.ToJson(), System.Text.Encoding.UTF8, "application/json")
                    };
                }
                else
                {
                    if (Dependency.Resolve<IRepositoryDapper>("",
                                               new { connectString = PublicBLL.connectionString }).
                                               ExecuteScalar("select 1 from ph64 where so_ct_web=@soctweb limit 1", new { soctweb = so_info.CustInfo.so_ct_web.Convert_ToString() }).Convert_ToInt() == 0)
                    {
                        //kiem tra số chứng từ web đã tồn tại chưa

                        //lay thong tin phieu 
                        /*thong tin ph*/
                        ServiceAPI.Entities.ph64 _ph64 = new ServiceAPI.Entities.ph64();
                        //Dependency.Resolve<IVoucher>().GetNewVoucher<ph74>("PND", ref _ph74, ma_dvcs, user_id);
                        //Phan vung, mien
                        int _vung_mien = so_info.CustInfo.phan_vung;
                        string _ma_dvcs = _vung_mien == 1 ? "ELMICH2" : "ELMICH3";

                        Dependency.Resolve<IVoucher>().GetNewVoucher<ServiceAPI.Entities.ph64>("SO1", ref _ph64, _ma_dvcs, 0);
                        _ph64.status = "1"; //mac dinh la cho duyet
                        _ph64.ma_gd = 3;
                        _ph64.ma_nt = "VND";
                        _ph64.ty_gia = 1;
                        //Tao thong tin kh
                        string _ma_kh = Dependency.Resolve<IRepositoryDapper>("", new
                        {
                            connectString = PublicBLL.connectionString
                        }).ExecuteScalar("select * from createkhweb(@dienthoai,@tenkh,@diachi,@email,@sodh,@tk_online)", new { dienthoai = so_info.CustInfo.dien_thoai, tenkh = so_info.CustInfo.ten_kh, diachi = so_info.CustInfo.dia_chi, email = so_info.CustInfo.e_mail, sodh = _ph64.so_ct, tk_online = so_info.CustInfo.tk_online }).Convert_ToString();

                        decimal _t_tien2 = so_info.ListItems.Sum(a => a.tien2);
                        _ph64.ma_kh = _ph64.ma_kh2 = _ma_kh;
                        _ph64.ma_nvbh = PublicBLL.GetValueOptions("DF_CASHIER_WEBSITE").Convert_ToString();
                        _ph64.ngay_ch = ("1900-01-01").Convert_ToDateTime();
                        _ph64.thoi_gian_ch = " ";
                        _ph64.so_seri = " ";
                        _ph64.ong_ba = _ph64.ong_ba2 = _ph64.ong_ba3 = _ph64.dia_chi3 = " ";
                        _ph64.ma_so_thue = _ph64.ma_so_thue2 = " ";
                        _ph64.so_dh = _ph64.so_ct;
                        _ph64.t_so_luong = so_info.ListItems.Sum(a => a.so_luong);
                        _ph64.t_tien2 = _ph64.t_tien_nt2 = _t_tien2;
                        _ph64.t_ck = _ph64.t_ck_nt = so_info.ListItems.Sum(a => a.ck);
                        _ph64.t_thue = _ph64.t_thue_nt = so_info.ListItems.Sum(a => a.thue);
                        _ph64.t_tt = so_info.ListItems.Sum(a => a.tien2);
                        _ph64.t_tt_nt = so_info.ListItems.Sum(a => a.tien2);
                        _ph64.dien_giai = "Đơn hàng Online";
                        _ph64.dh_web_yn = 1;
                        decimal _tien_tich_luy = Dependency.Resolve<IRepositoryDapper>("",
                          new
                          {
                              connectString = PublicBLL.connectionString
                          }).ExecuteScalar("select coalesce(doanh_so, 0) from webdoanhso(@sdt) ", new { sdt = so_info.CustInfo.dien_thoai.Convert_ToString() }).Convert_ToDecimal();

                        _ph64.tien_tich_luy = _tien_tich_luy;
                        //Data API
                        decimal _tk_tichluy = so_info.CustInfo.tk_tichluy.Convert_ToDecimal();
                        _ph64.so_ct_web = so_info.CustInfo.so_ct_web.Convert_ToString();
                        _ph64.ten_kh = _ph64.ten_kh2 = so_info.CustInfo.ten_kh.Convert_ToString();
                        _ph64.dien_thoai2 = so_info.CustInfo.dien_thoai;
                        _ph64.dia_chi = _ph64.dia_chi2 = so_info.CustInfo.dia_chi;
                        _ph64.ngay_gh = so_info.CustInfo.ngay_gh;
                        _ph64.gt_voucher = so_info.CustInfo.gt_voucher.Convert_ToDecimal();
                        _ph64.tk_tichluy = _tk_tichluy;
                        _ph64.phi_vc = so_info.CustInfo.phi_vc.Convert_ToDecimal();
                        _ph64.ngay_ct = _ph64.ngay_lct = _ph64.ngay_tt = so_info.CustInfo.ngay_dh;
                        _ph64.ten_kh2 = so_info.CustInfo.ten_kh2;
                        _ph64.dia_chi2 = so_info.CustInfo.dia_chi2;
                        _ph64.ma_so_thue2 = so_info.CustInfo.ma_so_thue2;
                        _ph64.tk_online = so_info.CustInfo.tk_online;

                        short _ln = -32768;
                        // lay thong tin tai khoan ck,tai khoan doanh thu 
                        string listmavt = string.Join(",", so_info.ListItems.Select(a => "'" + a.ma_vt + "'"));
                        IEnumerable<taikhoanvt> ttTK = Dependency.Resolve<IRepositoryDapper>("",
                          new
                          {
                              connectString = PublicBLL.connectionString
                          }).SqlQuery<taikhoanvt>(string.Format("select ma_vt, tk_vt, tk_gv, tk_dt, tk_ck, dvt from dmvt where ma_vt in ({0})", listmavt),
                          null);

                        //lay ma kho mặc định
                        string ma_kho = PublicBLL.GetValueOptions("M_KHO_WEB").Convert_ToString();
                        //lay tk_thue
                        string _tk_thue = Dependency.Resolve<IRepositoryDapper>("",
                          new
                          {
                              connectString = PublicBLL.connectionString
                          }).ExecuteScalar("select tk_thue from dmctdf where ma_ct ='SO1' and ma_dvcs= @madvcs", new { madvcs = PublicBLL.GetValueOptions("DF_UNIT_WEBSITE") }).Convert_ToString();

                        /*Thong tin ct*/
                        so_info.ListItems.Select(ctl =>
                         {
                             decimal _gia_web0 = 0, _gia_web2 = 0, _so_luong = 0, _tien_web2 = 0, _gia2 = 0, _gia_truoc_tl2 = 0, _tien_pb = 0, _tien_truoc_tl2 = 0, _tien2 = 0;

                             ctl.stt_rec = _ph64.stt_rec;
                             ctl.ngay_ct = _ph64.ngay_ct;
                             ctl.so_ct = ctl.so_dh = _ph64.so_ct;
                             ctl.ma_ct = _ph64.ma_ct;
                             ctl.ln = ctl.ln_dh = _ln;
                             _so_luong = ctl.so_luong.Convert_ToDecimal();
                             ctl.sl_duyet = ctl.so_luong = _so_luong;
                             ctl.ma_ck = "00";
                             ctl.ma_thue = "10";
                             ctl.app_yn_i = 0;
                             //set cac gia tri null
                             ctl.ma_lo = ctl.ma_vv = ctl.ma_vi_tri = ctl.so_seri_vt = " ";
                             ctl.ma_td = ctl.ma_td2 = ctl.ma_td3 = " ";
                             ctl.sl_td1 = ctl.sl_td2 = ctl.sl_td3 = 0;
                             ctl.gc_td1 = ctl.gc_td2 = ctl.gc_td3 = " ";
                             ctl.ngay_td1 = ctl.ngay_td2 = ctl.ngay_td3 = ("1900-01-01").Convert_ToDateTime();
                             ctl.pt_ck = ctl.tien2 == 0 ? 0 : ctl.ck / ctl.tien2 * 100;
                             ctl.thue_suat = 10;
                             ctl.tk_ck = ttTK.Count() == 0 ? "" : ttTK.Where(a => a.ma_vt.Equals(ctl.ma_vt)).FirstOrDefault().tk_ck;
                             ctl.tk_dt = ttTK.Count() == 0 ? "" : ttTK.Where(a => a.ma_vt.Equals(ctl.ma_vt)).FirstOrDefault().tk_dt;
                             ctl.tk_vt = ttTK.Count() == 0 ? "" : ttTK.Where(a => a.ma_vt.Equals(ctl.ma_vt)).FirstOrDefault().tk_vt;
                             ctl.tk_gv = ttTK.Count() == 0 ? "" : ttTK.Where(a => a.ma_vt.Equals(ctl.ma_vt)).FirstOrDefault().tk_gv;
                             ctl.tk_thue = _tk_thue.Convert_ToString();
                             ctl.dvt = ttTK.Count() == 0 ? "" : ttTK.Where(a => a.ma_vt.Equals(ctl.ma_vt)).FirstOrDefault().dvt;
                             ctl.he_so = 1;
                             ctl.ma_kho = ma_kho;
                             // gia, tien 
                             _gia_web0 = Math.Round(ctl.gia0, PublicBLL.GetValueOptions("M_ROUND_GIA").Convert_ToInt());
                             _gia_web2 = Math.Round(ctl.gia2, PublicBLL.GetValueOptions("M_ROUND_GIA").Convert_ToInt());
                             //Gia web truoc VAT
                             ctl.gia_web0_tvat = _gia_web0;
                             ctl.gia_web2_tvat = _gia_web2;
                             //Gia sau VAT
                             ctl.gia_web0 = Math.Round(_gia_web0 / (1.1).Convert_ToDecimal(), PublicBLL.GetValueOptions("M_ROUND_GIA").Convert_ToInt());
                             ctl.gia_web2 = Math.Round(_gia_web2 / (1.1).Convert_ToDecimal(), PublicBLL.GetValueOptions("M_ROUND_GIA").Convert_ToInt());

                             ctl.ck_nt = ctl.ck = Math.Round(ctl.ck, PublicBLL.GetValueOptions("M_ROUND_TIEN").Convert_ToInt());
                             _tien_web2 = Math.Round(ctl.tien2, PublicBLL.GetValueOptions("M_ROUND_TIEN").Convert_ToInt());
                             ctl.tien_web2 = _tien_web2;
                             ctl.pt_ck_g3 = Math.Round(100 * (1 - _gia_web2 / _gia_web0), 5);
                             //Gia chuan 
                             decimal _gia0 = Dependency.Resolve<IRepositoryDapper>("",
                               new
                               {
                                   connectString = PublicBLL.connectionString
                               }).ExecuteScalar("select coalesce(gia0, 0) from getmainitemprice(@madvcs, '', @mavt, 1, 1) ", new { madvcs = _ma_dvcs, mavt = ctl.ma_vt.Convert_ToString() }).Convert_ToDecimal();

                             ctl.gia_ban_nt = ctl.gia_ban = Math.Round(_gia0, PublicBLL.GetValueOptions("M_ROUND_GIA").Convert_ToInt());
                             ctl.tien_ban = ctl.tien_ban_nt = Math.Round(_so_luong * _gia0, PublicBLL.GetValueOptions("M_ROUND_TIEN").Convert_ToInt());

                             _gia_truoc_tl2 = Math.Round(_gia_web2 / (1.1).Convert_ToDecimal(), PublicBLL.GetValueOptions("M_ROUND_GIA").Convert_ToInt());
                             _tien_truoc_tl2 = Math.Round((_gia_web2 / (1.1).Convert_ToDecimal()) * _so_luong, PublicBLL.GetValueOptions("M_ROUND_TIEN").Convert_ToInt());

                             _tien_pb = Math.Round((_tk_tichluy / 1.1.Convert_ToDecimal()) * (_tien_web2 / _t_tien2), PublicBLL.GetValueOptions("M_ROUND_TIEN").Convert_ToInt());

                             _tien2 = _tien_truoc_tl2 - _tien_pb;
                             ctl.tien2 = ctl.tien_nt2 = Math.Round(_tien2, PublicBLL.GetValueOptions("M_ROUND_TIEN").Convert_ToInt());
                             _gia2 = Math.Round(_tien2 / _so_luong, 5);
                             ctl.gia_truoc_tl2 = _gia_truoc_tl2;
                             ctl.tien_truoc_tl2 = _gia_truoc_tl2 * _so_luong;
                             ctl.gia2 = ctl.gia_nt2 = _gia2;
                             ctl.thue = ctl.thue_nt = Math.Round((_tien2 * 0.1.Convert_ToDecimal()), PublicBLL.GetValueOptions("M_ROUND_TIEN").Convert_ToInt());
                             ctl.pt_ck_g3 = Math.Round(100 * (1 - _gia_web2 / _gia_web0), PublicBLL.GetValueOptions("M_ROUND_SL").Convert_ToInt());
                             ctl.gia = Math.Round((_gia0 * _gia_web2 / _gia_web0), PublicBLL.GetValueOptions("M_ROUND_GIA").Convert_ToInt());

                             _ln += 8;
                             return ctl;
                         })
                       .ToList();

                        //
                        // save ph64
                        int ins_ph64 = Dependency.Resolve<IRepositoryDapper>("",
                           new
                           {
                               connectString = PublicBLL.connectionString
                           }).Insert(_ph64);


                        int ins_ct64 = Dependency.Resolve<IRepositoryDapper>("",
                          new
                          {
                              connectString = PublicBLL.connectionString
                          }).InsertList(so_info.ListItems);
                        int _post = Dependency.Resolve<IRepositoryDapper>("",
                        new
                        {
                            connectString = PublicBLL.connectionString
                        }).ExecuteCommand("SELECT postso(@sttrec)", new { sttrec = _ph64.stt_rec });

                        if (ins_ph64 == 1 && _post == -1)
                        {
                            return new HttpResponseMessage()
                            {
                                Content = new StringContent(new { ID_SO = _ph64.so_ct, StatusCode = 200, Message = "Thành công" }.ToJson(), System.Text.Encoding.UTF8, "application/json")
                            };
                        }
                        else
                            return new HttpResponseMessage()
                            {
                                Content = new StringContent(new { StatusCode = 500, Message = "Có lỗi phía Server" }.ToJson(), System.Text.Encoding.UTF8, "application/json")
                            };

                    }
                    else return new HttpResponseMessage()
                    {
                        Content = new StringContent(new { StatusCode = 400, Message = "Số đơn hàng web đã tồn tại trong hệ thống ERP" }.ToJson(), System.Text.Encoding.UTF8, "application/json")
                    };
                }
            }
            catch (Exception e)
            {

                return new HttpResponseMessage()
                {
                    Content = new StringContent(new { StatusCode = 500, Message = "Có lỗi phía Server" }.ToJson(), System.Text.Encoding.UTF8, "application/json")
                };
            }
            #endregion
        }
        //DH
        [HttpGet]
        [ActionName("layDanhSachHoaDonChoXuat")]
        public HttpResponseMessage layDanhSachHoaDonChoXuat(string dfrom, string dto, string cma_dvcs)
        {
            try
            {
                string sql_query = string.Empty;
                sql_query = @"SELECT * FROM getdatainvoice(@dfrom, @dto, @_ma_dvcs)";
                DynamicParameters par = new DynamicParameters();
                par.Add("dfrom", dfrom.Convert_ToDateTime(), DbType.Date);
                par.Add("dto", dto.Convert_ToDateTime(), DbType.Date);
                par.Add("_ma_dvcs", cma_dvcs.Convert_ToString());

                var result = Dependency.Resolve<IRepositoryDapper>("", new { connectString = PublicBLL.connectionSysString })
                    .SqlQuery<DatasetReturncs>(sql_query, par);

                List<item> data = new List<item>();
                item itemData;
                var listph = result.GroupBy(a => a.id).Select(a => a.First());

                foreach (var item in listph)
                {
                    itemData = new item();
                    itemData.invoice = result.Where(a => a.id.Equals(item.id)).Select(a => new invoice
                    {
                        id = a.id,
                        id_form_release = a.id_form_release,
                        invoice_number = a.invoice_number,
                        adjustment_type = a.adjustment_type,
                        currency_code = a.currency_code,
                        exchange_rate = a.exchange_rate,
                        payment_option = a.payment_option,
                        id_user_name = a.id_user_name,
                        user_name = a.user_name,
                        note = a.note,
                        department_code = a.department_code,
                        invoice_type_code = a.invoice_type_code,
                        no_print_convert = a.no_print_convert,
                        month = a.inv_month,
                        from_date = a.from_date,
                        to_date = a.to_date,
                        is_crm_auto_value = a.is_crm_auto_value,
                        total_number = a.total_number,
                        total = a.total,
                        total_vat_5 = a.total_vat_5,
                        total_vat_10 = a.total_vat_10,
                        total_vat_x = a.total_vat_x,
                        total_vat = a.total_vat,
                        total_discount = a.total_discount,
                        total_payment = a.total_payment,
                        receipts_no = a.receipts_no
                    }).FirstOrDefault();
                    itemData.customer = result.Where(a => a.id.Equals(item.id)).Select(a => new customer
                    {
                        id = a.cus_id,
                        code = a.code,
                        tax_number = a.tax_number,
                        unit_name = a.unit_name,
                        unit_type = a.unit_type,
                        address = a.address,
                        contact = a.contact,
                        email = a.email,
                        clock_code = a.clock_code,
                        people_num = a.people_num,
                        point_num = a.point_num,

                    }).FirstOrDefault();
                    itemData.order = result.Where(a => a.id.Equals(item.id)).Select(a => new order
                    {
                        meter_name = a.meter_name,
                        item_code = a.item_code,
                        item_name = a.item_name,
                        amount = a.amount,
                        price = a.price,
                        previous_index = a.previous_index,
                        current_index = a.current_index,
                        factor = a.factor,
                        unit_name = a.ord_unit_name,
                        money = a.money,
                        discount_percentage = a.discount_percentage,
                        money_discount = a.money_discount,
                        vat_percentage = a.vat_percentage,
                        money_vat = a.money_vat,
                        total_payment = a.ord_total_payment,
                        stt = a.ord_order
                    }).ToList();

                    data.Add(itemData);
                }
                var kq = new
                {
                    status = "success",
                    code = "VS001",
                    data = new
                    {
                        item = data
                    }
                };
                return new HttpResponseMessage()
                {
                    Content = new StringContent(kq.ToJson(), System.Text.Encoding.UTF8, "application/json")
                };
            }
            catch (Exception e)
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent(new { StatusCode = 500, Message = "Có lỗi phía Server" }.ToJson(), System.Text.Encoding.UTF8, "application/json")
                };
            }
        }
        //HDDT
        [HttpGet]
        [ActionName("layDanhMucHangHoa")]
        public HttpResponseMessage layDanhMucHangHoa(string cma_vt)
        {
            string sql_query = string.Empty;
            sql_query = @"SELECT * FROM dmvt(@_ma_vt)";
            var result = Dependency.Resolve<IRepositoryDapper>("", new { connectString = PublicBLL.connectionSysString })
                .SqlQuery<ServiceAPI.Entities.dmvt>(sql_query, new { _ma_vt = cma_vt.Convert_ToString() });
            var kq = new
            {
                status = "success",
                code = "VS001",
                data = new
                {
                    item = result
                }
            };
            return new HttpResponseMessage()
            {
                Content = new StringContent(kq.ToJson(), System.Text.Encoding.UTF8, "application/json")
            };
        }
      
        //Process Up
        [HttpPost]
        [ActionName("capNhatHoaDon")]
        public HttpResponseMessage capNhatHoaDon(object data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(new { StatusCode = 400, Message = "Kiểu dữ liệu hoặc độ dài của tham số đầu vào không hợp lệ" }.ToJson(), System.Text.Encoding.UTF8, "application/json")
                    };
                }
                else
                {
                    var data_order = JsonConvert.DeserializeObject<order_in>(data.ToString());

                    string sql_query = string.Empty;
                    string _ma_ct = data_order.order.id.Substring(data_order.order.id.Length - 3);
                    string _m_ph = Dependency.Resolve<IRepositoryDapper>("",
                          new
                          {
                              connectString = PublicBLL.connectionSysString
                          }).ExecuteScalar("select m_ph from dmct where ma_ct= @mact", new { mact = _ma_ct }).Convert_ToString();

                    sql_query = string.Format(@"UPDATE {0} SET so_hd = @sohd, ngay_hd = @ngayhd, so_seri = @seri, status_hddt = 1, invoice_type = @type WHERE stt_rec = @stt_rec; ", _m_ph);
                    sql_query += @"UPDATE ctgt20 SET so_ct = @sohd, ngay_ct = @ngayhd, so_seri = @seri, mau_hd = @mau_hd WHERE stt_rec = @stt_rec";

                    //Log
                    string str_Log = string.Format("APIcapNhatHoaDon(ID={0}, So_Hd={1}, Type={2}, time={3}, person_name={4});", data_order.order.id.ToString(), data_order.order.item_code.ToString(), data_order.order.type.ToString(), data_order.order.time.ToString(), data_order.order.person_name.ToString());
                    WriteToFile(DateTime.Now + "-" + str_Log);
                    //
                    int _return_num = 1;
                    using (IDbConnection cn = new NpgsqlConnection(PublicBLL.connectionSysString))
                    {
                        cn.Open();
                        _return_num = cn.Execute(sql_query, new
                        {
                            sohd = data_order.order.item_code.ToString(),
                            ngayhd = data_order.order.time.Convert_ToDateTime(),
                            stt_rec = data_order.order.id.ToString(),
                            seri = data_order.order.symbol.ToString(),
                            mau_hd = data_order.order.template_code.ToString(),
                            type = data_order.order.type.ToString()
                        })
                      ;
                    }
                    var kq = new
                    {
                        status = _return_num == 2 ? "success" : "err",
                        code = "VS001",
                        data = new
                        {
                        }
                    };

                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(kq.ToJson(), System.Text.Encoding.UTF8, "application/json")
                    };
                }
            }
            catch (Exception e)
            {

                return new HttpResponseMessage()
                {
                    Content = new StringContent(new { StatusCode = 500, Message = "Có lỗi phía Server" }.ToJson(), System.Text.Encoding.UTF8, "application/json")
                };
            }
        }

        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog.txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }

    }
}
