namespace SalesDemo.Models;

public class DanhMuc
{
    public int MaDanhMuc { get; set; }
    public string TenDanhMuc { get; set; } = "";
    public override string ToString() => TenDanhMuc;
}