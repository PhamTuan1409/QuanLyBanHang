namespace SalesDemo.Models;

// Model tuong ung bang KHACH_HANG trong CSDL
public class KhachHang
{
    public int MaKH { get; set; }
    public string HoTen { get; set; } = "";
    public string HangThanhVien { get; set; } = "Bac";

    public override string ToString() => $"{HoTen} ({HangThanhVien})";
}
