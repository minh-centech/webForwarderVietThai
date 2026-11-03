create procedure [dbo].[Delete_DanhMucKhachHangForwarder]
	@ID			bigint
as
begin
	set nocount on;
	
	declare @ErrMsg nvarchar(max), @countID int;

	begin tran
	begin try
		delete DanhMucKhachHangForwarder	where ID = @ID;
	commit tran
	end try
	begin catch
		if @@TRANCOUNT > 0 rollback tran;
		select @ErrMsg = ERROR_MESSAGE();
		raiserror(@ErrMsg, 16, 1);
	end catch
end
