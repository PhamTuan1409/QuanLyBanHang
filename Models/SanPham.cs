using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

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

    public bool ConHang => SoLuongTon > 0;

   public string ImageUrl => MaSP switch
{
    1 => "avares://SalesDemo/Assets/Products/goi-ngu-cotton.png",
    2 => "avares://SalesDemo/Assets/Products/goi-co-du-lich.png",
    3 => "avares://SalesDemo/Assets/Products/chan-mong-mua-he.png",
    4 => "avares://SalesDemo/Assets/Products/default.png",
    5 => "avares://SalesDemo/Assets/Products/default.png",
    6 => "avares://SalesDemo/Assets/Products/ly-giu-nhiet-500ml.png",
    7 => "avares://SalesDemo/Assets/Products/binh-nuoc-the-thao.png",
    8 => "avares://SalesDemo/Assets/Products/coc-su.png",
    9 => "avares://SalesDemo/Assets/Products/default.png",
    10 => "avares://SalesDemo/Assets/Products/so-tay.png",
    11 => "avares://SalesDemo/Assets/Products/but-gel.png",
    12 => "avares://SalesDemo/Assets/Products/tai-nghe-bluetooth.png",
    13 => "avares://SalesDemo/Assets/Products/den-ngu-mini.png",
    14 => "avares://SalesDemo/Assets/Products/tai-nghe-bluetooth.png",
    15 => "avares://SalesDemo/Assets/Products/default.png",
    _ => "avares://SalesDemo/Assets/Products/default.png"
};

    public Bitmap? ProductImage
{
    get
    {
        try
        {
            var uri = new Uri(ImageUrl);
            var asset = AssetLoader.Open(uri);
            return new Bitmap(asset);
        }
        catch
        {
            return null;
        }
    }
}
    public string TrangThaiTonKho
        => ConHang ? $"Còn {SoLuongTon}" : "Hết hàng";

    public string NutThemVaoGioText
        => ConHang ? "+ Thêm vào giỏ" : "Hết hàng";

    public override string ToString()
        => $"{TenSP} - {GiaBan:N0} đ";
}