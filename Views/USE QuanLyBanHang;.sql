USE QuanLyBanHang;

DELETE FROM CHI_TIET_HOA_DON;
DELETE FROM CHI_TIET_PHIEU_NHAP;
DELETE FROM HOA_DON;
DELETE FROM PHIEU_NHAP_HANG;
DELETE FROM SAN_PHAM;
DELETE FROM DANH_MUC_SAN_PHAM;
DBCC CHECKIDENT ('SAN_PHAM', RESEED, 0);
DBCC CHECKIDENT ('DANH_MUC_SAN_PHAM', RESEED, 0);
DBCC CHECKIDENT ('HOA_DON', RESEED, 0);

INSERT INTO DANH_MUC_SAN_PHAM (TenDanhMuc) VALUES
(N'Đồ gia dụng'), (N'Bình nước & Ly'), (N'Văn phòng phẩm'), (N'Điện tử');

INSERT INTO SAN_PHAM (TenSP, GiaNhap, GiaBan, SoLuongTon, DonViTinh, MaDanhMuc) VALUES
(N'Gối ngủ Cotton', 55000, 89000, 30, N'cái', 1),
(N'Gối cổ du lịch', 40000, 65000, 4, N'cái', 1),
(N'Chăn mỏng mùa hè', 95000, 159000, 12, N'cái', 1),
(N'Khăn mặt cotton', 22000, 39000, 50, N'cái', 1),
(N'Nến thơm', 45000, 79000, 6, N'hũ', 1),
(N'Ly giữ nhiệt 500ml', 75000, 129000, 0, N'cái', 2),
(N'Bình nước thể thao', 58000, 99000, 25, N'cái', 2),
(N'Cốc sứ', 28000, 49000, 0, N'cái', 2),
(N'Hộp cơm 2 tầng', 70000, 119000, 8, N'cái', 2),
(N'Sổ tay', 18000, 35000, 40, N'cuốn', 3),
(N'Bút gel', 7000, 15000, 100, N'cây', 3),
(N'Túi tote canvas', 32000, 59000, 18, N'cái', 3),
(N'Đèn ngủ mini', 85000, 145000, 3, N'cái', 4),
(N'Tai nghe Bluetooth', 180000, 299000, 10, N'cái', 4),
(N'Giá đỡ điện thoại', 38000, 69000, 2, N'cái', 4);