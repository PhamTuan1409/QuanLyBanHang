using System;
using System.Globalization;
using System.Text;
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

    // Chuyển "Khăn mặt cotton" -> "khan-mat-cotton"
    private static string ChuanHoaTenFile(string ten)
    {
        string s = ten.Trim().ToLowerInvariant();

        // Bỏ dấu tiếng Việt
        string normalized = s.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var c in normalized)
        {
            var uc = CharUnicodeInfo.GetUnicodeCategory(c);
            if (uc != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }
        s = sb.ToString().Normalize(NormalizationForm.FormC);
        s = s.Replace('đ', 'd').Replace('Đ', 'D');

        // Khoảng trắng / ký tự lạ -> dấu gạch ngang
        var result = new StringBuilder();
        foreach (var c in s)
        {
            if (char.IsLetterOrDigit(c)) result.Append(c);
            else if (c == ' ') result.Append('-');
        }

        return result.ToString();
    }

    public string ImageUrl
    {
        get
        {
            string slug = ChuanHoaTenFile(TenSP);
            string uriDuDoan = $"avares://SalesDemo/Assets/Products/{slug}.png";

            if (AssetLoader.Exists(new Uri(uriDuDoan)))
                return uriDuDoan;

            // Không tìm thấy ảnh khớp tên -> dùng ảnh mặc định
            return "avares://SalesDemo/Assets/Products/default.png";
        }
    }

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