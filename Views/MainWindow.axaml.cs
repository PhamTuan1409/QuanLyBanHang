using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using SalesDemo.Models;
using SalesDemo.Services;
using System.Linq;

namespace SalesDemo.Views;
public partial class MainWindow : Window
{
    private readonly DatabaseHelper _db = new();

    private readonly ObservableCollection<SanPham> _dsSanPham = new();
    private readonly ObservableCollection<ChiTietHoaDonItem> _gioHang = new();
    private readonly ObservableCollection<DonHang> _dsDonHang = new();

    private bool _daKhoiTaoXong = false;

    public MainWindow()
    {
        InitializeComponent();

        ListSanPham.ItemsSource = _dsSanPham;
        ListGioHang.ItemsSource = _gioHang;
        ListDonHang.ItemsSource = _dsDonHang;

        _ = TaiDuLieuBanDauAsync();
    }

    private async Task TaiDuLieuBanDauAsync()
    {
        SetStatus("Dang ket noi co so du lieu...");
        try
        {
            var danhMucList = await Task.Run(() => _db.LayDanhSachDanhMuc());

            danhMucList.Insert(0, new DanhMuc
{
            MaDanhMuc = 0,
            TenDanhMuc = "Tất cả"
});

           CboDanhMuc.ItemsSource = danhMucList;
           CboDanhMuc.SelectedIndex = 0;

            var khachHangList = await Task.Run(() => _db.LayDanhSachKhachHang());
            CboKhachHang.ItemsSource = khachHangList;

            await TaiSanPhamAsync();
            CboSanPham.ItemsSource = _dsSanPham;
            // Combobox "Nhap hang" cung dung chung nguon du lieu san pham
            CboSanPhamNhap.ItemsSource = _dsSanPham;

            await TaiDonHangAsync();

            _daKhoiTaoXong = true;
            SetStatus($"Da tai {_dsSanPham.Count} san pham.");
        }
        catch (Exception ex)
        {
            SetStatus("Loi ket noi CSDL — xem chi tiet ben duoi.");
            ShowInlineError("Khong the ket noi database: " + ex.Message);
        }
    }

    private async Task TaiSanPhamAsync()
{
    string tuKhoa = TxtTimKiem?.Text ?? "";
    int maDanhMuc = (CboDanhMuc?.SelectedItem as DanhMuc)?.MaDanhMuc ?? 0;

    var list = await Task.Run(() =>
        _db.LayDanhSachSanPham(tuKhoa, maDanhMuc)
    );

    if (maDanhMuc == 0)
    {
        list = list
            .OrderBy(sp => sp.MaSP)
            .ToList();
    }

    _dsSanPham.Clear();

    foreach (var sp in list)
    {
        _dsSanPham.Add(sp);
    }
}

    private async Task TaiDonHangAsync()
    {
        var list = await Task.Run(() => _db.LayDanhSachDonHang());
        _dsDonHang.Clear();
        foreach (var dh in list) _dsDonHang.Add(dh);
    }

    private async void OnTimKiemThayDoi(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        if (!_daKhoiTaoXong) return;
        await TaiSanPhamAsync();
    }

    private async void OnDanhMucThayDoi(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        if (!_daKhoiTaoXong) return;
        await TaiSanPhamAsync();
    }

    private void SetStatus(string text) => Dispatcher.UIThread.Post(() => TxtStatus.Text = text);

    private void ShowInlineError(string message)
    {
        Dispatcher.UIThread.Post(() =>
        {
            var dialog = new Window
            {
                Width = 420,
                Height = 180,
                Title = "Thong bao",
                Content = new TextBlock
                {
                    Text = message,
                    Margin = new Avalonia.Thickness(20),
                    TextWrapping = Avalonia.Media.TextWrapping.Wrap
                }
            };
            dialog.Show();
        });
    }

    private void ShowSuccessDialog(int soHD, decimal tongTien)
    {
        Dispatcher.UIThread.Post(() =>
        {
            var panel = new StackPanel { Margin = new Avalonia.Thickness(24), Spacing = 10 };
            panel.Children.Add(new TextBlock { Text = "✅ Dat hang thanh cong!", FontSize = 18, FontWeight = Avalonia.Media.FontWeight.Bold, Foreground = Avalonia.Media.Brushes.Green });
            panel.Children.Add(new TextBlock { Text = $"Ma hoa don: HD{soHD:D5}" });
            panel.Children.Add(new TextBlock { Text = $"Tong tien: {tongTien:N0} đ" });

            var dialog = new Window
            {
                Width = 380,
                Height = 200,
                Title = "Thanh cong",
                Content = panel
            };
            dialog.Show();
        });
    }

    private void ShowInfoDialog(string title, string message)
    {
        Dispatcher.UIThread.Post(() =>
        {
            var panel = new StackPanel { Margin = new Avalonia.Thickness(24), Spacing = 10 };
            panel.Children.Add(new TextBlock { Text = title, FontSize = 16, FontWeight = Avalonia.Media.FontWeight.Bold, Foreground = Avalonia.Media.Brushes.SeaGreen });
            panel.Children.Add(new TextBlock { Text = message, TextWrapping = Avalonia.Media.TextWrapping.Wrap });

            var dialog = new Window
            {
                Width = 380,
                Height = 180,
                Title = "Thong bao",
                Content = panel
            };
            dialog.Show();
        });
    }

