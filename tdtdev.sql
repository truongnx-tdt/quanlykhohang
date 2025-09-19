/* =========================================================
   1) BẢNG GIÁ (mua/bán)
   ========================================================= */
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PriceListMaster')
BEGIN
    CREATE TABLE PriceListMaster(
        Id              VARCHAR(32) PRIMARY KEY NOT NULL,
        Name            NVARCHAR(128) NOT NULL,
        -- Type: 'P' = Purchase (mua), 'S' = Sale (bán)
        Type            CHAR(1) NOT NULL CONSTRAINT CK_PriceList_Type CHECK (Type IN ('P','S')),
        StartDate       DATE NULL,
        EndDate         DATE NULL,
        IsActive        BIT NOT NULL DEFAULT(1),
        -- Áp dụng riêng cho 1 NCC/KH (tùy Type) – để NULL nếu áp dụng chung
        SupplierId      VARCHAR(32) NULL FOREIGN KEY REFERENCES Supplier(Id),
        CustomerId      VARCHAR(32) NULL FOREIGN KEY REFERENCES Customer(Id),
        Notes           NVARCHAR(256) NULL,
        UserCreate      NVARCHAR(128) NULL,
        DateCreate      DATETIME NULL DEFAULT(GETDATE()),
        UserUpdate      NVARCHAR(128) NULL,
        DateUpdate      DATETIME NULL
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PriceListDetail')
BEGIN
    CREATE TABLE PriceListDetail(
        Id              VARCHAR(64) PRIMARY KEY NOT NULL,
        PriceListId     VARCHAR(32) NOT NULL FOREIGN KEY REFERENCES PriceListMaster(Id),
        ProductId       VARCHAR(32) NOT NULL FOREIGN KEY REFERENCES Products(Id),
        FromQty         DECIMAL(18,3) NOT NULL DEFAULT(1),
        ToQty           DECIMAL(18,3) NULL,
        UnitPrice       DECIMAL(18,2) NOT NULL,  -- giá theo bảng
        Notes           NVARCHAR(256) NULL
    );
    CREATE INDEX IX_PriceListDetail_PriceList ON PriceListDetail(PriceListId);
    CREATE INDEX IX_PriceListDetail_Product ON PriceListDetail(ProductId);
END
GO

-- 1) Tìm tên FK đang chặn
SELECT fk.name AS FKName
FROM sys.foreign_keys fk
JOIN sys.tables t_parent ON fk.referenced_object_id = t_parent.object_id
JOIN sys.tables t_child  ON fk.parent_object_id     = t_child.object_id
WHERE t_parent.name = 'PriceListMaster'
  AND t_child.name  = 'PriceListDetail';

-- Giả sử kết quả là: FK__PriceList__Price__6FE99F9F
-- 2) Drop FK cũ
ALTER TABLE dbo.PriceListDetail
DROP CONSTRAINT [FK__PriceList__Price__6FE99F9F];

-- 3) Tạo lại FK có ON DELETE CASCADE
ALTER TABLE dbo.PriceListDetail
ADD CONSTRAINT FK_PriceListDetail_PriceListMaster
    FOREIGN KEY (PriceListId)
    REFERENCES dbo.PriceListMaster(Id)
    ON DELETE CASCADE;

/* =========================================================
   2) HÓA ĐƠN MUA / BÁN (không tác động kho trực tiếp)
   ========================================================= */
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PurchaseInvoiceMaster')
BEGIN
    CREATE TABLE PurchaseInvoiceMaster(
        Id              VARCHAR(32) PRIMARY KEY NOT NULL,
        DocNo           NVARCHAR(32) NULL,      -- số chứng từ
        DocDate         DATETIME NOT NULL,
        SupplierId      VARCHAR(32) NOT NULL FOREIGN KEY REFERENCES Supplier(Id),
        WarehouseId     VARCHAR(32) NULL FOREIGN KEY REFERENCES Warehouse(Id), -- kho mặc định (nếu muốn)
        PriceListId     VARCHAR(32) NULL FOREIGN KEY REFERENCES PriceListMaster(Id),
        Status          TINYINT NOT NULL DEFAULT(0), -- 0 Draft, 1 Approved, 2 Posted, 9 Cancelled
        TotalQty        DECIMAL(18,3) NULL,
        TotalMoney      DECIMAL(18,2) NULL,
        Notes           NVARCHAR(256) NULL,
        UserCreate      NVARCHAR(128) NULL,
        DateCreate      DATETIME NULL DEFAULT(GETDATE()),
        UserUpdate      NVARCHAR(128) NULL,
        DateUpdate      DATETIME NULL
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PurchaseInvoiceDetail')
BEGIN
    CREATE TABLE PurchaseInvoiceDetail(
        Id              VARCHAR(64) PRIMARY KEY NOT NULL,
        PIMasterId      VARCHAR(32) NOT NULL FOREIGN KEY REFERENCES PurchaseInvoiceMaster(Id),
        ProductId       VARCHAR(32) NOT NULL FOREIGN KEY REFERENCES Products(Id),
        Qty             DECIMAL(18,3) NOT NULL,
        UnitPrice       DECIMAL(18,2) NOT NULL,
        DiscountRate    DECIMAL(9,4) NOT NULL DEFAULT(0), -- %
        TaxRate         DECIMAL(9,4) NOT NULL DEFAULT(0), -- %
        LineAmount      DECIMAL(18,2) NOT NULL,           -- thành tiền sau chiết khấu/thuế (lưu sẵn)
        Notes           NVARCHAR(256) NULL
    );
    CREATE INDEX IX_PurchaseInvoiceDetail_Master ON PurchaseInvoiceDetail(PIMasterId);
    CREATE INDEX IX_PurchaseInvoiceDetail_Product ON PurchaseInvoiceDetail(ProductId);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'SalesInvoiceMaster')
