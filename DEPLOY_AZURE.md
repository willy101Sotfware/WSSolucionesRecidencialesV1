# Despliegue de API + SQL Azure

## 1) Requisitos

- Tener `az` (Azure CLI) instalado y autenticado: `az login`
- Tener `.NET SDK` instalado
- Estar en la raiz del proyecto

## 2) Variables sugeridas

```bash
RG="solucionesGroup"
LOCATION="brazilsouth"
PLAN="plan-soluciones-api"
WEBAPP="soluciones-api-sl"
SQL_SERVER="servidor-sl"
SQL_DB="SOLUCIONES_RECIDENCIALES.DB"
SQL_USER="SolucionesRecidencialesV1"
SQL_PASS="TU_PASSWORD_AQUI"
```

## 3) Cadena de conexion

```bash
CONN="Server=tcp:${SQL_SERVER}.database.windows.net,1433;Initial Catalog=${SQL_DB};Persist Security Info=False;User ID=${SQL_USER};Password=${SQL_PASS};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
```

## 4) Habilitar acceso SQL desde Azure Services

```bash
az sql server firewall-rule create \
  --resource-group "$RG" \
  --server "$SQL_SERVER" \
  --name "AllowAzureServices" \
  --start-ip-address 0.0.0.0 \
  --end-ip-address 0.0.0.0
```

## 5) Crear App Service (si aun no existe)

```bash
az appservice plan create \
  --name "$PLAN" \
  --resource-group "$RG" \
  --location "$LOCATION" \
  --sku B1 \
  --is-linux

az webapp create \
  --resource-group "$RG" \
  --plan "$PLAN" \
  --name "$WEBAPP" \
  --runtime "DOTNETCORE:8.0"
```

> Nota: el proyecto quedo ajustado a `net8.0` para compatibilidad con App Service Linux.

## 6) Configurar variable de conexion en Web App

```bash
az webapp config appsettings set \
  --resource-group "$RG" \
  --name "$WEBAPP" \
  --settings ConnectionStrings__DefaultConnection="$CONN"
```

## 7) Publicar API

```bash
dotnet publish "WSSolucionesRecidenciales/WSSolucionesRecidenciales.csproj" -c Release -o ./publish
cd publish
zip -r ../api.zip .
cd ..

az webapp deployment source config-zip \
  --resource-group "$RG" \
  --name "$WEBAPP" \
  --src ./api.zip
```

## 8) Migraciones EF (primera vez)

```bash
dotnet ef migrations add InitialAzureSql \
  --project "Infrastructure/Infrastructure.csproj" \
  --startup-project "WSSolucionesRecidenciales/WSSolucionesRecidenciales.csproj" \
  --output-dir "Persistence/Migrations"
```

## 9) Aplicar migraciones a Azure SQL manualmente

```bash
dotnet ef database update \
  --project "Infrastructure/Infrastructure.csproj" \
  --startup-project "WSSolucionesRecidenciales/WSSolucionesRecidenciales.csproj" \
  --connection "$CONN"
```

## 10) Verificacion

- Probar Swagger: `https://<WEBAPP>.azurewebsites.net/swagger`
- Revisar logs:

```bash
az webapp log tail --resource-group "$RG" --name "$WEBAPP"
```

