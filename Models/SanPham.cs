namespace SalesDemo.Models;

public class SanPham
{
    public int MaSP { get; set; }
    public string TenSP { get; set; } = "";
    public decimal GiaBan { get; set; }
    public int SoLuongTon { get; set; }
    public string DonViTinh { get; set; } = "";

    public bool ConHang => SoLuongTon > 0;

    public string TrangThaiTonKho
    {
        get
        {
            if (SoLuongTon <= 0) return "🔴 Het hang";
            if (SoLuongTon <= 5) return $"🟠 Chi con {SoLuongTon}";
            return $"🟢 Con {SoLuongTon}";
        }
    }

    public string NutThemVaoGioText => ConHang ? "+ Gio" : "Het hang";
    public string NutThemVaoGioClass => ConHang ? "secondary" : "disabled2";

    public override string ToString() => $"{TenSP} (Ton: {SoLuongTon})";
}