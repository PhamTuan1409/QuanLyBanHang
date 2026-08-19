namespace SalesDemo.Models;

// Model tam dung de hien thi 1 dong trong "gio hang" truoc khi luu vao CHI_TIET_HOA_DON
public class ChiTietHoaDonItem
{
    public int MaSP { get; set; }
    public string TenSP { get; set; } = "";
    public int SoLuong { get; set; }
    public decimal DonGia { get; set; }
    public decimal ThanhTien => SoLuong * DonGia;
}
