CREATE DATABASE SQLQuanLyHangTonKho
GO
USE SQLQuanLyHangTonKho
Go
CREATE TABLE MenuItem
(
	Id INT PRIMARY KEY NOT NULL,
	Name NVARCHAR(128),
	Form VARCHAR(32),
	See VARCHAR(256),
	New VARCHAR(256),
	Edit VARCHAR(256),
	Del VARCHAR(256),
	Excel VARCHAR(256),
	Copy VARCHAR(256)
)
GO
INSERT INTO MenuItem(Id,Name,Form) VALUES(1, N'Danh mục tài khoản','frmUsers')
INSERT INTO MenuItem(Id,Name,Form) VALUES(2,	N'Danh mục nhân viên','frmEmployees')
INSERT INTO MenuItem(Id,Name,Form) VALUES(3,	N'Danh mục kho','frmWarehouse')
INSERT INTO MenuItem(Id,Name,Form) VALUES(4,	N'Danh mục nhóm hàng hóa','frmProductCategory')
INSERT INTO MenuItem(Id,Name,Form) VALUES(5,	N'Danh mục hàng hóa','frmProducts')
INSERT INTO MenuItem(Id,Name,Form) VALUES(6,	N'Danh mục nhà cung cấp','frmSupplier')
INSERT INTO MenuItem(Id,Name,Form) VALUES(7,	N'Danh mục khách hàng','frmCustomer')
INSERT INTO MenuItem(Id,Name,Form) VALUES(8,	N'Nhập kho','frmImportStockMaster')
INSERT INTO MenuItem(Id,Name,Form) VALUES(9,	N'Xuất kho','frmExportStockMaster')
INSERT INTO MenuItem(Id,Name,Form) VALUES(10,	N'Điều chuyển','frmTransferStockMaster')
INSERT INTO MenuItem(Id,Name,Form) VALUES(11,N'Kiểm kho','frmTakingStockMaster')
INSERT INTO MenuItem(Id,Name,Form) VALUES(12,N'Thống kê và báo cáo','frmReportInventory')
GO
CREATE TABLE Users
(
	Id VARCHAR(32) PRIMARY KEY NOT NULL,
	Name NVARCHAR(128) NOT NULL,
	Password NVARCHAR(256) NOT NULL,
	RePassword NVARCHAR(256) NOT NULL,
	Admin BIT,
	JobPosition NVARCHAR(256),
	Phone VARCHAR(16)
)
GO
INSERT INTO Users(Id,Name,Password,RePassword,Admin) VALUES('ADMIN',N'Nguyễn Vũ Mạnh','123','123',1)
INSERT INTO Users(Id,Name,Password,RePassword,Admin) VALUES('TEST',N'TEST','123','123',0)
GO
CREATE TABLE Employees 
(
    Id VARCHAR(32) PRIMARY KEY NOT NULL,
    Name NVARCHAR(128) NOT NULL,
    DateOfBirth DATETIME,                         
    Gender BIT,      
    Phone VARCHAR(15),                    
    Email VARCHAR(100),    
	PermanentAddress NVARCHAR(256),        
    TemporaryAddress NVARCHAR(256),        
    HireDate DATETIME,
	Position NVARCHAR(128),
	UserCreate NVARCHAR(128),
	DateCreate DATETIME,
	UserUpdate NVARCHAR(128),
	DateUpdate DATETIME
)
GO
CREATE TABLE Warehouse (
	Id VARCHAR(32) PRIMARY KEY NOT NULL,
    Name NVARCHAR(128) NOT NULL,
    Location NVARCHAR(256),                       -- Vị trí kho
	Status BIT,
	UserCreate NVARCHAR(128),
	DateCreate DATETIME,
	UserUpdate NVARCHAR(128),
	DateUpdate DATETIME
);
GO
CREATE TABLE ProductCategory (
    Id VARCHAR(32) PRIMARY KEY NOT NULL,
    Name NVARCHAR(128) NOT NULL,
    Description NVARCHAR(256),                          
);
CREATE TABLE Products (
    Id VARCHAR(32) PRIMARY KEY NOT NULL,
    Name NVARCHAR(128) NOT NULL,
	ProductCategoryId VARCHAR(32) foreign key references ProductCategory(Id) NOT NULL,
	Unit NVARCHAR(64),
	MinimumStock DECIMAL, -- Số lượng tồn tối thiểu
	MaximumStock DECIMAL, -- Số lượng tồn tối đa
    Description NVARCHAR(256),  
	ExpirationDate DATETIME, -- Hạn sử dụng
	Status BIT,
	UserCreate NVARCHAR(128),
	DateCreate DATETIME,
	UserUpdate NVARCHAR(128),
	DateUpdate DATETIME
);
CREATE TABLE Supplier (
    Id VARCHAR(32) PRIMARY KEY NOT NULL,
    Name NVARCHAR(128) NOT NULL,
    ContactName NVARCHAR(100),                        -- Tên người liên hệ
    ContactTitle NVARCHAR(100),                       -- Chức vụ người liên hệ
    Address NVARCHAR(255),                            -- Địa chỉ của nhà cung cấp
    Phone VARCHAR(20),                               -- Số điện thoại
    Email VARCHAR(100),                       -- Email nhà cung cấp
    Website VARCHAR(255),                            -- Website của nhà cung cấp
);
CREATE TABLE Customer (
    Id VARCHAR(32) PRIMARY KEY NOT NULL,
    Name NVARCHAR(128) NOT NULL,
    Address VARCHAR(255),                           -- Địa chỉ khách hàng
    Phone VARCHAR(20),                              -- Số điện thoại khách hàng
    Email VARCHAR(100),                      -- Email khách hàng (duy nhất)
);
CREATE TABLE ImportStockMaster
(
	Id VARCHAR(32) PRIMARY KEY NOT NULL,
	VoucherDate DATETIME,
	WarehouseId VARCHAR(32) foreign key references Warehouse(Id) NOT NULL,
	SupplierId VARCHAR(32) foreign key references Supplier(Id) NOT NULL,
	Note NVARCHAR(256),  
	TotalAmount DECIMAL,
	TotalMoney DECIMAL,
	UserCreate NVARCHAR(128),
	DateCreate DATETIME,
	UserUpdate NVARCHAR(128),
	DateUpdate DATETIME
)
GO
CREATE TABLE ImportStockDetail
(
	Id VARCHAR(64) PRIMARY KEY NOT NULL,
	ImportStockMasterId VARCHAR(32) foreign key references ImportStockMaster(Id) NOT NULL,
	ProductsId VARCHAR(32) foreign key references Products(Id) NOT NULL,
	Amount DECIMAL,
	Price DECIMAL,
	TotalMoney DECIMAL,
	Note NVARCHAR(256) 
)
GO
CREATE TABLE ExportStockMaster
(
	Id VARCHAR(32) PRIMARY KEY NOT NULL,
	VoucherDate DATETIME,
	CustomerId VARCHAR(32) foreign key references Customer(Id) NOT NULL,
	Note NVARCHAR(256),  
	TotalAmount DECIMAL,
	TotalMoney DECIMAL,
	UserCreate NVARCHAR(128),
	DateCreate DATETIME,
	UserUpdate NVARCHAR(128),
	DateUpdate DATETIME
)
GO
CREATE TABLE ExportStockDetail
(
	Id VARCHAR(64) PRIMARY KEY NOT NULL,
	ExportStockMasterId VARCHAR(32) foreign key references ExportStockMaster(Id) NOT NULL,
	ProductsId VARCHAR(32) foreign key references Products(Id) NOT NULL,
	Amount DECIMAL,
	Price DECIMAL,
	TotalMoney DECIMAL,
	Note NVARCHAR(256) 
)
GO
CREATE TABLE TransferStockMaster
(
	Id VARCHAR(32) PRIMARY KEY NOT NULL,
	VoucherDate DATETIME,
	WarehouseIdFrom VARCHAR(32) foreign key references Warehouse(Id) NOT NULL, -- Chuyển từ kho
	WarehouseIdTo VARCHAR(32) foreign key references Warehouse(Id) NOT NULL, -- Chuyển sang kho
	Note NVARCHAR(256),  
	TotalAmount DECIMAL,
	UserCreate NVARCHAR(128),
	DateCreate DATETIME,
	UserUpdate NVARCHAR(128),
	DateUpdate DATETIME
)
GO
CREATE TABLE TransferStockDetail
(
	Id VARCHAR(64) PRIMARY KEY NOT NULL,
	TransferStockMasterId VARCHAR(32) foreign key references TransferStockMaster(Id) NOT NULL,
	ProductsId VARCHAR(32) foreign key references Products(Id) NOT NULL,
	Amount DECIMAL,
	Note NVARCHAR(256) 
)
GO
CREATE TABLE TakingStockMaster
(
	Id VARCHAR(32) PRIMARY KEY NOT NULL,
	VoucherDate DATETIME,
	WarehouseId VARCHAR(32) foreign key references Warehouse(Id) NOT NULL, 
	EmployeesId1 VARCHAR(32) foreign key references Employees(Id) NOT NULL, 
	EmployeesId2 VARCHAR(32) foreign key references Employees(Id) NOT NULL, 
	EmployeesId3 VARCHAR(32) foreign key references Employees(Id) NOT NULL, 
	Note NVARCHAR(256),  
	TotalAmount DECIMAL,
	UserCreate NVARCHAR(128),
	DateCreate DATETIME,
	UserUpdate NVARCHAR(128),
	DateUpdate DATETIME
)
GO
CREATE TABLE TakingStockDetail
(
	Id VARCHAR(64) PRIMARY KEY NOT NULL,
	TakingStockMasterId VARCHAR(32) foreign key references TakingStockMaster(Id) NOT NULL,
	ProductsId VARCHAR(32) foreign key references Products(Id) NOT NULL,
	Amount DECIMAL,
	Note NVARCHAR(256) 
)
GO
CREATE TABLE PostData
(
	VoucherId VARCHAR(32),
	VoucherCode CHAR(3),
	VoucherDate DATETIME,
	ProductsId VARCHAR(32),
	Amount DECIMAL,
	Price DECIMAL,
	TotalMoney DECIMAL,
)
GO
INSERT INTO Customer (Id,Name,Address,Phone,Email)VALUES(N'KH01',N'Cô Thu',N'0',N'',N'')
INSERT INTO Customer (Id,Name,Address,Phone,Email)VALUES(N'KH02',N'Chị Hằng',N'0',N'',N'')
GO
INSERT INTO Employees (Id,Name,DateOfBirth,Gender,Phone,Email,PermanentAddress,TemporaryAddress,HireDate,Position,UserCreate,DateCreate,UserUpdate,DateUpdate)VALUES(N'KYDUYEN',N'Kỳ Duyên','Mar 14 1990 12:00:00:000AM',0,N'0987890989',N'',N'',N'',NULL,N'',N'Nguyễn Vũ Mạnh','Mar 14 2025  9:19:18:667AM',N'Nguyễn Vũ Mạnh','Mar 14 2025  9:19:18:667AM')
INSERT INTO Employees (Id,Name,DateOfBirth,Gender,Phone,Email,PermanentAddress,TemporaryAddress,HireDate,Position,UserCreate,DateCreate,UserUpdate,DateUpdate)VALUES(N'NGOCTRINH',N'Ngọc Trinh','Jan  3 1989 12:00:00:000AM',0,N'0378790987',N'',N'',N'',NULL,N'',N'Nguyễn Vũ Mạnh','Mar 14 2025  9:18:55:710AM',N'Nguyễn Vũ Mạnh','Mar 14 2025  9:18:55:710AM')
GO
INSERT INTO ProductCategory (Id,Name,Description)VALUES(N'DOGIADUNG',N'Đồ gia dụng',N'')
INSERT INTO ProductCategory (Id,Name,Description)VALUES(N'THUCANNHANH',N'Thức ăn nhanh',N'')
GO
INSERT INTO Products (Id,Name,ProductCategoryId,Unit,MinimumStock,MaximumStock,Description,ExpirationDate,Status,UserCreate,DateCreate,UserUpdate,DateUpdate)VALUES(N'QUATTRAN',N'Quạt trần',N'DOGIADUNG',N'Cái',1,100,N'','Mar 14 2030 12:00:00:000AM',1,N'Nguyễn Vũ Mạnh','Mar 14 2025  9:28:26:350AM',N'Nguyễn Vũ Mạnh','Mar 14 2025  9:30:28:390AM')
INSERT INTO Products (Id,Name,ProductCategoryId,Unit,MinimumStock,MaximumStock,Description,ExpirationDate,Status,UserCreate,DateCreate,UserUpdate,DateUpdate)VALUES(N'TULANH',N'Tủ lạnh',N'DOGIADUNG',N'Cái',1,100,N'','Mar 14 2030 12:00:00:000AM',1,N'Nguyễn Vũ Mạnh','Mar 14 2025  9:30:18:387AM',N'Nguyễn Vũ Mạnh','Mar 14 2025  9:30:18:387AM')
GO
INSERT INTO Supplier (Id,Name,ContactName,ContactTitle,Address,Phone,Email,Website)VALUES(N'FPT',N'FPT',N'',N'',N'',N'',N'',N'')
INSERT INTO Supplier (Id,Name,ContactName,ContactTitle,Address,Phone,Email,Website)VALUES(N'TGDD',N'Thế giới di động',N'',N'',N'',N'',N'',N'')
GO
INSERT INTO Warehouse (Id,Name,Location,Status,UserCreate,DateCreate,UserUpdate,DateUpdate)VALUES(N'KHO1',N'Kho 1',N'',1,N'Nguyễn Vũ Mạnh','Mar 14 2025  9:19:49:327AM',N'Nguyễn Vũ Mạnh','Mar 14 2025  9:19:49:327AM')
INSERT INTO Warehouse (Id,Name,Location,Status,UserCreate,DateCreate,UserUpdate,DateUpdate)VALUES(N'KHO2',N'Kho 2',N'',1,N'Nguyễn Vũ Mạnh','Mar 14 2025  9:19:55:813AM',N'Nguyễn Vũ Mạnh','Mar 14 2025  9:19:55:813AM')
INSERT INTO Warehouse (Id,Name,Location,Status,UserCreate,DateCreate,UserUpdate,DateUpdate)VALUES(N'KHO3',N'Kho 3',N'',1,N'Nguyễn Vũ Mạnh','Mar 14 2025  9:20:03:217AM',N'Nguyễn Vũ Mạnh','Mar 14 2025  9:20:03:217AM')

