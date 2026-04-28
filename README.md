# WSSolucionesRecidenciales API

API REST completa desarrollada en **.NET 10** para la gestión de soluciones recidenciales. Implementa **Clean Architecture**, **CQRS simplificado** con MediatR, y **Entity Framework Core**.

## 🏗️ Arquitectura

El proyecto sigue los principios de **Clean Architecture** con las siguientes capas:

```
┌─────────────────────────────────────┐
│           WebApi                    │  ← Controllers, DI Configuration
│    (WSSolucionesRecidenciales)      │
├─────────────────────────────────────┤
│           Application               │  ← DTOs, Handlers CQRS, Mappings
│    (CQRS + MediatR + AutoMapper)    │
├─────────────────────────────────────┤
│           Infrastructure            │  ← EF Core, DbContext, Configurations
│    (SQL Server + EF Core)           │
├─────────────────────────────────────┤
│           Domain                    │  ← Entidades puras
│    (Core Business Logic)           │
└─────────────────────────────────────┘
```

## 🚀 Tecnologías

- **.NET 10**
- **Entity Framework Core 10** (preview)
- **MediatR 12** (CQRS simplificado)
- **AutoMapper** (Mapeo de objetos)
- **SQL Server**
- **Swagger/OpenAPI**

## 📦 Entidades

### Core Entities
| Entidad | Descripción | Relaciones |
|---------|-------------|------------|
| **Companies** | Empresas/constructoras | 1:N → Buildings |
| **Buildings** | Edificios/propiedades | N:1 → Company, 1:N → Employees, 1:N → Quotations |
| **Employees** | Empleados/operarios | N:1 → Building |
| **Quotations** | Cotizaciones | N:1 → Building, 1:N → QuotationItems |
| **QuotationItems** | Items de cotización | N:1 → Quotation (cascade delete) |
| **Users** | Usuarios del sistema | - |

## 🔗 Endpoints API

### Buildings
```
GET    /api/Buildings              → Listar edificios
GET    /api/Buildings/{id}         → Obtener edificio por ID
POST   /api/Buildings              → Crear edificio
PUT    /api/Buildings/{id}         → Actualizar edificio
DELETE /api/Buildings/{id}         → Eliminar edificio
```

### Companies
```
GET    /api/Companies              → Listar empresas
GET    /api/Companies/{id}         → Obtener empresa por ID
POST   /api/Companies              → Crear empresa
PUT    /api/Companies/{id}         → Actualizar empresa
DELETE /api/Companies/{id}         → Eliminar empresa
```

### Employees
```
GET    /api/Employees              → Listar empleados
GET    /api/Employees/{id}         → Obtener empleado por ID
POST   /api/Employees              → Crear empleado
PUT    /api/Employees/{id}         → Actualizar empleado
DELETE /api/Employees/{id}         → Eliminar empleado
```

### Quotations
```
GET    /api/Quotations              → Listar cotizaciones
GET    /api/Quotations/{id}         → Obtener cotización por ID
POST   /api/Quotations              → Crear cotización
PUT    /api/Quotations/{id}         → Actualizar cotización
DELETE /api/Quotations/{id}         → Eliminar cotización
```

### QuotationItems
```
GET    /api/QuotationItems              → Listar items
GET    /api/QuotationItems/{id}         → Obtener item por ID
GET    /api/QuotationItems/by-quotation/{quotationId} → Items por cotización
POST   /api/QuotationItems              → Crear item
PUT    /api/QuotationItems/{id}         → Actualizar item
DELETE /api/QuotationItems/{id}         → Eliminar item
```

### Users
```
GET    /api/Users              → Listar usuarios
GET    /api/Users/{id}         → Obtener usuario por ID
POST   /api/Users              → Crear usuario
PUT    /api/Users/{id}         → Actualizar usuario
DELETE /api/Users/{id}         → Eliminar usuario
```

## ⚙️ Configuración

