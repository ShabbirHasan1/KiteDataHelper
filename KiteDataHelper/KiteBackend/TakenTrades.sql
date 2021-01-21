CREATE TABLE [dbo].[TakenTrades]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[GeneratedTradeId] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[GeneratedTrades](Id),
	[EntryPrice] FLOAT NOT NULL,
	[IsOpen] bit NOT NULL,
	[TakenAt] datetime NOT NULL
)
