USE [SolucionesRecidenciales]
GO
/****** Objeto: Table [dbo].[Buildings] Fecha de script: 28/04/2026 7:53:51 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Buildings](
	[IdEdificio] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](255) NOT NULL,
	[Email] [nvarchar](100) NULL,
	[Telefono] [nvarchar](20) NULL,
	[Direccion] [nvarchar](255) NULL,
	[Ciudad] [nvarchar](100) NULL,
	[Departamento] [nvarchar](100) NULL,
	[Pais] [nvarchar](100) NULL,
	[Nit] [nvarchar](20) NULL,
	[Activo] [int] NULL,
	[CompanyId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdEdificio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Companies] Fecha de script: 28/04/2026 7:53:51 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Companies](
	[IdEmpresa] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](255) NOT NULL,
	[Nit] [nvarchar](20) NULL,
	[Email] [nvarchar](100) NULL,
	[Telefono] [nvarchar](20) NULL,
	[Direccion] [nvarchar](255) NULL,
	[Activo] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdEmpresa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Employees] Fecha de script: 28/04/2026 7:53:51 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[IdEmpleado] [int] IDENTITY(1,1) NOT NULL,
	[NumeroDocumento] [nvarchar](20) NULL,
	[NombreCompleto] [nvarchar](255) NOT NULL,
	[Telefono] [nvarchar](20) NULL,
	[Email] [nvarchar](100) NULL,
	[Direccion] [nvarchar](255) NULL,
	[Barrio] [nvarchar](100) NULL,
	[FechaIngreso] [nvarchar](50) NULL,
	[Activo] [int] NULL,
	[BuildingId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdEmpleado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[QuotationItems] Fecha de script: 28/04/2026 7:53:51 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuotationItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdCotizacion] [int] NOT NULL,
	[Descripcion] [nvarchar](max) NOT NULL,
	[Cantidad] [decimal](18, 2) NULL,
	[UnidadMedida] [nvarchar](50) NULL,
	[Imagen] [varbinary](max) NULL,
	[ValorUnitario] [decimal](18, 2) NULL,
	[ValorTotal] [decimal](18, 2) NULL,
	[PlazoEntrega] [nvarchar](100) NULL,
	[ShowPlazo] [int] NULL,
	[Garantia] [nvarchar](100) NULL,
	[ShowGarantia] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Quotations] Fecha de script: 28/04/2026 7:53:51 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Quotations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Numero] [nvarchar](50) NOT NULL,
	[Fecha] [nvarchar](50) NULL,
	[IdEdificio] [int] NULL,
	[Asunto] [nvarchar](255) NULL,
	[CordialSaludo] [nvarchar](max) NULL,
	[HeaderPropuesta] [nvarchar](max) NULL,
	[DescripcionObra] [nvarchar](max) NULL,
	[ValorObra] [decimal](18, 2) NULL,
	[PorcentajeUtilidad] [decimal](18, 2) NULL,
	[Utilidad] [decimal](18, 2) NULL,
	[PorcentajeIva] [decimal](18, 2) NULL,
	[IvaUtilidad] [decimal](18, 2) NULL,
	[Total] [decimal](18, 2) NULL,
	[NotaPie] [nvarchar](max) NULL,
	[FirmaNombre] [nvarchar](255) NULL,
	[FirmaCargo] [nvarchar](255) NULL,
	[FirmaCelular] [nvarchar](20) NULL,
	[PlazoEntrega] [nvarchar](100) NULL,
	[ShowPlazo] [int] NOT NULL,
	[Garantia] [nvarchar](100) NULL,
	[ShowGarantia] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Numero] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Objeto: Table [dbo].[Users] Fecha de script: 28/04/2026 7:53:51 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Buildings] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Companies] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Employees] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[QuotationItems] ADD  DEFAULT ((0)) FOR [ShowPlazo]
GO
ALTER TABLE [dbo].[QuotationItems] ADD  DEFAULT ((0)) FOR [ShowGarantia]
GO
ALTER TABLE [dbo].[Quotations] ADD  CONSTRAINT [DF_Quotations_ShowPlazo]  DEFAULT ((0)) FOR [ShowPlazo]
GO
ALTER TABLE [dbo].[Quotations] ADD  CONSTRAINT [DF_Quotations_ShowGarantia]  DEFAULT ((0)) FOR [ShowGarantia]
GO
ALTER TABLE [dbo].[Buildings]  WITH CHECK ADD FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([IdEmpresa])
GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD FOREIGN KEY([BuildingId])
REFERENCES [dbo].[Buildings] ([IdEdificio])
GO
ALTER TABLE [dbo].[QuotationItems]  WITH CHECK ADD FOREIGN KEY([IdCotizacion])
REFERENCES [dbo].[Quotations] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Quotations]  WITH CHECK ADD FOREIGN KEY([IdEdificio])
REFERENCES [dbo].[Buildings] ([IdEdificio])
GO