### Connection String
Configurar en `WSSolucionesRecidenciales/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=DESKTOP-71B8376\\SQLEXPRESS01;Database=SolucionesRecidenciales;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

## 🧠 CQRS Simplificado

Cada entidad tiene un **único Handler** que contiene todos los comandos y queries (Vertical Slice):

```csharp
// Application/Features/Buildings/BuildingsHandler.cs
public class BuildingsHandler :
    IRequestHandler<GetAllBuildingsQuery, List<BuildingResponse>>,
    IRequestHandler<GetBuildingByIdQuery, BuildingResponse?>,
    IRequestHandler<CreateBuildingCommand, int>,
    IRequestHandler<UpdateBuildingCommand, bool>,
    IRequestHandler<DeleteBuildingCommand, bool>
{
    // Implementación...
}
```

## 🗄️ Base de Datos

### Diagrama de Relaciones

```
┌─────────────┐       ┌─────────────┐       ┌─────────────┐
│  Companies  │ 1:N   │  Buildings  │ 1:N   │  Employees  │
│  IdEmpresa  │──────→│  IdEdificio │←──────│  IdEmpleado │
└─────────────┘       └─────────────┘       └─────────────┘
                              │
                              │ 1:N
                              ↓
                       ┌─────────────┐       ┌─────────────┐
                       │  Quotations │ 1:N   │QuotationItems│
                       │     Id      │──────→│     Id      │
                       └─────────────┘       └─────────────┘
```

### Configuración EF Core

Las configuraciones de entidades están en:
```
Infrastructure/Persistence/Configurations/
├── BuildingConfiguration.cs
├── CompanyConfiguration.cs
├── EmployeeConfiguration.cs
├── QuotationConfiguration.cs
├── QuotationItemConfiguration.cs
└── UserConfiguration.cs
```

## 🚀 Iniciar Proyecto

### Requisitos
- .NET 10 SDK
- SQL Server

### Pasos

1. **Clonar repositorio**
```bash
git clone https://github.com/willy101Sotfware/WSSolucionesRecidencialesV1.git
cd WSSolucionesRecidencialesV1
```

2. **Restaurar paquetes**
```bash
dotnet restore
```

3. **Configurar base de datos**
Actualizar `appsettings.json` con tu connection string.

4. **Ejecutar**
```bash
dotnet run --project WSSolucionesRecidenciales
```

5. **Abrir Swagger**
```
https://localhost:{port}/swagger
```

## 📁 Estructura del Proyecto

```
ProyectoSolucionesRecidencialesV2/
├── Domain/
│   └── Entities/
│       ├── Building.cs
│       ├── Company.cs
│       ├── Employee.cs
│       ├── Quotation.cs
│       ├── QuotationItem.cs
│       └── User.cs
│
├── Application/
│   ├── DTOs/
│   │   ├── BuildingDtos.cs
│   │   ├── CompanyDtos.cs
│   │   ├── EmployeeDtos.cs
│   │   ├── QuotationDtos.cs
│   │   ├── QuotationItemDtos.cs
│   │   └── UserDtos.cs
│   ├── Features/
│   │   ├── Buildings/BuildingsHandler.cs
│   │   ├── Companies/CompaniesHandler.cs
│   │   ├── Employees/EmployeesHandler.cs
│   │   ├── Quotations/QuotationsHandler.cs
│   │   ├── QuotationItems/QuotationItemsHandler.cs
│   │   └── Users/UsersHandler.cs
│   ├── Interfaces/IApplicationDbContext.cs
│   ├── Mappings/MappingProfile.cs
│   └── ServiceRegistration.cs
│
├── Infrastructure/
│   ├── Persistence/
│   │   ├── ApplicationDbContext.cs
│   │   └── Configurations/
│   │       ├── BuildingConfiguration.cs
│   │       ├── CompanyConfiguration.cs
│   │       ├── EmployeeConfiguration.cs
│   │       ├── QuotationConfiguration.cs
│   │       ├── QuotationItemConfiguration.cs
│   │       └── UserConfiguration.cs
│   └── ServiceRegistration.cs
│
└── WSSolucionesRecidenciales/
    ├── Controllers/
    │   ├── BuildingsController.cs
    │   ├── CompaniesController.cs
    │   ├── EmployeesController.cs
    │   ├── QuotationsController.cs
    │   ├── QuotationItemsController.cs
    │   └── UsersController.cs
    ├── appsettings.json
    └── Program.cs
```

## 🔧 Principios SOLID Aplicados

- **S**ingle Responsibility: Cada clase tiene una única responsabilidad
- **O**pen/Closed: Extensiones mediante nuevos handlers sin modificar código existente
- **L**iskov Substitution: Implementaciones intercambiables via interfaces
- **I**nterface Segregation: IApplicationDbContext expone solo lo necesario
- **D**ependency Inversion: Dependencias inyectadas, no acopladas

## 📄 Licencia    Willian ruiz z     

Proyecto privado - WSSolucionesRecidenciales

---

**Desarrollado con .NET 10 + Clean Architecture + CQRS**
