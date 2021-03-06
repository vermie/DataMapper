USE [Theoretical]
GO
/****** Object:  Table [dbo].[MultiKey]    Script Date: 5/12/2013 4:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MultiKey](
	[KeyOne] [int] NOT NULL,
	[KeyTwo] [uniqueidentifier] NOT NULL,
	[KeyThree] [varchar](10) NOT NULL,
	[OrderId] [int] NOT NULL,
	[Number] [varchar](20) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[ConcurrencyId] [int] NOT NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[KeyOne] ASC,
	[KeyTwo] ASC,
	[KeyThree] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Order]    Script Date: 5/12/2013 4:32:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Order](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OrderInformation]    Script Date: 5/12/2013 4:32:14 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OrderItem]    Script Date: 5/12/2013 4:32:14 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[MultiKey]  WITH CHECK ADD  CONSTRAINT [FK_MultiKey_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MultiKey] CHECK CONSTRAINT [FK_MultiKey_Order]
GO
ALTER TABLE [dbo].[OrderInformation]  WITH CHECK ADD  CONSTRAINT [FK_OrderInformation_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderInformation] CHECK CONSTRAINT [FK_OrderInformation_Order]
GO
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_Order]
GO
