USE [master]
GO
CREATE DATABASE [Personas]
GO 

USE [Personas]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Usuario](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nombreUsuario] [varchar](20) NOT NULL,
	[contraseniaSalt] [varchar] (50) NOT NULL,
	[contraseniaHashed] [varchar] (100) NOT NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

SET IDENTITY_INSERT [dbo].[Usuario] ON 

INSERT [dbo].[Usuario] ([id], [nombreUsuario], [contraseniaSalt], [contraseniaHashed]) VALUES (1, N'usuario1', N'cywdzqAanI9U3VckD065Rg==', N'g1XZPoQi9vdhni+5HGRXHrG8LgNDR5YOFscnJgjpG7s=')

SET IDENTITY_INSERT [dbo].[Usuario] OFF
GO