ALTER PROCEDURE [dbo].[List_DanhMucKhachHangForwarder_Login]
    @MaKH NVARCHAR(128)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ID,
        MaKH,
        MatKhau,
        TenKH,
        Email,
        SoDienThoai,
        LoaiTaiKhoan,
        IDDanhMucDaiLy,
        CreateDate,
        EditDate
    FROM DanhMucKhachHangForwarder
    WHERE LTRIM(RTRIM(MaKH)) = LTRIM(RTRIM(@MaKH));
END;