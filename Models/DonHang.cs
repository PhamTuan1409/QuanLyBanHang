using System;

namespace SalesDemo.Models;

public class DonHang
{
    public int SoHD { get; set; }
    public DateTime NgayLap { get; set; }
    public decimal TongTien { get; set; }
    public string TrangThai { get; set; } = "";
    public string HinhThucThanhToan { get; set; } = "";

    // Icon mau theo trang thai, dung cho hien thi
    public string TrangThaiHienThi => TrangThai switch
    {
        "DaThanhToan" => "🟢 Hoan thanh",
        "DaHuy" => "🔴 Da huy",
        _ => "🟡 Cho xu ly"
    };
}