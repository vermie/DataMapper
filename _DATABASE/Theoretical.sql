USE [master]
GO
/****** Object:  Database [Theoretical]    Script Date: 11/02/2012 14:14:57 ******/
CREATE DATABASE [Theoretical] ON  PRIMARY 
( NAME = N'Theoretical', FILENAME = N'C:\CODE\SqlDatabaseMDF\Theoretical.mdf' , SIZE = 1536KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Theoretical_log', FILENAME = N'C:\CODE\SqlDatabaseMDF\Theoretical_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Theoretical] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Theoretical].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Theoretical] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [Theoretical] SET ANSI_NULLS OFF
GO
ALTER DATABASE [Theoretical] SET ANSI_PADDING OFF
GO
ALTER DATABASE [Theoretical] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [Theoretical] SET ARITHABORT OFF
GO
ALTER DATABASE [Theoretical] SET AUTO_CLOSE ON
GO
ALTER DATABASE [Theoretical] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [Theoretical] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [Theoretical] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [Theoretical] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [Theoretical] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [Theoretical] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [Theoretical] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [Theoretical] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [Theoretical] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [Theoretical] SET  DISABLE_BROKER
GO
ALTER DATABASE [Theoretical] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [Theoretical] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [Theoretical] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [Theoretical] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [Theoretical] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [Theoretical] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [Theoretical] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [Theoretical] SET  READ_WRITE
GO
ALTER DATABASE [Theoretical] SET RECOVERY SIMPLE
GO
ALTER DATABASE [Theoretical] SET  MULTI_USER
GO
ALTER DATABASE [Theoretical] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [Theoretical] SET DB_CHAINING OFF
GO
USE [Theoretical]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 11/02/2012 14:14:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Product](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[VendorName] [varchar](100) NOT NULL,
	[Category] [varchar](100) NOT NULL,
	[Description] [varchar](255) NOT NULL,
	[SalePrice] [money] NOT NULL,
	[IsDiscontinued] [bit] NOT NULL,
	[IsSerialNumberRequired] [bit] NOT NULL,
	[ConcurrencyId] [int] NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Associate]    Script Date: 11/02/2012 14:14:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Associate](
	[AssociateId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Password] [binary](100) NOT NULL,
	[FirstName] [varchar](100) NOT NULL,
	[LastName] [varchar](100) NOT NULL,
	[ConcurrencyId] [int] NOT NULL,
 CONSTRAINT [PK_Associate] PRIMARY KEY CLUSTERED 
(
	[AssociateId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Associate] ON [dbo].[Associate] 
(
	[AssociateId] ASC,
	[Username] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 11/02/2012 14:14:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Account](
	[AccountId] [int] IDENTITY(1,1) NOT NULL,
	[Number] [varchar](20) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[ConcurrencyId] [int] NOT NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PurchaseOrder]    Script Date: 11/02/2012 14:14:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PurchaseOrder](
	[PurchaseOrderId] [int] IDENTITY(1,1) NOT NULL,
	[Number] [varchar](100) NOT NULL,
	[Vendor] [varchar](100) NOT NULL,
	[ShippingCost] [money] NOT NULL,
	[DatePurchased] [datetime] NOT NULL,
	[PurchasedBy] [varchar](100) NOT NULL,
	[ConcurrencyId] [int] NOT NULL,
 CONSTRAINT [PK_PurchaseOrder] PRIMARY KEY CLUSTERED 
(
	[PurchaseOrderId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProductUpc]    Script Date: 11/02/2012 14:14:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductUpc](
	[ProductUpcId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[Upc] [varchar](100) NOT NULL,
	[ConcurrencyId] [int] NOT NULL,
 CONSTRAINT [PK_ProductUpc] PRIMARY KEY CLUSTERED 
(
	[ProductUpcId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ProductUpc] ON [dbo].[ProductUpc] 
(
	[ProductId] ASC,
	[Upc] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductInventory]    Script Date: 11/02/2012 14:14:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProductInventory](
	[ProductInventoryId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[PurchaseOrderId] [int] NOT NULL,
	[Cost] [money] NOT NULL,
	[SerialNumber] [varchar](100) NOT NULL,
	[DateReceived] [datetime] NOT NULL,
	[ReceivedBy] [varchar](100) NOT NULL,
	[ConcurrencyId] [int] NOT NULL,
 CONSTRAINT [PK_ProductInventory] PRIMARY KEY CLUSTERED 
(
	[ProductInventoryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ProductInventory] ON [dbo].[ProductInventory] 
(
	[ProductId] ASC,
	[SerialNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 11/02/2012 14:14:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Order](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Number] [varchar](100) NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[TaxRate] [money] NOT NULL,
	[ConcurrencyId] [int] NOT NULL,
	[OptionalNote] [varchar](100) NULL,
	[OptionalPrice] [money] NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Order] ON [dbo].[Order] 
(
	[AccountId] ASC,
	[Number] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderItem]    Script Date: 11/02/2012 14:14:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OrderItem](
	[OrderItemId] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[SalePrice] [money] NOT NULL,
	[Upc] [varchar](100) NOT NULL,
	[HasSerialNumber] [bit] NOT NULL,
	[SerialNumber] [varchar](100) NOT NULL,
	[ConcurrencyId] [int] NOT NULL,
 CONSTRAINT [PK_OrderItem] PRIMARY KEY CLUSTERED 
(
	[OrderItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OrderInformation]    Script Date: 11/02/2012 14:14:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OrderInformation](
	[OrderInformationId] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[TrackingNumber] [varchar](100) NULL,
	[TrackAmount] [decimal](18, 0) NULL,
	[TrackDate] [datetime] NULL,
 CONSTRAINT [PK_OrderInformation] PRIMARY KEY CLUSTERED 
(
	[OrderInformationId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[DeleteOrders]    Script Date: 11/02/2012 14:15:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DeleteOrders]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DELETE FROM OrderItem;
	DELETE FROM [Order];
END
GO
/****** Object:  ForeignKey [FK_ProductUpc_Product]    Script Date: 11/02/2012 14:14:58 ******/
ALTER TABLE [dbo].[ProductUpc]  WITH CHECK ADD  CONSTRAINT [FK_ProductUpc_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductId])
GO
ALTER TABLE [dbo].[ProductUpc] CHECK CONSTRAINT [FK_ProductUpc_Product]
GO
/****** Object:  ForeignKey [FK_ProductInventory_Product]    Script Date: 11/02/2012 14:14:58 ******/
ALTER TABLE [dbo].[ProductInventory]  WITH CHECK ADD  CONSTRAINT [FK_ProductInventory_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductId])
GO
ALTER TABLE [dbo].[ProductInventory] CHECK CONSTRAINT [FK_ProductInventory_Product]
GO
/****** Object:  ForeignKey [FK_ProductInventory_PurchaseOrder]    Script Date: 11/02/2012 14:14:58 ******/
ALTER TABLE [dbo].[ProductInventory]  WITH CHECK ADD  CONSTRAINT [FK_ProductInventory_PurchaseOrder] FOREIGN KEY([PurchaseOrderId])
REFERENCES [dbo].[PurchaseOrder] ([PurchaseOrderId])
GO
ALTER TABLE [dbo].[ProductInventory] CHECK CONSTRAINT [FK_ProductInventory_PurchaseOrder]
GO
/****** Object:  ForeignKey [FK_Order_Account]    Script Date: 11/02/2012 14:14:58 ******/
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([AccountId])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Account]
GO
/****** Object:  ForeignKey [FK_OrderItem_Order]    Script Date: 11/02/2012 14:14:58 ******/
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
GO
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_Order]
GO
/****** Object:  ForeignKey [FK_OrderInformation_Order]    Script Date: 11/02/2012 14:14:58 ******/
ALTER TABLE [dbo].[OrderInformation]  WITH CHECK ADD  CONSTRAINT [FK_OrderInformation_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
GO
ALTER TABLE [dbo].[OrderInformation] CHECK CONSTRAINT [FK_OrderInformation_Order]
GO
