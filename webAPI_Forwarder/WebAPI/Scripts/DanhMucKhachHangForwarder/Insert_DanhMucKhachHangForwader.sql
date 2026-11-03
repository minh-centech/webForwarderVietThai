ALTER PROCEDURE [dbo].[Insert_DanhMucKhachHangForwarder]
    @ID BIGINT OUT,
    @MaKH NVARCHAR(50) = NULL,
    @MatKhau NVARCHAR(MAX) = NULL,
    @TenKH NVARCHAR(150) = NULL,
    @Email NVARCHAR(255) = NULL,
    @SoDienThoai NVARCHAR(20) = NULL,
    @LoaiTaiKhoan INT = 2,
    @IDDanhMucDaiLy BIGINT = NULL,
    @CreateDate DATETIME OUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ErrMsg NVARCHAR(MAX);

    SET @MaKH = dbo.ChuanHoaChuoi(@MaKH);
    SET @TenKH = dbo.ChuanHoaChuoi(@TenKH);
    SET @Email = dbo.ChuanHoaChuoi(@Email);

    IF @MaKH IS NULL OR @MaKH = ''
    BEGIN
        RAISERROR(N'Mã khách hàng không được bỏ trống!', 16, 1);
        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM DanhMucKhachHangForwarder
        WHERE MaKH = @MaKH
    )
    BEGIN
        SET @ErrMsg = N'Mã khách hàng ' + @MaKH + N' đã tồn tại!';
        RAISERROR(@ErrMsg, 16, 1);
        RETURN;
    END;

    IF @TenKH IS NULL OR @TenKH = ''
    BEGIN
        RAISERROR(N'Tên khách hàng không được bỏ trống!', 16, 1);
        RETURN;
    END;

    IF @Email IS NULL OR @Email = ''
    BEGIN
        RAISERROR(N'Email không được bỏ trống!', 16, 1);
        RETURN;
    END;

    IF @SoDienThoai IS NULL OR @SoDienThoai = ''
    BEGIN
        RAISERROR(N'Số điện thoại không được bỏ trống!', 16, 1);
        RETURN;
    END;

    IF @LoaiTaiKhoan = 1
    BEGIN
        SET @IDDanhMucDaiLy = NULL;
    END

    ELSE IF @LoaiTaiKhoan = 2
    BEGIN
        IF @IDDanhMucDaiLy IS NULL
        BEGIN
            RAISERROR(N'Tài khoản khách hàng phải có IDDanhMucDaiLy!', 16, 1);
            RETURN;
        END;

        IF NOT EXISTS
        (
            SELECT 1
            FROM DanhMucDaiLy
            WHERE ID = @IDDanhMucDaiLy
        )
        BEGIN
            RAISERROR(N'Đại lý không tồn tại!', 16, 1);
            RETURN;
        END;

        IF EXISTS
        (
            SELECT 1
            FROM DanhMucKhachHangForwarder
            WHERE IDDanhMucDaiLy = @IDDanhMucDaiLy
        )
        BEGIN
            RAISERROR(N'Đại lý này đã có tài khoản đăng nhập!', 16, 1);
            RETURN;
        END;
    END

    ELSE
    BEGIN
        RAISERROR(N'Loại tài khoản không hợp lệ!', 16, 1);
        RETURN;
    END;

    BEGIN TRAN;

    BEGIN TRY
        EXEC Insert_AutoID
            @ID OUT,
            @TenBangDuLieu = N'DanhMucKhachHangForwarder';
        SET @CreateDate = GETDATE();

        INSERT INTO DanhMucKhachHangForwarder
        (
            MaKH,
            MatKhau,
            TenKH,
            Email,
            SoDienThoai,
            LoaiTaiKhoan,
            IDDanhMucDaiLy,
            CreateDate
        )
        VALUES
        (
            @MaKH,
            @MatKhau,
            @TenKH,
            @Email,
            @SoDienThoai,
            @LoaiTaiKhoan,
            @IDDanhMucDaiLy,
            @CreateDate
        );

        SET @ID = SCOPE_IDENTITY();

        COMMIT TRAN;

    END TRY
    BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRAN;

        SET @ErrMsg = ERROR_MESSAGE();

        RAISERROR(@ErrMsg, 16, 1);

    END CATCH
END