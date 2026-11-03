USE [VIETTHAI]
GO
/****** Object:  StoredProcedure [dbo].[List_DanhMucKhachHangForwarder]    Script Date: 04-Sep-26 4:18:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[List_DanhMucKhachHangForwarder]
	@ID bigint = null
as
begin
	set nocount on;

	begin
		select	a.ID, 
				a.MaKH,
				a.MatKhau,
				a.TenKH,
				a.Email,
				a.SoDienThoai,
				a.LoaiTaiKhoan,
				a.CreateDate, 
				a.EditDate 
		from DanhMucKhachHangForwarder a 
		where  @ID IS NULL or a.ID = @ID
		order by a.MaKH;
	end
end
