CREATE TABLE [dbo].[GeneratedTrades]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[Logic] VARCHAR(100) NOT NULL,
	[InstrumentToken] VARCHAR(100) NOT NULL,
	[IntrumentName] VARCHAR(100) NOT NULL,
	[ExchangeToken] VARCHAR(100) NOT NULL,
	[Exchange] VARCHAR(100) NOT NULL,
	[EntryPrice] FLOAT NOT NULL,
	[IsTaken] bit NOT NULL,
	[GeneratedAt] datetime NOT NULL
)
