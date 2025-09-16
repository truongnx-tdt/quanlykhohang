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


/* =========================================================
   4) HÀNG TRẢ LẠI (mua/bán)
   ========================================================= */
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PurchaseReturnMaster')
BEGIN
    CREATE TABLE PurchaseReturnMaster(
        Id              VARCHAR(32) PRIMARY KEY NOT NULL,
        DocNo           NVARCHAR(32) NULL,
        DocDate         DATETIME NOT NULL,
        SupplierId      VARCHAR(32) NOT NULL FOREIGN KEY REFERENCES Supplier(Id),
        WarehouseId     VARCHAR(32) NOT NULL FOREIGN KEY REFERENCES Warehouse(Id),
        RefPIMasterId   VARCHAR(32) NULL FOREIGN KEY REFERENCES PurchaseInvoiceMaster(Id),
        Status          TINYINT NOT NULL DEFAULT(0),
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
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PurchaseReturnDetail')
BEGIN
    CREATE TABLE PurchaseReturnDetail(
        Id              VARCHAR(64) PRIMARY KEY NOT NULL,
        PRMasterId      VARCHAR(32) NOT NULL FOREIGN KEY REFERENCES PurchaseReturnMaster(Id),
        ProductId       VARCHAR(32) NOT NULL FOREIGN KEY REFERENCES Products(Id),
        Qty             DECIMAL(18,3) NOT NULL,
        UnitPrice       DECIMAL(18,2) NOT NULL,
        LineAmount      DECIMAL(18,2) NOT NULL,
        Notes           NVARCHAR(256) NULL
    );
    CREATE INDEX IX_PurchaseReturnDetail_Master ON PurchaseReturnDetail(PRMasterId);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'SalesReturnMaster')
BEGIN
    CREATE TABLE SalesReturnMaster(
        Id              VARCHAR(32) PRIMARY KEY NOT NULL,
        DocNo           NVARCHAR(32) NULL,
        DocDate         DATETIME NOT NULL,
        CustomerId      VARCHAR(32) NOT NULL FOREIGN KEY REFERENCES Customer(Id),
        WarehouseId     VARCHAR(32) NOT NULL FOREIGN KEY REFERENCES Warehouse(Id),
        RefSIMasterId   VARCHAR(32) NULL FOREIGN KEY REFERENCES SalesInvoiceMaster(Id),
        Status          TINYINT NOT NULL DEFAULT(0),
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
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'SalesReturnDetail')
BEGIN
    CREATE TABLE SalesReturnDetail(
        Id              VARCHAR(64) PRIMARY KEY NOT NULL,
        SRMasterId      VARCHAR(32) NOT NULL FOREIGN KEY REFERENCES SalesReturnMaster(Id),
        ProductId       VARCHAR(32) NOT NULL FOREIGN KEY REFERENCES Products(Id),
        Qty             DECIMAL(18,3) NOT NULL,
        UnitPrice       DECIMAL(18,2) NOT NULL,
        LineAmount      DECIMAL(18,2) NOT NULL,
        Notes           NVARCHAR(256) NULL
    );
    CREATE INDEX IX_SalesReturnDetail_Master ON SalesReturnDetail(SRMasterId);
END
GO


/* =========================================================
   5) SỔ KHO CHUẨN + SỐ DƯ ĐẦU KỲ + ĐIỀU CHỈNH (phát sinh từ kiểm kê)
   ========================================================= */
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'StockOpeningBalance')
BEGIN
    CREATE TABLE StockOpeningBalance(
        WarehouseId     VARCHAR(32) NOT NULL FOREIGN KEY REFERENCES Warehouse(Id),
        ProductId       VARCHAR(32) NOT NULL FOREIGN KEY REFERENCES Products(Id),
        Period          CHAR(6) NOT NULL,                 -- YYYYMM
        Qty             DECIMAL(18,3) NOT NULL DEFAULT(0),
        UnitCost        DECIMAL(18,2) NOT NULL DEFAULT(0),
        CONSTRAINT PK_StockOpeningBalance PRIMARY KEY (WarehouseId, ProductId, Period)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'StockTransactions')
BEGIN
    CREATE TABLE StockTransactions(
        Id              BIGINT IDENTITY(1,1) PRIMARY KEY,
        TransDate       DATETIME NOT NULL DEFAULT(GETDATE()),
        WarehouseId     VARCHAR(32) NOT NULL FOREIGN KEY REFERENCES Warehouse(Id),
        ProductId       VARCHAR(32) NOT NULL FOREIGN KEY REFERENCES Products(Id),
        InQty           DECIMAL(18,3) NOT NULL DEFAULT(0),
        OutQty          DECIMAL(18,3) NOT NULL DEFAULT(0),
        UnitCost        DECIMAL(18,2) NULL,      -- giá vốn tại thời điểm nhập/xuất
        Amount          DECIMAL(18,2) NULL,      -- InQty*UnitCost hoặc OutQty*UnitCost
        RefType         VARCHAR(20) NULL,        -- 'GRN','PI','DLY','SI','ADJ+','ADJ-','RET+','RET-'...
        RefId           VARCHAR(32) NULL,
        RefNo           NVARCHAR(32) NULL,
        RefLineId       VARCHAR(64) NULL,
        CreatedAt       DATETIME NOT NULL DEFAULT(GETDATE())
    );
    CREATE INDEX IX_StockTrans_Warehouse_Product ON StockTransactions(WarehouseId, ProductId, TransDate);
END
GO

-- Phiếu điều chỉnh sinh từ chênh lệch kiểm kê (TakingStock)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'StockAdjustMaster')
BEGIN
    CREATE TABLE StockAdjustMaster(
        Id              VARCHAR(32) PRIMARY KEY NOT NULL,
        DocNo           NVARCHAR(32) NULL,
        DocDate         DATETIME NOT NULL,
        WarehouseId     VARCHAR(32) NOT NULL FOREIGN KEY REFERENCES Warehouse(Id),
        Status          TINYINT NOT NULL DEFAULT(0),
        Notes           NVARCHAR(256) NULL,
        UserCreate      NVARCHAR(128) NULL,
        DateCreate      DATETIME NULL DEFAULT(GETDATE()),
        UserUpdate      NVARCHAR(128) NULL,
        DateUpdate      DATETIME NULL
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'StockAdjustDetail')
BEGIN
    CREATE TABLE StockAdjustDetail(
        Id              VARCHAR(64) PRIMARY KEY NOT NULL,
        AdjustId        VARCHAR(32) NOT NULL FOREIGN KEY REFERENCES StockAdjustMaster(Id),
        ProductId       VARCHAR(32) NOT NULL FOREIGN KEY REFERENCES Products(Id),
        DiffQty         DECIMAL(18,3) NOT NULL,      -- + thừa, - thiếu
        UnitCost        DECIMAL(18,2) NULL,
        Notes           NVARCHAR(256) NULL
    );
    CREATE INDEX IX_StockAdjustDetail_Adjust ON StockAdjustDetail(AdjustId);
END
GO

