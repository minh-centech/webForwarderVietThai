CREATE TABLE DanhMucKhachHangForwarder
(
    ID BIGINT IDENTITY(1,1) NOT NULL,
    MaKH NVARCHAR(50) NULL,
    MatKhau NVARCHAR(MAX) NULL,
    TenKH NVARCHAR(150) NULL,
    Email NVARCHAR(255) NULL,
    SoDienThoai NVARCHAR(20) NULL,
    LoaiTaiKhoan INT DEFAULT 2,
    CreateDate DATETIME NULL,
    EditDate DATETIME NULL,

    CONSTRAINT PK_DanhMucKhachHangForwarder
        PRIMARY KEY (ID)
);