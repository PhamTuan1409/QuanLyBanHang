using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using SalesDemo.Models;

namespace SalesDemo.Services;

public class DatabaseHelper
{
    private const string ConnectionString =
        "Server=localhost,14330;Database=QuanLyBanHang;User Id=sa;Password=YourPass123!;TrustServerCertificate=True;";

    private SqlConnection GetConnection() => new SqlConnection(ConnectionString);

    // ---------------- SAN_PHAM ----------------

    public List<SanPham> LayDanhSachSanPham(string tuKhoa = "", int maDanhMuc = 0)
    {
        var list = new List<SanPham>();

        using var conn = GetConnection();
        conn.Open();

        const string sql = @"
            SELECT MaSP, TenSP, GiaBan, SoLuongTon, DonViTinh, MaDanhMuc
            FROM SAN_PHAM
            WHERE (@TuKhoa = '' OR TenSP LIKE '%' + @TuKhoa + '%')
              AND (@MaDanhMuc = 0 OR MaDanhMuc = @MaDanhMuc)
            ORDER BY TenSP";

        using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@TuKhoa", tuKhoa ?? "");
        cmd.Parameters.AddWithValue("@MaDanhMuc", maDanhMuc);

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new SanPham
            {
                MaSP = reader.GetInt32(0),
                TenSP = reader.GetString(1),
                GiaBan = reader.GetDecimal(2),
                SoLuongTon = reader.GetInt32(3),
                DonViTinh = reader.IsDBNull(4) ? "" : reader.GetString(4),
                MaDanhMuc = reader.GetInt32(5)
            });
        }

        return list;
    }

    public SanPham? LaySanPhamTheoMa(int maSP)
    {
        using var conn = GetConnection();
        conn.Open();

        const string sql = @"
            SELECT
                MaSP,
                TenSP,
                GiaNhap,
                GiaBan,
                SoLuongTon,
                DonViTinh,
                MaDanhMuc
            FROM SAN_PHAM
            WHERE MaSP = @MaSP";

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MaSP", maSP);

        using var reader = cmd.ExecuteReader();

        if (!reader.Read())
            return null;

        return new SanPham
        {
            MaSP = reader.GetInt32(0),
            TenSP = reader.GetString(1),
            GiaNhap = reader.GetDecimal(2),
            GiaBan = reader.GetDecimal(3),
            SoLuongTon = reader.GetInt32(4),
            DonViTinh = reader.IsDBNull(5) ? "" : reader.GetString(5),
            MaDanhMuc = reader.GetInt32(6)
        };
    }

    public void NhapThemHang(int maSP, int soLuong)
    {
        if (soLuong <= 0)
            throw new ArgumentException("Số lượng nhập phải lớn hơn 0.");

        using var conn = GetConnection();
        conn.Open();

        const string sql = @"
            UPDATE SAN_PHAM
            SET SoLuongTon = SoLuongTon + @SoLuong
            WHERE MaSP = @MaSP";

        using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@MaSP", maSP);
        cmd.Parameters.AddWithValue("@SoLuong", soLuong);

        int rows = cmd.ExecuteNonQuery();

        if (rows == 0)
            throw new Exception("Không tìm thấy sản phẩm.");
    }

    // ---------------- DANH_MUC_SAN_PHAM ----------------

    public List<DanhMuc> LayDanhSachDanhMuc()
    {
        var list = new List<DanhMuc>();

        using var conn = GetConnection();
        conn.Open();

        const string sql = @"
            SELECT MaDanhMuc, TenDanhMuc
            FROM DANH_MUC_SAN_PHAM
            ORDER BY TenDanhMuc";

        using var cmd = new SqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new DanhMuc
            {
                MaDanhMuc = reader.GetInt32(0),
                TenDanhMuc = reader.GetString(1)
            });
        }

        return list;
    }

    // ---------------- KHACH HANG ----------------

    public List<KhachHang> LayDanhSachKhachHang()
    {
        var list = new List<KhachHang>();

        using var conn = GetConnection();
        conn.Open();

        const string sql = @"
            SELECT MaKH, HoTen, SoDienThoai, Email, DiaChi, HangThanhVien, NgayDangKy
            FROM KHACH_HANG
            ORDER BY HoTen";

        using var cmd = new SqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new KhachHang
            {
                MaKH = reader.GetInt32(0),
                HoTen = reader.IsDBNull(1) ? "" : reader.GetString(1),
                SoDienThoai = reader.IsDBNull(2) ? "" : reader.GetString(2),
                Email = reader.IsDBNull(3) ? "" : reader.GetString(3),
                DiaChi = reader.IsDBNull(4) ? "" : reader.GetString(4),
                HangThanhVien = reader.IsDBNull(5) ? "Bac" : reader.GetString(5),
                NgayDangKy = reader.IsDBNull(6) ? DateTime.Now : reader.GetDateTime(6)
            });
        }

        return list;
    }

    public int ThemKhachHang(string hoTen, string soDienThoai)
    {
        using var conn = GetConnection();
        conn.Open();

        const string sql = @"
            INSERT INTO KHACH_HANG
            (
                HoTen,
                SoDienThoai,
                HangThanhVien,
                NgayDangKy
            )
            OUTPUT INSERTED.MaKH
            VALUES
            (
                @HoTen,
                @SoDienThoai,
                N'Bac',
                GETDATE()
            )";

        using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@HoTen", hoTen);
        cmd.Parameters.AddWithValue("@SoDienThoai", soDienThoai);

        return (int)cmd.ExecuteScalar();
    }

    // ---------------- CHIET KHAU ----------------

    public decimal LayMucChietKhau(string hangThanhVien) => hangThanhVien switch
    {
        "Vang" => 0.05m,
        "Kim cuong" => 0.10m,
        _ => 0m
    };

    // ---------------- DON HANG ----------------

    public List<DonHang> LayDanhSachDonHang()
    {
        var list = new List<DonHang>();

        using var conn = GetConnection();
        conn.Open();

        const string sql = @"
            SELECT SoHD, NgayLap, TongTien, TrangThai, HinhThucThanhToan
            FROM HOA_DON
            ORDER BY SoHD DESC";

        using var cmd = new SqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new DonHang
            {
                SoHD = reader.GetInt32(0),
                NgayLap = reader.GetDateTime(1),
                TongTien = reader.GetDecimal(2),
                TrangThai = reader.IsDBNull(3) ? "" : reader.GetString(3),
                HinhThucThanhToan = reader.IsDBNull(4) ? "" : reader.GetString(4)
            });
        }

        return list;
    }

    // ---------------- LAP HOA DON ----------------

    public int LapHoaDon(
        int? maKH,
        int maNV,
        string hinhThucThanhToan,
        List<ChiTietHoaDonItem> gioHang)
    {
        using var conn = GetConnection();
        conn.Open();

        using var transaction = conn.BeginTransaction();

        try
        {
            // Kiểm tra tồn kho
            foreach (var item in gioHang)
            {
                const string checkSql = @"
                    SELECT SoLuongTon
                    FROM SAN_PHAM
                    WHERE MaSP = @MaSP";

                using var checkCmd = new SqlCommand(checkSql, conn, transaction);
                checkCmd.Parameters.AddWithValue("@MaSP", item.MaSP);

                var tonKho = (int)checkCmd.ExecuteScalar();

                if (tonKho < item.SoLuong)
                {
                    throw new InvalidOperationException(
                        $"San pham '{item.TenSP}' chi con {tonKho}, khong du de ban {item.SoLuong}.");
                }
            }

            // Tính tổng tiền
            decimal tongTienGoc = 0;

            foreach (var item in gioHang)
            {
                tongTienGoc += item.ThanhTien;
            }

            // Tính chiết khấu khách hàng
            decimal chietKhau = 0;

            if (maKH.HasValue)
            {
                const string hangSql = @"
                    SELECT HangThanhVien
                    FROM KHACH_HANG
                    WHERE MaKH = @MaKH";

                using var hangCmd = new SqlCommand(hangSql, conn, transaction);
                hangCmd.Parameters.AddWithValue("@MaKH", maKH.Value);

                var hang = (string)hangCmd.ExecuteScalar();
                chietKhau = LayMucChietKhau(hang);
            }

            decimal tongTienSauGiam = tongTienGoc * (1 - chietKhau);

            // Tạo hóa đơn
            const string insertHD = @"
                INSERT INTO HOA_DON
                (
                    HinhThucThanhToan,
                    TongTien,
                    TrangThai,
                    MaKH,
                    MaNV
                )
                OUTPUT INSERTED.SoHD
                VALUES
                (
                    @HTTT,
                    @TongTien,
                    N'DaThanhToan',
                    @MaKH,
                    @MaNV
                )";

            using var insertHDCmd = new SqlCommand(insertHD, conn, transaction);

            insertHDCmd.Parameters.AddWithValue("@HTTT", hinhThucThanhToan);
            insertHDCmd.Parameters.AddWithValue("@TongTien", tongTienSauGiam);
            insertHDCmd.Parameters.AddWithValue("@MaKH", (object?)maKH ?? DBNull.Value);
            insertHDCmd.Parameters.AddWithValue("@MaNV", maNV);

            int soHD = (int)insertHDCmd.ExecuteScalar();

            // Thêm chi tiết hóa đơn
            foreach (var item in gioHang)
            {
                const string insertCT = @"
                    INSERT INTO CHI_TIET_HOA_DON
                    (
                        SoHD,
                        MaSP,
                        SoLuong,
                        DonGia
                    )
                    VALUES
                    (
                        @SoHD,
                        @MaSP,
                        @SoLuong,
                        @DonGia
                    )";

                using var insertCTCmd = new SqlCommand(insertCT, conn, transaction);

                insertCTCmd.Parameters.AddWithValue("@SoHD", soHD);
                insertCTCmd.Parameters.AddWithValue("@MaSP", item.MaSP);
                insertCTCmd.Parameters.AddWithValue("@SoLuong", item.SoLuong);
                insertCTCmd.Parameters.AddWithValue("@DonGia", item.DonGia);

                insertCTCmd.ExecuteNonQuery();

                // Trừ tồn kho
                const string updateSP = @"
                    UPDATE SAN_PHAM
                    SET SoLuongTon = SoLuongTon - @SoLuong
                    WHERE MaSP = @MaSP";

                using var updateSPCmd = new SqlCommand(updateSP, conn, transaction);

                updateSPCmd.Parameters.AddWithValue("@SoLuong", item.SoLuong);
                updateSPCmd.Parameters.AddWithValue("@MaSP", item.MaSP);

                updateSPCmd.ExecuteNonQuery();
            }

            transaction.Commit();
            return soHD;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}