BEGIN
    CREATE TABLE SalesInvoiceMaster(
        Id              VARCHAR(32) PRIMARY KEY NOT NULL,
        DocNo           NVARCHAR(32) NULL,
        DocDate         DATETIME NOT NULL,
        CustomerId      VARCHAR(32) NOT NULL FOREIGN KEY REFERENCES Customer(Id),
        WarehouseId     VARCHAR(32) NULL FOREIGN KEY REFERENCES Warehouse(Id), -- kho xuất mặc định (nếu dùng)
        PriceListId     VARCHAR(32) NULL FOREIGN KEY REFERENCES PriceListMaster(Id),
        IsRetail        BIT NOT NULL DEFAULT(1),        -- bán lẻ = trừ tồn ngay khi Post
        Status          TINYINT NOT NULL DEFAULT(0),    -- 0 Draft, 1 Approved, 2 Posted, 9 Cancelled
        TotalQty        DECIMAL(18,3) NULL,
        TotalMoney      DECIMAL(18,2) NULL,
        Notes           NVARCHAR(256) NULL,
        UserCreate      NVARCHAR(128) NULL,
        DateCreate      DATETIME NULL DEFAULT(GETDATE()),
        UserUpdate      NVARCHAR(128) NULL,
        DateUpdate      DATETIME NULL
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'SalesInvoiceDetail')
BEGIN
    CREATE TABLE SalesInvoiceDetail(
        Id              VARCHAR(64) PRIMARY KEY NOT NULL,
        SIMasterId      VARCHAR(32) NOT NULL FOREIGN KEY REFERENCES SalesInvoiceMaster(Id),
        ProductId       VARCHAR(32) NOT NULL FOREIGN KEY REFERENCES Products(Id),
        Qty             DECIMAL(18,3) NOT NULL,
        UnitPrice       DECIMAL(18,2) NOT NULL,
        DiscountRate    DECIMAL(9,4) NOT NULL DEFAULT(0),
        TaxRate         DECIMAL(9,4) NOT NULL DEFAULT(0),
        LineAmount      DECIMAL(18,2) NOT NULL,
        Notes           NVARCHAR(256) NULL
    );
    CREATE INDEX IX_SalesInvoiceDetail_Master ON SalesInvoiceDetail(SIMasterId);
    CREATE INDEX IX_SalesInvoiceDetail_Product ON SalesInvoiceDetail(ProductId);
END
GO


/* =========================================================
   3) LIÊN KẾT HÓA ĐƠN <-> PHIẾU NHẬP/XUẤT (để "tạo phiếu từ hóa đơn")
   ========================================================= */
-- Import từ HĐ mua
IF COL_LENGTH('ImportStockMaster', 'PurchaseInvoiceId') IS NULL
    ALTER TABLE ImportStockMaster ADD PurchaseInvoiceId VARCHAR(32) NULL 
        CONSTRAINT FK_ImportStock_PIMaster REFERENCES PurchaseInvoiceMaster(Id);
GO

-- Xuất từ HĐ bán (dành cho bán sỉ)
IF COL_LENGTH('ExportStockMaster', 'SalesInvoiceId') IS NULL
    ALTER TABLE ExportStockMaster ADD SalesInvoiceId VARCHAR(32) NULL 
        CONSTRAINT FK_ExportStock_SIMaster REFERENCES SalesInvoiceMaster(Id);
GO
ALTER TABLE ExportStockDetail ADD SalesInvoiceDetailId VARCHAR(64) NULL;

go 
ALTER TABLE ImportStockDetail ADD PurchaseInvoiceDetailId VARCHAR(64) NULL;
-- (khuyến nghị) tăng tốc các truy vấn kiểm tra:
CREATE NONCLUSTERED INDEX IX_ImportStockDetail_PIDetail
ON ImportStockDetail (PurchaseInvoiceDetailId);

go
-- PXK cần kho
IF COL_LENGTH('dbo.ExportStockMaster','WarehouseId') IS NULL
    ALTER TABLE dbo.ExportStockMaster ADD WarehouseId VARCHAR(64) NULL;

-- PostData cần kho để tổng hợp tồn theo kho
IF COL_LENGTH('dbo.PostData','WarehouseId') IS NULL
    ALTER TABLE dbo.PostData ADD WarehouseId VARCHAR(64) NULL;

-- backfill cho các phiếu đã post trước đây
UPDATE p
SET p.WarehouseId = m.WarehouseId
FROM dbo.PostData p
JOIN dbo.ImportStockMaster m ON p.VoucherCode='PNK' AND p.VoucherId=m.Id
WHERE p.WarehouseId IS NULL;

UPDATE p
SET p.WarehouseId = m.WarehouseId
FROM dbo.PostData p
JOIN dbo.ExportStockMaster m ON p.VoucherCode='PXK' AND p.VoucherId=m.Id
WHERE p.WarehouseId IS NULL;


