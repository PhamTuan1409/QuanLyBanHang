using System;

namespace SalesDemo.Models;

public class KhachHang
{
    public int MaKH { get; set; }
    public string HoTen { get; set; } = "";
    public string SoDienThoai { get; set; } = "";
    public string? Email { get; set; }
    public string? DiaChi { get; set; }
    public string HangThanhVien { get; set; } = "Bac";
    public DateTime NgayDangKy { get; set; }

    public string HienThi =>
        string.IsNullOrWhiteSpace(SoDienThoai) ? HoTen : $"{HoTen} - {SoDienThoai}";
}