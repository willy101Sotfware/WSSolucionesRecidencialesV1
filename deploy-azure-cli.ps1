# =============================================
# Script de despliegue en Azure App Service
# Ejecutar en PowerShell como Administrador
# =============================================

# 1. Login en Azure (se abrirá navegador)
az login

# 2. Crear Grupo de Recursos (si no existe)
$resourceGroup = "rg-solucionesrecidenciales"
$location = "eastus"

az group create `
    --name $resourceGroup `
    --location $location

# 3. Crear Plan de App Service (Tier Gratuito F1)
$appServicePlan = "plan-solucionesrecidenciales"

az appservice plan create `
    --name $appServicePlan `
    --resource-group $resourceGroup `
    --sku F1 `
    --is-linux false

# 4. Crear App Service
$appName = "wssolucionesrecidenciales-api"
$runtime = "DOTNET|10.0"

az webapp create `
    --name $appName `
    --resource-group $resourceGroup `
    --plan $appServicePlan `
    --runtime $runtime

# 5. Configurar Connection String en Azure
$connectionString = "Server=tcp:servidor1.database.windows.net,1433;Database=SolucionesRecidenciales;User Id=sqladmin;Password=TU_PASSWORD_AQUI;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;MultipleActiveResultSets=True;"

az webapp config connection-string set `
    --name $appName `
    --resource-group $resourceGroup `
    --connection-string-type SQLAzure `
    --settings DefaultConnection="$connectionString"

# 6. Habilitar Managed Identity (opcional, para Azure AD)
az webapp identity assign `
    --name $appName `
    --resource-group $resourceGroup

# 7. Publicar aplicación (Build + Zip Deploy)
# Primero publicar localmente
$publishPath = "./publish"
dotnet publish ./WSSolucionesRecidenciales/WSSolucionesRecidenciales.csproj -c Release -o $publishPath

# Crear zip
cd $publishPath
Compress-Archive -Path * -DestinationPath ../deploy.zip -Force
cd ..

# Deploy zip a Azure
az webapp deployment source config-zip `
    --resource-group $resourceGroup `
    --name $appName `
    --src ./deploy.zip

Write-Host "====================================="
Write-Host "Despliegue completado!"
Write-Host "URL: https://$appName.azurewebsites.net"
Write-Host "Swagger: https://$appName.azurewebsites.net/swagger"
Write-Host "====================================="
