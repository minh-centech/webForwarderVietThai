using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using webForwarder_VIETTHAI.Models;

namespace webForwarder_VIETTHAI.Services
{
    public static class ExcelExporter
    {
        private sealed record Col<T>(string Header, Func<T, object?> Value, string? Format = null, bool Sum = false);

        private const string Num0 = "#,##0";
        private const string Num3 = "#,##0.000";
        private const string DateTimeFmt = "dd/MM/yyyy HH:mm";

        public static byte[] XacNhanNhapKho(IReadOnlyList<XacNhanNhapKhoRow> rows, DateTime tu, DateTime den)
            => Build<XacNhanNhapKhoRow>("Nhap kho", "XÁC NHẬN HOÀN THÀNH NHẬP KHO HÀNG XUẤT KHẨU", tu, den, rows,
            [
                new("Số", r => r.So),
            new("Ngày lập", r => r.NgayLap, DateTimeFmt),
            new("Xác nhận hoàn thành", r => r.NgayGioXacNhanHoanThanh, DateTimeFmt),
            new("Số lệnh nhập kho", r => r.SoLenhNhapKhoHangXuatKhau),
            new("Booking", r => r.Booking),
            new("Số xe", r => r.SoXe),
            new("Mã đại lý", r => r.MaDanhMucDaiLy),
            new("SL đăng ký", r => r.SoLuongDangKyNhap, Num0, Sum: true),
            new("SL nhập", r => r.SoLuongNhap, Num0, Sum: true),
            new("KL đăng ký (kg)", r => r.KhoiLuongDangKyNhap, Num0, Sum: true),
            new("KL nhập (kg)", r => r.KhoiLuongNhap, Num0, Sum: true),
            new("CBM đăng ký", r => r.CBMDangKyNhap, Num3, Sum: true),
            new("CBM nhập", r => r.CBMNhap, Num3, Sum: true),
            new("Pallet", r => r.SoLuongPallet, Num0, Sum: true),
            ]);

        public static byte[] XacNhanDongHang(IReadOnlyList<XacNhanDongHangRow> rows, DateTime tu, DateTime den)
            => Build<XacNhanDongHangRow>("Dong hang", "XÁC NHẬN HOÀN THÀNH ĐÓNG HÀNG XUẤT KHẨU", tu, den, rows,
            [
                new("Số", r => r.So),
            new("Ngày lập", r => r.NgayLap, DateTimeFmt),
            new("Bắt đầu đóng", r => r.NgayGioBatDauDongHang, DateTimeFmt),
            new("Kết thúc đóng", r => r.NgayGioKetThucDongHang, DateTimeFmt),
            new("Xác nhận hoàn thành", r => r.NgayGioXacNhanHoanThanh, DateTimeFmt),
            new("Booking", r => r.Booking),
            new("Carrier booking", r => r.CarrierBooking),
            new("Số container", r => r.SoContainer),
            new("Loại cont", r => r.MaDanhMucLoaiContainer),
            new("Hãng tàu", r => r.TenDanhMucHangTau),
            new("Cảng đi", r => r.MaDanhMucCangDi),
            new("Cửa làm hàng", r => r.MaDanhMucCuaLamHang),
            new("Mã đại lý", r => r.MaDanhMucDaiLy),
            new("SL đăng ký", r => r.SoLuongDangKyDongHang, Num0, Sum: true),
            new("SL đóng", r => r.SoLuongDongHang, Num0, Sum: true),
            new("KL đăng ký (kg)", r => r.KhoiLuongDangKyDongHang, Num0, Sum: true),
            new("KL đóng (kg)", r => r.KhoiLuongDongHang, Num0, Sum: true),
            new("CBM đăng ký", r => r.CBMDangKyDongHang, Num3, Sum: true),
            new("CBM đóng", r => r.CBMDongHang, Num3, Sum: true),
            new("Ghi chú", r => r.GhiChu),
            ]);
        public static byte[] XacNhanXuatKho(IReadOnlyList<XacNhanXuatKhoRow> rows, DateTime tu, DateTime den)
            => Build<XacNhanXuatKhoRow>("Xuat kho", "XÁC NHẬN HOÀN THÀNH XUẤT KHO HÀNG NHẬP KHẨU", tu, den, rows,
            [
                new("Số", r => r.So),
                new("Ngày lập", r => r.NgayLap, DateTimeFmt),
                new("Xác nhận xuất kho", r => r.NgayGioXacNhanXuatKho, DateTimeFmt),
                new("Số vận đơn", r => r.SoVanDon),
                new("Chủ hàng", r => r.TenChuHang),
                new("Mô tả hàng hoá", r => r.MoTaHangHoa),
                new("Số container", r => r.SoContainer),
                new("Số seal", r => r.SoSeal),
                new("Loại cont", r => r.MaDanhMucLoaiContainer),
                new("Tên tàu", r => r.TenTau),
                new("Số chuyến", r => r.SoChuyen),
                new("Ngày tàu đến", r => r.NgayTauDen, "dd/MM/yyyy"),
                new("Hãng tàu", r => r.MaDanhMucHangTau),
                new("Mã đại lý", r => r.MaDanhMucDaiLy),
                new("SL xuất", r => r.SoLuongKienXuat, Num0, Sum: true),
                new("ĐVT", r => r.MaDanhMucDonViTinh),
                new("KL xuất (kg)", r => r.KhoiLuongXuat, Num0, Sum: true),
                new("CBM xuất", r => r.CBMXuat, Num3, Sum: true),
                new("Ghi chú", r => r.GhiChu),
            ]);

