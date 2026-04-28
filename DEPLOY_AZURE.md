# Despliegue en Azure App Service

Guía paso a paso para desplegar la API .NET 10 en Azure App Service con Azure SQL Database.

## 📋 Requisitos Previos

- [x] Azure App Service (plan Gratuito F1 disponible)
- [x] Azure SQL Database configurada
- [x] Visual Studio 2022 o VS Code
- [x] Azure CLI (opcional)

## 🔧 Paso 1: Instalar Paquete necesario para Azure AD

Abrir Package Manager Console y ejecutar:

```powershell
Install-Package Microsoft.Data.SqlClient -Version 6.0.1
```

O con .NET CLI:
```bash
dotnet add package Microsoft.Data.SqlClient --version 6.0.1
```

## 🚀 Paso 2: Despliegue desde Visual Studio

### Opción A: Publish Profile (Más fácil)

1. **Click derecho en proyecto** `WSSolucionesRecidenciales` → **Publicar**

2. **Seleccionar destino:**
   - Azure
   - Azure App Service (Windows)

3. **Iniciar sesión en Azure**

4. **Crear nuevo App Service** o seleccionar existente:
   - **Nombre:** `wssolucionesrecidenciales-api`
   - **Suscripción:** (tu suscripción gratuita)
   - **Grupo de recursos:** Crear nuevo → `rg-solucionesrecidenciales`
   - **Plan de App Service:**
     - Crear nuevo → `plan-solucionesrecidenciales`
     - **Ubicación:** East US / West US
     - **Tamaño:** **Free F1** (0.5 GB RAM, 1 GB storage)

5. **Finalizar** → Se descargará perfil de publicación automáticamente

6. **Click en "Publicar"**

### Opción B: GitHub Actions (CI/CD Automático)

1. En Azure Portal:
   - App Service → Deployment Center
   - Seleccionar GitHub como origen
   - Autorizar y seleccionar repositorio `WSSolucionesRecidencialesV1`
   - Seleccionar rama `main`
   - Framework: `.NET 10`

2. Azure creará automáticamente archivo de workflow en `.github/workflows/`

## 🔐 Paso 3: Configurar Identidad Administrada (Azure AD)

Para usar "Active Directory Default", necesitas Managed Identity:

### En Azure Portal:

1. Ir a tu **App Service**

2. **Identidad** (en el menú lateral) → **Sistema asignado**

3. **Estado:** Activado → **Guardar**

4. Copiar el **Object ID** que se genera

### Dar permisos a SQL Database:

1. Ir a **Azure SQL Database** → **servidor1**

2. **Microsoft Entra ID** (Azure Active Directory)

3. **Establecer administrador** → Seleccionar tu usuario/app

4. Conectar a SQL Database con Azure Data Studio o SSMS con Azure AD

5. Ejecutar SQL:

```sql
CREATE USER [nombre-de-tu-app-service] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [nombre-de-tu-app-service];
ALTER ROLE db_datawriter ADD MEMBER [nombre-de-tu-app-service];
ALTER ROLE db_ddladmin ADD MEMBER [nombre-de-tu-app-service];
```

## ⚙️ Paso 4: Configurar Variables de Entorno (Recomendado)

En lugar de hardcodear el connection string, usa Azure App Settings:

### En Azure Portal:

1. App Service → **Configuración** → **Configuración de la aplicación**

2. **Nueva configuración de la aplicación:**
   - **Nombre:** `ConnectionStrings__DefaultConnection`
   - **Valor:** `Server=tcp:servidor1.database.windows.net,1433;Initial Catalog=SolucionesRecidenciales;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=Active Directory Managed Identity;MultipleActiveResultSets=True;`

3. **Guardar** → App Service se reiniciará automáticamente

## 🔄 Alternativa: SQL Authentication (Más simple)

Si Azure AD es complejo, puedes usar usuario/contraseña SQL:

### Cambiar connection string en Azure:
```
Server=tcp:servidor1.database.windows.net,1433;Database=SolucionesRecidenciales;User ID=tu-usuario-sql;Password=tu-password;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```

### Configurar usuario en SQL Database:
```sql
CREATE USER apiuser WITH PASSWORD = 'StrongPassword123!';
ALTER ROLE db_owner ADD MEMBER apiuser;
```

## 🌐 Paso 5: Verificar Despliegue

1. Abrir URL de App Service:
   ```
   https://wssolucionesrecidenciales-api.azurewebsites.net/swagger
   ```

2. Probar endpoint:
   ```bash
   curl https://wssolucionesrecidenciales-api.azurewebsites.net/api/Buildings
   ```

## 🔍 Troubleshooting

### Error: "Login failed for user '<token-identified principal>'"
- Verificar que Managed Identity esté activada
- Verificar permisos SQL otorgados correctamente
- Esperar 5-10 minutos después de cambios de permisos

### Error: "A network-related or instance-specific error"
- Verificar que Azure SQL Database permita acceso desde Azure Services:
  - SQL Server → Seguridad de redes → Permitir servicios de Azure: **SÍ**

### App no inicia:
- Verificar logs: App Service → Supervisión → Log stream
- Verificar que .NET 10 esté disponible en la región

## 📊 Monitoreo

En Azure Portal:
- **App Service** → Supervisión → Log Stream (logs en tiempo real)
- **Application Insights** (opcional, agregar para métricas detalladas)
- **Azure SQL Database** → Supervisión → Métricas

## 💰 Costos (Tier Gratuito)

| Servicio | Tier | Costo |
|----------|------|-------|
| App Service | F1 (Shared) | **Gratis** |
| Azure SQL | Serverless (5 DTU) | ~$5-10/mes o Gratis con Azure for Students |
| Storage | 1 GB incluido | Gratis |

**Total: Gratis a ~$10/mes dependiendo del uso**

## ✅ Checklist Pre-Despliegue

- [ ] Connection string actualizado para Azure SQL
- [ ] Microsoft.Data.SqlClient instalado
- [ ] Azure App Service creado (F1 tier)
- [ ] Managed Identity activada (si usas Azure AD)
- [ ] Permisos SQL otorgados
- [ ] Variables de entorno configuradas en Azure
- [ ] Swagger funciona localmente
- [ ] Base de datos accesible desde Azure

---

**¿Problemas?** Revisar logs en: Azure Portal → App Service → Supervisión → Log stream
