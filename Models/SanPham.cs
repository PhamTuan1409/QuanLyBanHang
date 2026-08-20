namespace SalesDemo.Models;

public class SanPham
{
    public int MaSP { get; set; }
    public string TenSP { get; set; } = "";
    public decimal GiaBan { get; set; }
    public int SoLuongTon { get; set; }
    public string DonViTinh { get; set; } = "";
    public int MaDanhMuc { get; set; }

    public bool ConHang => SoLuongTon > 0;

    public string TrangThaiTonKho
    {
        get
        {
            if (SoLuongTon <= 0) return "🔴 Hết hàng";
            if (SoLuongTon <= 5) return $"🟠 Chỉ còn {SoLuongTon}";
            return $"🟢 Còn {SoLuongTon}";
        }
    }

    public string NutThemVaoGioText => ConHang ? "+ Giỏ hàng" : "Hết hàng";

    // Anh minh hoa rieng cho tung san pham, dua theo MaSP (khop thu tu insert trong CreateDatabase.sql)
    public string ImageUrl
    {
        get
        {
            string keyword = MaSP switch
            {
                1 => "pillow",
                2 => "travel-pillow",
                3 => "blanket",
                4 => "towel",
                5 => "candle",
                6 => "tumbler",
                7 => "water-bottle",
                8 => "coffee-mug",
                9 => "lunchbox",
                10 => "notebook",
                11 => "pen",
                12 => "tote-bag",
                13 => "night-lamp",
                14 => "headphones",
                15 => "phone-stand",
                _ => "product"
            };
            return $"https://loremflickr.com/300/300/{keyword}?lock={MaSP}";
        }
    }

    public override string ToString() => $"{TenSP} (Còn: {SoLuongTon})";
}