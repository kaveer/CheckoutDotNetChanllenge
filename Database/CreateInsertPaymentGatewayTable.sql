USE [PaymentGateway]
GO
/****** Object:  Table [dbo].[ApplicationLog]    Script Date: 8/12/2019 9:21:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationLog](
	[LogId] [int] IDENTITY(1,1) NOT NULL,
	[LogType] [varchar](100) NOT NULL,
	[Details] [varchar](max) NULL,
	[DateCreated] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Merchant]    Script Date: 8/12/2019 9:21:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Merchant](
	[merId] [int] IDENTITY(1,1) NOT NULL,
	[MerchantId] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[merId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MerchantDetails]    Script Date: 8/12/2019 9:21:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MerchantDetails](
	[DetailsId] [int] IDENTITY(1,1) NOT NULL,
	[MerchantId] [varchar](100) NOT NULL,
	[CardNumber] [varchar](50) NOT NULL,
	[CVC] [int] NOT NULL,
	[ExpiryMonth] [int] NOT NULL,
	[ExpiryYear] [int] NOT NULL,
	[TotalAmount] [decimal](9, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[DetailsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MerchantKey]    Script Date: 8/12/2019 9:21:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MerchantKey](
	[KeyId] [int] IDENTITY(1,1) NOT NULL,
	[merId] [int] NOT NULL,
	[PublicKey] [varchar](max) NOT NULL,
	[PrivateKey] [varchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[KeyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Token]    Script Date: 8/12/2019 9:21:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Token](
	[TokenId] [int] IDENTITY(1,1) NOT NULL,
	[MerchantId] [varchar](100) NOT NULL,
	[ClientToken] [varchar](max) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TokenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionLog]    Script Date: 8/12/2019 9:21:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionLog](
	[TransactionId] [int] IDENTITY(1,1) NOT NULL,
	[TransactionIdentifiyer] [varchar](100) NULL,
	[MerchantId] [varchar](100) NOT NULL,
	[IsSuccess] [bit] NULL,
	[AmountCredited] [decimal](9, 2) NULL,
	[BankResponse] [varchar](500) NULL,
	[TransactionDate] [datetime] NULL,
	[CVC] [int] NOT NULL,
	[CardNumber] [varchar](100) NULL,
	[ExpiryMonth] [int] NOT NULL,
	[ExpiryYear] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Merchant] ON 

INSERT [dbo].[Merchant] ([merId], [MerchantId]) VALUES (1, N'd585e5c0d07a40deaeb2f18eea8bf321')
SET IDENTITY_INSERT [dbo].[Merchant] OFF
SET IDENTITY_INSERT [dbo].[MerchantDetails] ON 

INSERT [dbo].[MerchantDetails] ([DetailsId], [MerchantId], [CardNumber], [CVC], [ExpiryMonth], [ExpiryYear], [TotalAmount]) VALUES (1, N'd585e5c0d07a40deaeb2f18eea8bf321', N'4111111111111111', 200, 12, 2030, CAST(10000.00 AS Decimal(9, 2)))
SET IDENTITY_INSERT [dbo].[MerchantDetails] OFF
SET IDENTITY_INSERT [dbo].[MerchantKey] ON 

INSERT [dbo].[MerchantKey] ([KeyId], [merId], [PublicKey], [PrivateKey]) VALUES (2, 1, N'MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCoF7md16kHvwAJY6z+sN0ccmaehqNo1S1ORCLd9sXJDLwQx8beOM5gCoYybX+QDvAslV+CG/08axa0WWgYzDerC3Tr+aorLtt8c5bR0rTxXdatVFMDwaROyseNcwNCtVKPbUZOu/c+46Zbgb/KKLM8lmdqhenZfx7O9WHLC80BNwIDAQAB', N'MIICdQIBADANBgkqhkiG9w0BAQEFAASCAl8wggJbAgEAAoGBAKgXuZ3XqQe/AAljrP6w3RxyZp6Go2jVLU5EIt32xckMvBDHxt44zmAKhjJtf5AO8CyVX4Ib/TxrFrRZaBjMN6sLdOv5qisu23xzltHStPFd1q1UUwPBpE7Kx41zA0K1Uo9tRk679z7jpluBv8ooszyWZ2qF6dl/Hs71YcsLzQE3AgMBAAECgYAXxWMmgz0QL16d9U6dpf1e7H6+pGOvi5Ygn1oe8ar+x6JA7T+eZYIp6tMXhoynnrUwWN9s0vB4/tvzHUGvYBHvD4apmAxXiFbHCYsYhuwxk5SMMjUXRJF8yi/9Mvv8ioad3hiuNljM7VQfUzHzAX6KJGUDaE3qNeoyG9LBmGACYQJBAOJSjMC2F0CTi5yn4nL4GcuB8AQu/XC9tQ8EFflifa22/ZgdNOAEBAKgu6UgGRv0wuDCXCWMVC/2zu88ppkqf0MCQQC+InOWe6hoTl3FhyFCN7DL8mlgdRT3Daz3oFk61pcboRVLF03QtSftQdJ76W/EVPg/VfPSrv2+2Rx5XCBF9BT9AkBwGzHrd4c3Dp72X1bVWj30x41rlRcnVGEuafi0imv0s5MUWUtKt9KAtrucRLULWwd5K+1XEBbXl2rTqHhhoGJfAkAelZAeTrniPWjcE0aITkkEJXRJ7ct3ih2en4565nXcHec88vyza9CGW6YuBHjUDg74sSzNYRS0FFx+MRGH7yFJAkBo5fNMaq2Fm+YBV6o0Or7ezJqmroopUPXqqk39IHJCcEucA41qV4hFdt/ZHNljjEPNI0+79nqvXFSj47Ipz14o')
SET IDENTITY_INSERT [dbo].[MerchantKey] OFF

SET IDENTITY_INSERT [dbo].[TransactionLog] ON 

INSERT [dbo].[TransactionLog] ([TransactionId], [TransactionIdentifiyer], [MerchantId], [IsSuccess], [AmountCredited], [BankResponse], [TransactionDate], [CVC], [CardNumber], [ExpiryMonth], [ExpiryYear]) VALUES (1, N'b6e76f88-f87a-4277-af56-746fbfa33ae8', N'd585e5c0d07a40deaeb2f18eea8bf321', 1, CAST(100.00 AS Decimal(9, 2)), N'Transaction successfull', CAST(N'2019-08-12T15:11:47.453' AS DateTime), 250, N'378282246310005', 10, 2019)
INSERT [dbo].[TransactionLog] ([TransactionId], [TransactionIdentifiyer], [MerchantId], [IsSuccess], [AmountCredited], [BankResponse], [TransactionDate], [CVC], [CardNumber], [ExpiryMonth], [ExpiryYear]) VALUES (2, N'8757345b-d4c5-4964-9cde-a99b2f37d7a2', N'd585e5c0d07a40deaeb2f18eea8bf321', 1, CAST(200.00 AS Decimal(9, 2)), N'Transaction successfull', CAST(N'2019-08-12T15:12:48.863' AS DateTime), 250, N'378282246310005', 10, 2019)
INSERT [dbo].[TransactionLog] ([TransactionId], [TransactionIdentifiyer], [MerchantId], [IsSuccess], [AmountCredited], [BankResponse], [TransactionDate], [CVC], [CardNumber], [ExpiryMonth], [ExpiryYear]) VALUES (3, N'eb0ee8c8-751a-4c02-b41b-45b7721a0a66', N'd585e5c0d07a40deaeb2f18eea8bf321', 1, CAST(150.00 AS Decimal(9, 2)), N'Transaction successfull', CAST(N'2019-08-12T15:12:58.137' AS DateTime), 250, N'378282246310005', 10, 2019)
INSERT [dbo].[TransactionLog] ([TransactionId], [TransactionIdentifiyer], [MerchantId], [IsSuccess], [AmountCredited], [BankResponse], [TransactionDate], [CVC], [CardNumber], [ExpiryMonth], [ExpiryYear]) VALUES (4, N'cbee99dc-20a5-4ad6-8c30-7d9aae9827a4', N'd585e5c0d07a40deaeb2f18eea8bf321', 1, CAST(50.00 AS Decimal(9, 2)), N'Transaction successfull', CAST(N'2019-08-12T15:13:03.363' AS DateTime), 250, N'378282246310005', 10, 2019)
SET IDENTITY_INSERT [dbo].[TransactionLog] OFF
ALTER TABLE [dbo].[ApplicationLog] ADD  DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Token] ADD  DEFAULT (getdate()) FOR [DateCreated]
GO
