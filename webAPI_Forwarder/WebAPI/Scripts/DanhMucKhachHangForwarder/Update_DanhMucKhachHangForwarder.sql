USE [VIETTHAI]
GO
/****** Object:  StoredProcedure [dbo].[Update_DanhMucKhachHangForwarder]    Script Date: 04-Sep-26 4:19:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[Update_DanhMucKhachHangForwarder]
	@ID							bigint,
	@MatKhau					nvarchar(MAX) = null,
	@TenKH						nvarchar(150) = null,
	@Email						nvarchar(255) = null,
	@SoDienThoai				nvarchar(10) = null,
	@EditDate					datetime = null out
as
begin
	set nocount on;
	
	declare @ErrMsg nvarchar(max), @countID int;

	IF NOT EXISTS (
        SELECT 1
        FROM DanhMucKhachHangForwarder
        WHERE ID = @ID
    )
    BEGIN
        RAISERROR(N'Tài khoản không tồn tại!', 16, 1);
        RETURN;
    END;

	set @TenKH = dbo.ChuanHoaChuoi(@TenKH);
	set @Email = dbo.ChuanHoaChuoi(@Email);
	set @SoDienThoai = dbo.ChuanHoaChuoi(@SoDienThoai);

	if @TenKH is null or @TenKH = ''
	begin
		raiserror(N'Tên khách hàng không được bỏ trống!', 16, 1);
		return;
	end;
	if len(@TenKH) > 150
	begin
		raiserror(N'Tên khách hàng không được dài quá 150 ký tự!', 16, 1);
		return;
	end;

	if @Email is null
	begin
		raiserror(N'Email không được bỏ trống!', 16, 1);
		return;
	end;
	if len(@Email) > 255
	begin
		raiserror(N'Email không được dài quá 255 ký tự!', 16, 1);
		return;
	end;
	
	if @SoDienThoai is null
	begin
		raiserror(N'Số điện thoại không được bỏ trống!', 16, 1);
		return;
	end;

	begin tran
	begin try
		set @EditDate = GETDATE();

		update DanhMucKhachHangForwarder set
			MatKhau = @MatKhau,
			TenKH = @TenKH,
			Email = @Email,
			SoDienThoai = @SoDienThoai,
			EditDate = @EditDate
		where ID = @ID;
	commit tran
	end try
	begin catch
		if @@TRANCOUNT > 0 rollback tran;
		select @ErrMsg = ERROR_MESSAGE();
		raiserror(@ErrMsg, 16, 1);
	end catch;
end
