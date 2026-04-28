# =============================================
# Script de redeploy en Azure App Service
# Ejecutar después de cambios en el código
# =============================================

Write-Host "Iniciando redeploy..." -ForegroundColor Green

# 1. Publicar aplicación
Write-Host "Publicando aplicación..." -ForegroundColor Yellow
dotnet publish ./WSSolucionesRecidenciales/WSSolucionesRecidenciales.csproj -c Release -o ./publish

if ($LASTEXITCODE -ne 0) {
    Write-Host "Error al publicar la aplicación" -ForegroundColor Red
    exit 1
}

# 2. Crear zip
Write-Host "Creando archivo zip..." -ForegroundColor Yellow
Compress-Archive -Path ./publish/* -DestinationPath ./deploy.zip -Force

# 3. Deploy a Azure
Write-Host "Desplegando a Azure..." -ForegroundColor Yellow
az webapp deployment source config-zip --resource-group wssolucionesrecidenciales-api_group --name wssolucionesrecidenciales-api --src ./deploy.zip

if ($LASTEXITCODE -ne 0) {
    Write-Host "Error al desplegar a Azure" -ForegroundColor Red
    exit 1
}

Write-Host "=====================================" -ForegroundColor Green
Write-Host "Redeploy completado exitosamente!" -ForegroundColor Green
Write-Host "URL: https://wssolucionesrecidenciales-api-a6hvbaaxa0b8a9dn.mexicocentral-01.azurewebsites.net" -ForegroundColor Green
Write-Host "Swagger: https://wssolucionesrecidenciales-api-a6hvbaaxa0b8a9dn.mexicocentral-01.azurewebsites.net/swagger" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green
