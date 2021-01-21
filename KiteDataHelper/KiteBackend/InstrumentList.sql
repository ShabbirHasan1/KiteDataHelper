CREATE TABLE [dbo].[InstrumentList]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[InstrumentToken] INT NOT NULL,
	[ExchangeToken] INT NOT NULL,
	[TradingSymbol] VARCHAR(100) NOT NULL,
	[Name] VARCHAR(200) NOT NULL,
	[LastPrice] INT NOT NULL,
	[TickSize] INT NOT NULL,
	[Expiry] datetime NOT NULL,
	[InstrumentType] VARCHAR(100) NOT NULL,
	[Segment] VARCHAR(20) NOT NULL,
	[Exchange] VARCHAR(100) NOT NULL,
	[Strike] INT,
	[LotSize] INT NOT NULL,
	[Created] datetime NOT NULL,
	[Updated] datetime NOT NULL

)
