SET IDENTITY_INSERT [dbo].[Movies] ON
INSERT INTO [dbo].[Movies] ([Id], [Title], [PriceForPurchase], [QuantityForPurchase], [GenreId], [ReleaseDate], [PriceForRenting], [QuantityForRenting]) VALUES (4, N'IT', CAST(100.00 AS Decimal(10, 2)), 5, 1, N'2025-07-02 09:50:00', 10, 5)
SET IDENTITY_INSERT [dbo].[Movies] OFF
