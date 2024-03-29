USE [Bank]
GO
/****** Object:  Table [dbo].[CardDetails]    Script Date: 8/12/2019 9:17:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CardDetails](
	[CardId] [int] IDENTITY(1,1) NOT NULL,
	[CardNumber] [varchar](50) NOT NULL,
	[ExpiryMonth] [int] NOT NULL,
	[CVC] [int] NOT NULL,
	[ExpiryYear] [int] NOT NULL,
	[TotalAmount] [decimal](9, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[CardId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CardDetails] ON 

INSERT [dbo].[CardDetails] ([CardId], [CardNumber], [ExpiryMonth], [CVC], [ExpiryYear], [TotalAmount]) VALUES (1, N'4111111111111111', 12, 200, 2030, CAST(10680.00 AS Decimal(9, 2)))
INSERT [dbo].[CardDetails] ([CardId], [CardNumber], [ExpiryMonth], [CVC], [ExpiryYear], [TotalAmount]) VALUES (2, N'378282246310005', 12, 250, 2030, CAST(9300.00 AS Decimal(9, 2)))
INSERT [dbo].[CardDetails] ([CardId], [CardNumber], [ExpiryMonth], [CVC], [ExpiryYear], [TotalAmount]) VALUES (3, N'36259600000004', 12, 300, 2030, CAST(10000.00 AS Decimal(9, 2)))
INSERT [dbo].[CardDetails] ([CardId], [CardNumber], [ExpiryMonth], [CVC], [ExpiryYear], [TotalAmount]) VALUES (4, N'3530111333300000', 12, 100, 2030, CAST(10000.00 AS Decimal(9, 2)))
INSERT [dbo].[CardDetails] ([CardId], [CardNumber], [ExpiryMonth], [CVC], [ExpiryYear], [TotalAmount]) VALUES (5, N'5555555555554444', 12, 300, 2030, CAST(10000.00 AS Decimal(9, 2)))
INSERT [dbo].[CardDetails] ([CardId], [CardNumber], [ExpiryMonth], [CVC], [ExpiryYear], [TotalAmount]) VALUES (6, N'4005519200000004', 12, 500, 2030, CAST(10000.00 AS Decimal(9, 2)))
INSERT [dbo].[CardDetails] ([CardId], [CardNumber], [ExpiryMonth], [CVC], [ExpiryYear], [TotalAmount]) VALUES (7, N'4012000033330026', 12, 560, 2030, CAST(10000.00 AS Decimal(9, 2)))
INSERT [dbo].[CardDetails] ([CardId], [CardNumber], [ExpiryMonth], [CVC], [ExpiryYear], [TotalAmount]) VALUES (8, N'4012888888881881', 12, 660, 2030, CAST(10000.00 AS Decimal(9, 2)))
INSERT [dbo].[CardDetails] ([CardId], [CardNumber], [ExpiryMonth], [CVC], [ExpiryYear], [TotalAmount]) VALUES (9, N'4217651111111119', 12, 120, 2030, CAST(10000.00 AS Decimal(9, 2)))
SET IDENTITY_INSERT [dbo].[CardDetails] OFF