    private async void OnRefreshSanPham(object? sender, RoutedEventArgs e)
    {
        TxtLoadingSanPham.Text = "Dang tai...";
        try
        {
            await TaiSanPhamAsync();
            TxtLoadingSanPham.Text = "";
            SetStatus($"Da tai {_dsSanPham.Count} san pham.");
        }
        catch (Exception ex)
        {
            TxtLoadingSanPham.Text = "";
            ShowInlineError("Loi khi lam moi: " + ex.Message);
        }
    }

    // Bam nhanh nut "+ Gio" ngay tren card san pham (mac dinh so luong = 1)
    private void OnThemNhanhVaoGio(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is SanPham sp)
        {
            if (!sp.ConHang) return;

            // Neu san pham da co trong gio hang roi thi cong don so luong, khong tao dong moi
            var existing = _gioHang.FirstOrDefault(x => x.MaSP == sp.MaSP);
            if (existing != null)
            {
                existing.SoLuong += 1;
                // Bat ItemsControl cap nhat lai vi ObservableCollection khong tu bat thay doi property ben trong item
                var index = _gioHang.IndexOf(existing);
                _gioHang.RemoveAt(index);
                _gioHang.Insert(index, existing);
            }
            else
            {
                _gioHang.Add(new ChiTietHoaDonItem { MaSP = sp.MaSP, TenSP = sp.TenSP, SoLuong = 1, DonGia = sp.GiaBan });
            }

            CapNhatTongTien();
            SetStatus($"Da them '{sp.TenSP}' vao gio hang.");
        }
    }

    private void OnThemVaoGio(object? sender, RoutedEventArgs e)
    {
        if (CboSanPham.SelectedItem is not SanPham sp)
        {
            ShowInlineError("Vui long chon san pham.");
            return;
        }

        int soLuong = (int)(NumSoLuong.Value ?? 1);

        if (soLuong > sp.SoLuongTon)
        {
            ShowInlineError($"San pham '{sp.TenSP}' chi con {sp.SoLuongTon}, khong du de them {soLuong}.");
            return;
        }

        var existing = _gioHang.FirstOrDefault(x => x.MaSP == sp.MaSP);
        if (existing != null)
        {
            existing.SoLuong += soLuong;
            var index = _gioHang.IndexOf(existing);
            _gioHang.RemoveAt(index);
            _gioHang.Insert(index, existing);
        }
        else
        {
            _gioHang.Add(new ChiTietHoaDonItem { MaSP = sp.MaSP, TenSP = sp.TenSP, SoLuong = soLuong, DonGia = sp.GiaBan });
        }

        CapNhatTongTien();
        SetStatus($"Da them '{sp.TenSP}' x{soLuong} vao gio hang.");
    }

    private void CapNhatTongTien()
    {
        decimal tong = 0;
        foreach (var item in _gioHang) tong += item.ThanhTien;
        TxtTongTien.Text = $"Tam tinh: {tong:N0} đ";
    }

    private async void OnLuuHoaDon(object? sender, RoutedEventArgs e)
    {
        if (_gioHang.Count == 0)
        {
            ShowInlineError("Gio hang dang trong, chua co san pham nao.");
            return;
        }

        try
        {
            int? maKH = (CboKhachHang.SelectedItem as KhachHang)?.MaKH;
            string hinhThucTT = (CboHinhThucTT.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Tien mat";
            const int maNVDangNhap = 1;

            decimal tongTienGioHang = 0;
            foreach (var i in _gioHang) tongTienGioHang += i.ThanhTien;

            var gioHangSnapshot = new List<ChiTietHoaDonItem>(_gioHang);
            int soHD = await Task.Run(() => _db.LapHoaDon(maKH, maNVDangNhap, hinhThucTT, gioHangSnapshot));

            ShowSuccessDialog(soHD, tongTienGioHang);
            SetStatus($"Lap hoa don HD{soHD:D5} thanh cong!");

            _gioHang.Clear();
            CapNhatTongTien();
            await TaiSanPhamAsync();
            await TaiDonHangAsync();
        }
        catch (Exception ex)
        {
            ShowInlineError("Loi khi luu hoa don: " + ex.Message);
        }
    }

    private async void OnRefreshDonHang(object? sender, RoutedEventArgs e)
    {
        try
        {
            await TaiDonHangAsync();
        }
        catch (Exception ex)
        {
            ShowInlineError("Loi khi tai don hang: " + ex.Message);
        }
    }

    private async void OnNhapHang(object? sender, RoutedEventArgs e)
    {
        if (CboSanPhamNhap.SelectedItem is not SanPham sp)
        {
            ShowInlineError("Vui long chon san pham can nhap hang.");
            return;
        }

        int soLuong = (int)(NumSoLuongNhap.Value ?? 1);
        if (soLuong <= 0)
        {
            ShowInlineError("So luong nhap phai lon hon 0.");
            return;
        }

        try
        {
            await Task.Run(() => _db.NhapThemHang(sp.MaSP, soLuong));

            await TaiSanPhamAsync();

            SetStatus($"Da nhap them {soLuong} '{sp.TenSP}'. Ton kho da cap nhat.");
            ShowInfoDialog("📥 Nhap hang thanh cong!", $"Da nhap them {soLuong} '{sp.TenSP}' vao kho.");
        }
        catch (Exception ex)
        {
            ShowInlineError("Loi khi nhap hang: " + ex.Message);
        }
    }
}