--Script para crear la las tablas y realizar los insert de los status 
CREATE TABLE [dbo].[products](
	[id_product] [int] IDENTITY(1,1) NOT NULL,
	[sku] [varchar](20) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[unit_price] [float] NOT NULL,
	[stock] [int] NOT NULL,
	[deleted] [bit] NOT NULL,
	[creation_date] [datetime] NOT NULL,
	[modification_date] [datetime] NOT NULL,
 CONSTRAINT [PK_products] PRIMARY KEY CLUSTERED 
(
	[id_product] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[status](
	[id_status] [int] NOT NULL,
	[name] [varchar](50) NOT NULL,
	[creation_date] [datetime] NOT NULL,
	[modification_date] [datetime] NOT NULL,
 CONSTRAINT [PK_status] PRIMARY KEY CLUSTERED 
(
	[id_status] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[orders](
	[id_order] [int] NOT NULL,
	[creation_date] [datetime] NOT NULL,
	[modification_date] [datetime] NOT NULL,
	[id_status] [int] NOT NULL,
 CONSTRAINT [PK_orders] PRIMARY KEY CLUSTERED 
(
	[id_order] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[order_details](
	[unit_price] [float] NOT NULL,
	[quantity] [int] NOT NULL,
	[creation_date] [datetime] NOT NULL,
	[modification_date] [datetime] NOT NULL,
	[id_order] [int] NOT NULL,
	[id_product] [int] NOT NULL,
)
GO
ALTER TABLE [dbo].[orders]  WITH CHECK ADD  CONSTRAINT [FK_ORDERS-STATUS] FOREIGN KEY([id_status])
REFERENCES [dbo].[status] ([id_status])
ALTER TABLE [dbo].[order_details]  WITH CHECK ADD  CONSTRAINT [FK_ORDER_DETAILS-ORDERS] FOREIGN KEY([id_order])
REFERENCES [dbo].[orders] ([id_order])
ALTER TABLE [dbo].[order_details]  WITH CHECK ADD  CONSTRAINT [FK_ORDER_DETAILS-PRODUCTS] FOREIGN KEY([id_product])
REFERENCES [dbo].[products] ([id_product])

INSERT INTO [dbo].[status]
           ([id_status]
           ,[name]
           ,[creation_date]
           ,[modification_date])
     VALUES
           (1
           ,'Pending'
           ,GETDATE()
           ,GETDATE()),
			(2
           ,'In Process'
           ,GETDATE()
           ,GETDATE()),
			(3
           ,'Completed'
           ,GETDATE()
           ,GETDATE()),
			(4
           ,'Delivered'
           ,GETDATE()
           ,GETDATE()),
			(5
           ,'Canceled'
           ,GETDATE()
           ,GETDATE())
