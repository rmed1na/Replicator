CREATE DATABASE [REPOS]
GO
USE [REPOS]
GO
/****** Object:  Table [dbo].[Almacen]    Script Date: 3/23/2020 8:09:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Almacen](
	[ID] [uniqueidentifier] NOT NULL,
	[FechaRegistro] [datetime] NULL,
	[SucursalID] [uniqueidentifier] NULL,
	[Codigo] [varchar](10) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Direccion] [varchar](300) NULL,
	[Estatus] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Articulo]    Script Date: 3/23/2020 8:09:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Articulo](
	[ID] [uniqueidentifier] NOT NULL,
	[FechaRegistro] [datetime] NULL,
	[Referencia] [varchar](50) NOT NULL,
	[Descripcion] [varchar](100) NULL,
	[FabricanteID] [uniqueidentifier] NULL,
	[Precio] [money] NULL,
	[PorcentajeImpuesto] [int] NULL,
	[Estatus] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Caja]    Script Date: 3/23/2020 8:09:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Caja](
	[ID] [uniqueidentifier] NOT NULL,
	[FechaRegistro] [datetime] NULL,
	[AlmacenID] [uniqueidentifier] NULL,
	[Codigo] [varchar](10) NOT NULL,
	[Nombre] [varchar](100) NULL,
	[Hostname] [varchar](100) NULL,
	[Ip] [varchar](50) NULL,
	[Estatus] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cliente]    Script Date: 3/23/2020 8:09:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cliente](
	[ID] [uniqueidentifier] NOT NULL,
	[FechaRegistro] [datetime] NULL,
	[Codigo] [varchar](20) NULL,
	[Nombre] [varchar](30) NOT NULL,
	[Apellido] [varchar](30) NOT NULL,
	[Telefono] [varchar](50) NULL,
	[Correo] [varchar](150) NULL,
	[Estatus] [bit] NOT NULL,
	[Cedula] [varchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Empresa]    Script Date: 3/23/2020 8:09:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empresa](
	[ID] [uniqueidentifier] NOT NULL,
	[FechaRegistro] [datetime] NULL,
	[Codigo] [varchar](10) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Estatus] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Fabricante]    Script Date: 3/23/2020 8:09:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fabricante](
	[ID] [uniqueidentifier] NOT NULL,
	[FechaRegistro] [datetime] NULL,
	[Codigo] [varchar](10) NOT NULL,
	[Nombre] [varchar](100) NULL,
	[Estatus] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sucursal]    Script Date: 3/23/2020 8:09:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sucursal](
	[ID] [uniqueidentifier] NOT NULL,
	[FechaRegistro] [datetime] NULL,
	[EmpresaID] [uniqueidentifier] NULL,
	[Codigo] [varchar](10) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Direccion] [varchar](300) NULL,
	[Estatus] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Venta]    Script Date: 3/23/2020 8:09:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Venta](
	[ID] [uniqueidentifier] NOT NULL,
	[FechaRegistro] [datetime] NULL,
	[SucursalID] [uniqueidentifier] NOT NULL,
	[ClienteID] [uniqueidentifier] NOT NULL,
	[CajaID] [uniqueidentifier] NULL,
	[SubTotal] [money] NOT NULL,
	[Impuesto] [money] NULL,
	[Total] [money] NOT NULL,
	[Lineas] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VentaD]    Script Date: 3/23/2020 8:09:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VentaD](
	[ID] [uniqueidentifier] NOT NULL,
	[FechaRegistro] [datetime] NULL,
	[VentaID] [uniqueidentifier] NOT NULL,
	[ArticuloID] [uniqueidentifier] NOT NULL,
	[Cantidad] [int] NOT NULL,
	[Precio] [money] NULL,
	[Descuento] [money] NULL,
	[SubTotal] [money] NOT NULL,
	[Impuesto] [money] NULL,
	[Total] [money] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Almacen] ([ID], [FechaRegistro], [SucursalID], [Codigo], [Nombre], [Direccion], [Estatus]) VALUES (N'bf0b9871-b3c5-4dba-bb14-307200c50e7c', CAST(N'2020-03-23T06:52:39.873' AS DateTime), N'8afc017c-79dd-464a-ac06-3825bd446f5a', N'A1-1', N'Piso de Venta Multiplaza', N'Mall Multiplaza Pacific. Local 15B. Segunda Planta', 1)
INSERT [dbo].[Cliente] ([ID], [FechaRegistro], [Codigo], [Nombre], [Apellido], [Telefono], [Correo], [Estatus], [Cedula]) VALUES (N'e7a21583-2784-4513-9f97-521940d788c7', CAST(N'2020-03-24T00:51:20.520' AS DateTime), NULL, N'Jose', N'Perez', NULL, NULL, 1, N'8-000-0000')
INSERT [dbo].[Cliente] ([ID], [FechaRegistro], [Codigo], [Nombre], [Apellido], [Telefono], [Correo], [Estatus], [Cedula]) VALUES (N'8070032e-2341-4107-969c-6dae75b52b48', CAST(N'2020-03-24T00:51:20.527' AS DateTime), NULL, N'Jorge', N'Arauz', NULL, NULL, 1, N'8-000-0000')
INSERT [dbo].[Cliente] ([ID], [FechaRegistro], [Codigo], [Nombre], [Apellido], [Telefono], [Correo], [Estatus], [Cedula]) VALUES (N'7ba1b3fe-89ea-4a5d-a8e3-acc37c8044ad', CAST(N'2020-03-24T00:51:20.527' AS DateTime), NULL, N'Maria', N'Gonzalez', NULL, NULL, 1, N'8-000-0000')
INSERT [dbo].[Cliente] ([ID], [FechaRegistro], [Codigo], [Nombre], [Apellido], [Telefono], [Correo], [Estatus], [Cedula]) VALUES (N'27965884-9204-4d90-b1d4-ffa69605630d', CAST(N'2020-03-24T00:31:40.493' AS DateTime), NULL, N'Rolando', N'Medina', N'64821553', N'rolando.ms@outlook.com', 1, N'8-909-2269')
INSERT [dbo].[Empresa] ([ID], [FechaRegistro], [Codigo], [Nombre], [Estatus]) VALUES (N'97dc7f59-2eaf-4864-8d84-1125e55ed6aa', CAST(N'2020-03-23T06:46:40.937' AS DateTime), N'E1', N'Pro Retailers S.A.', 1)
INSERT [dbo].[Sucursal] ([ID], [FechaRegistro], [EmpresaID], [Codigo], [Nombre], [Direccion], [Estatus]) VALUES (N'8afc017c-79dd-464a-ac06-3825bd446f5a', CAST(N'2020-03-23T06:50:47.200' AS DateTime), N'97dc7f59-2eaf-4864-8d84-1125e55ed6aa', N'S1-1', N'Tienda Pro Retailers Multiplaza', N'Mall Multiplaza Pacific. Via Israel', 1)
ALTER TABLE [dbo].[Almacen] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Almacen] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[Almacen] ADD  DEFAULT ((1)) FOR [Estatus]
GO
ALTER TABLE [dbo].[Articulo] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Articulo] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[Articulo] ADD  DEFAULT ((1)) FOR [Estatus]
GO
ALTER TABLE [dbo].[Caja] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Caja] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[Caja] ADD  DEFAULT ((1)) FOR [Estatus]
GO
ALTER TABLE [dbo].[Cliente] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Cliente] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[Cliente] ADD  DEFAULT ((1)) FOR [Estatus]
GO
ALTER TABLE [dbo].[Empresa] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Empresa] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[Empresa] ADD  DEFAULT ((1)) FOR [Estatus]
GO
ALTER TABLE [dbo].[Fabricante] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Fabricante] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[Fabricante] ADD  DEFAULT ((1)) FOR [Estatus]
GO
ALTER TABLE [dbo].[Sucursal] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Sucursal] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[Sucursal] ADD  DEFAULT ((1)) FOR [Estatus]
GO
ALTER TABLE [dbo].[Venta] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Venta] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[VentaD] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[VentaD] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[Almacen]  WITH CHECK ADD FOREIGN KEY([SucursalID])
REFERENCES [dbo].[Sucursal] ([ID])
GO
ALTER TABLE [dbo].[Almacen]  WITH CHECK ADD FOREIGN KEY([SucursalID])
REFERENCES [dbo].[Sucursal] ([ID])
GO
ALTER TABLE [dbo].[Articulo]  WITH CHECK ADD FOREIGN KEY([FabricanteID])
REFERENCES [dbo].[Fabricante] ([ID])
GO
ALTER TABLE [dbo].[Articulo]  WITH CHECK ADD FOREIGN KEY([FabricanteID])
REFERENCES [dbo].[Fabricante] ([ID])
GO
ALTER TABLE [dbo].[Caja]  WITH CHECK ADD FOREIGN KEY([AlmacenID])
REFERENCES [dbo].[Almacen] ([ID])
GO
ALTER TABLE [dbo].[Caja]  WITH CHECK ADD FOREIGN KEY([AlmacenID])
REFERENCES [dbo].[Almacen] ([ID])
GO
ALTER TABLE [dbo].[Sucursal]  WITH CHECK ADD FOREIGN KEY([EmpresaID])
REFERENCES [dbo].[Empresa] ([ID])
GO
ALTER TABLE [dbo].[Sucursal]  WITH CHECK ADD FOREIGN KEY([EmpresaID])
REFERENCES [dbo].[Empresa] ([ID])
GO
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD FOREIGN KEY([CajaID])
REFERENCES [dbo].[Caja] ([ID])
GO
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD FOREIGN KEY([CajaID])
REFERENCES [dbo].[Caja] ([ID])
GO
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD FOREIGN KEY([ClienteID])
REFERENCES [dbo].[Cliente] ([ID])
GO
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD FOREIGN KEY([ClienteID])
REFERENCES [dbo].[Cliente] ([ID])
GO
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD FOREIGN KEY([SucursalID])
REFERENCES [dbo].[Sucursal] ([ID])
GO
ALTER TABLE [dbo].[Venta]  WITH CHECK ADD FOREIGN KEY([SucursalID])
REFERENCES [dbo].[Sucursal] ([ID])
GO
ALTER TABLE [dbo].[VentaD]  WITH CHECK ADD FOREIGN KEY([ArticuloID])
REFERENCES [dbo].[Articulo] ([ID])
GO
ALTER TABLE [dbo].[VentaD]  WITH CHECK ADD FOREIGN KEY([ArticuloID])
REFERENCES [dbo].[Articulo] ([ID])
GO
ALTER TABLE [dbo].[VentaD]  WITH CHECK ADD FOREIGN KEY([VentaID])
REFERENCES [dbo].[Venta] ([ID])
GO
ALTER TABLE [dbo].[VentaD]  WITH CHECK ADD FOREIGN KEY([VentaID])
REFERENCES [dbo].[Venta] ([ID])
GO
CREATE TABLE Inventario(
	ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT(NEWID()),
	FechaRegistro DATETIME DEFAULT(GETDATE()),
	AlmacenID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Almacen(ID) NOT NULL,
	Disponible INT DEFAULT(0),
	Reservado INT DEFAULT(0),
	ArticuloID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Articulo(ID) NOT NULL,
	Estatus BIT DEFAULT(1))
GO
CREATE TABLE [dbo].[Usuario](
	[ID] [uniqueidentifier] NOT NULL,
	[FechaRegistro] [datetime] NULL,
	[Usuario] [varchar](100) NOT NULL,
	[Nombre] [varchar](50) NULL,
	[Apellido] [varchar](50) NULL,
	[Contrasena] [varchar](1000) NULL,
	[Estatus] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Usuario] ([ID], [FechaRegistro], [Usuario], [Nombre], [Apellido], [Contrasena], [Estatus]) VALUES (N'1c625890-684c-441c-9184-64e5d7581638', CAST(N'2020-04-17T19:49:04.977' AS DateTime), N'ro.medina', N'Rolando', N'Medina', N'FLLnndICIVQiNAwWmIq86W3nV77r7HibNd2nWzd2DDrm7nnABcN1vpMQZlcYXlqO', 1)
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Usuario__E3237CF77894A389]    Script Date: 5/2/2020 9:16:43 PM ******/
ALTER TABLE [dbo].[Usuario] ADD UNIQUE NONCLUSTERED 
(
	[Usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Usuario] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Usuario] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[Usuario] ADD  DEFAULT ((1)) FOR [Estatus]
GO
