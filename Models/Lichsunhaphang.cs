using System;

namespace SalesDemo.Models;

public class LichSuNhapHang
{
    public DateTime NgayNhap { get; set; }
    public string TenSP { get; set; } = "";
    public int SoLuong { get; set; }
    public decimal ThanhTien { get; set; }

    public string NgayNhapHienThi => NgayNhap.ToString("dd/MM/yyyy HH:mm");
}