SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Almacen](
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Articulo](
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Caja](
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Cliente](
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Empresa](
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Fabricante](
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Inventario](
	[ID] [uniqueidentifier] NOT NULL,
	[FechaRegistro] [datetime] NULL,
	[AlmacenID] [uniqueidentifier] NOT NULL,
	[Disponible] [int] NULL,
	[Reservado] [int] NULL,
	[ArticuloID] [uniqueidentifier] NOT NULL,
	[Estatus] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Sucursal](
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Venta](
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [VentaD](
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
ALTER TABLE [Almacen] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [Almacen] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [Almacen] ADD  DEFAULT ((1)) FOR [Estatus]
GO
ALTER TABLE [Articulo] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [Articulo] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [Articulo] ADD  DEFAULT ((1)) FOR [Estatus]
GO
ALTER TABLE [Caja] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [Caja] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [Caja] ADD  DEFAULT ((1)) FOR [Estatus]
GO
ALTER TABLE [Cliente] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [Cliente] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [Cliente] ADD  DEFAULT ((1)) FOR [Estatus]
GO
ALTER TABLE [Empresa] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [Empresa] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [Empresa] ADD  DEFAULT ((1)) FOR [Estatus]
GO
ALTER TABLE [Fabricante] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [Fabricante] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [Fabricante] ADD  DEFAULT ((1)) FOR [Estatus]
GO
ALTER TABLE [Inventario] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [Inventario] ADD  DEFAULT ((0)) FOR [Disponible]
GO
ALTER TABLE [Inventario] ADD  DEFAULT ((0)) FOR [Reservado]
GO
ALTER TABLE [Inventario] ADD  DEFAULT ((1)) FOR [Estatus]
GO
ALTER TABLE [Sucursal] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [Sucursal] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [Sucursal] ADD  DEFAULT ((1)) FOR [Estatus]
GO
ALTER TABLE [Venta] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [Venta] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [VentaD] ADD  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [VentaD] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [Almacen]  WITH CHECK ADD FOREIGN KEY([SucursalID])
REFERENCES [Sucursal] ([ID])
GO
ALTER TABLE [Almacen]  WITH CHECK ADD FOREIGN KEY([SucursalID])
REFERENCES [Sucursal] ([ID])
GO
ALTER TABLE [Articulo]  WITH CHECK ADD FOREIGN KEY([FabricanteID])
REFERENCES [Fabricante] ([ID])
GO
ALTER TABLE [Articulo]  WITH CHECK ADD FOREIGN KEY([FabricanteID])
REFERENCES [Fabricante] ([ID])
GO
ALTER TABLE [Caja]  WITH CHECK ADD FOREIGN KEY([AlmacenID])
REFERENCES [Almacen] ([ID])
GO
ALTER TABLE [Caja]  WITH CHECK ADD FOREIGN KEY([AlmacenID])
REFERENCES [Almacen] ([ID])
GO
ALTER TABLE [Inventario]  WITH CHECK ADD FOREIGN KEY([AlmacenID])
REFERENCES [Almacen] ([ID])
GO
ALTER TABLE [Inventario]  WITH CHECK ADD FOREIGN KEY([ArticuloID])
REFERENCES [Articulo] ([ID])
GO
ALTER TABLE [Sucursal]  WITH CHECK ADD FOREIGN KEY([EmpresaID])
REFERENCES [Empresa] ([ID])
GO
ALTER TABLE [Sucursal]  WITH CHECK ADD FOREIGN KEY([EmpresaID])
REFERENCES [Empresa] ([ID])
GO
ALTER TABLE [Venta]  WITH CHECK ADD FOREIGN KEY([CajaID])
REFERENCES [Caja] ([ID])
GO
ALTER TABLE [Venta]  WITH CHECK ADD FOREIGN KEY([CajaID])
REFERENCES [Caja] ([ID])
GO
ALTER TABLE [Venta]  WITH CHECK ADD FOREIGN KEY([ClienteID])
REFERENCES [Cliente] ([ID])
GO
ALTER TABLE [Venta]  WITH CHECK ADD FOREIGN KEY([ClienteID])
REFERENCES [Cliente] ([ID])
GO
ALTER TABLE [Venta]  WITH CHECK ADD FOREIGN KEY([SucursalID])
REFERENCES [Sucursal] ([ID])
GO
ALTER TABLE [Venta]  WITH CHECK ADD FOREIGN KEY([SucursalID])
REFERENCES [Sucursal] ([ID])
GO
ALTER TABLE [VentaD]  WITH CHECK ADD FOREIGN KEY([ArticuloID])
REFERENCES [Articulo] ([ID])
GO
ALTER TABLE [VentaD]  WITH CHECK ADD FOREIGN KEY([ArticuloID])
REFERENCES [Articulo] ([ID])
GO
ALTER TABLE [VentaD]  WITH CHECK ADD FOREIGN KEY([VentaID])
REFERENCES [Venta] ([ID])
GO
ALTER TABLE [VentaD]  WITH CHECK ADD FOREIGN KEY([VentaID])
REFERENCES [Venta] ([ID])
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