        private static byte[] Build<T>(string sheetName, string title, DateTime tu, DateTime den,
            IReadOnlyList<T> rows, IReadOnlyList<Col<T>> cols)
        {
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add(sheetName);
            var lastCol = cols.Count + 1;   // + cột STT

            // Tiêu đề
            ws.Cell(1, 1).Value = title;
            ws.Range(1, 1, 1, lastCol).Merge().Style
                .Font.SetBold().Font.SetFontSize(14)
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Cell(2, 1).Value = $"Từ ngày {tu:dd/MM/yyyy} đến ngày {den:dd/MM/yyyy} – xuất lúc {DateTime.Now:HH:mm dd/MM/yyyy}";
            ws.Range(2, 1, 2, lastCol).Merge().Style
                .Font.SetItalic()
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            // Dòng tiêu đề cột
            const int headerRow = 4;
            ws.Cell(headerRow, 1).Value = "STT";
            for (var c = 0; c < cols.Count; c++)
            {
                ws.Cell(headerRow, c + 2).Value = cols[c].Header;
            }

            ws.Range(headerRow, 1, headerRow, lastCol).Style
                .Font.SetBold()
                .Fill.SetBackgroundColor(XLColor.FromHtml("#E7EFFF"))
                .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                .Alignment.SetVertical(XLAlignmentVerticalValues.Center)
                .Alignment.SetWrapText(true);

            // Dữ liệu
            var r0 = headerRow + 1;
            for (var i = 0; i < rows.Count; i++)
            {
                var row = r0 + i;
                ws.Cell(row, 1).Value = (double)(i + 1);
                for (var c = 0; c < cols.Count; c++)
                {
                    SetCell(ws.Cell(row, c + 2), cols[c].Value(rows[i]), cols[c].Format);
                }
            }

            // Dòng tổng
            var totalRow = r0 + rows.Count;
            ws.Cell(totalRow, 1).Value = $"Tổng {rows.Count} phiếu";
            ws.Range(totalRow, 1, totalRow, 2).Merge();
            for (var c = 0; c < cols.Count; c++)
            {
                if (!cols[c].Sum || rows.Count == 0) continue;
                var colLetter = ws.Cell(r0, c + 2).Address.ColumnLetter;
                var cell = ws.Cell(totalRow, c + 2);
                cell.FormulaA1 = $"SUM({colLetter}{r0}:{colLetter}{totalRow - 1})";
                cell.Style.NumberFormat.Format = cols[c].Format ?? Num0;
            }
            ws.Range(totalRow, 1, totalRow, lastCol).Style
                .Font.SetBold()
                .Fill.SetBackgroundColor(XLColor.FromHtml("#FAFCFF"));

            // Kẻ khung, cố định dòng tiêu đề, độ rộng cột
            ws.Range(headerRow, 1, totalRow, lastCol).Style
                .Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                .Border.SetInsideBorder(XLBorderStyleValues.Thin);
            ws.SheetView.FreezeRows(headerRow);
            ws.Columns().AdjustToContents();
            ws.Column(1).Width = 8;

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }

        private static void SetCell(IXLCell cell, object? value, string? format)
        {
            switch (value)
            {
                case null:
                    cell.Clear();
                    break;
                case DateTime dt:
                    cell.Value = dt;
                    cell.Style.NumberFormat.Format = format ?? DateTimeFmt;
                    break;
                case decimal d:
                    cell.Value = (double)d;
                    cell.Style.NumberFormat.Format = format ?? Num0;
                    break;
                case int or long or double or float:
                    cell.Value = Convert.ToDouble(value);
                    cell.Style.NumberFormat.Format = format ?? Num0;
                    break;
                default:
                    cell.Value = value.ToString();
                    break;
            }
        }
    }
}
