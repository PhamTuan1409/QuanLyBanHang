namespace SalesDemo.Models;

public class SanPham
{
    public int MaSP { get; set; }
    public string TenSP { get; set; } = "";
    public decimal GiaNhap { get; set; }
    public decimal GiaBan { get; set; }
    public int SoLuongTon { get; set; }
    public string DonViTinh { get; set; } = "";
    public int MaDanhMuc { get; set; }

    // Các thuộc tính hỗ trợ hiển thị UI (Tránh lỗi AVLN2000)
    public bool ConHang => SoLuongTon > 0;
    public string ImageUrl => "https://via.placeholder.com/150"; 
    public string TrangThaiTonKho => ConHang ? $"Còn {SoLuongTon}" : "Hết hàng";
    public string NutThemVaoGioText => ConHang ? "+ Thêm vào giỏ" : "Hết hàng";

    public override string ToString()
        => $"{TenSP} - {GiaBan:N0} đ";
}
