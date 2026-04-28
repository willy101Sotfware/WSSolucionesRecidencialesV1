#!/bin/bash
# =============================================
# Script de despliegue en Azure App Service (Bash/Linux)
# =============================================

# Variables
RESOURCE_GROUP="rg-solucionesrecidenciales"
LOCATION="eastus"
APP_SERVICE_PLAN="plan-solucionesrecidenciales"
APP_NAME="wssolucionesrecidenciales-api"
RUNTIME="DOTNET|10.0"

# 1. Login en Azure
az login

# 2. Crear Grupo de Recursos
az group create \
    --name $RESOURCE_GROUP \
    --location $LOCATION

# 3. Crear Plan de App Service (Gratis F1)
az appservice plan create \
    --name $APP_SERVICE_PLAN \
    --resource-group $RESOURCE_GROUP \
    --sku F1 \
    --is-linux false

# 4. Crear App Service
az webapp create \
    --name $APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --plan $APP_SERVICE_PLAN \
    --runtime $RUNTIME

# 5. Configurar Connection String
CONNECTION_STRING="Server=tcp:servidor1.database.windows.net,1433;Database=SolucionesRecidenciales;User Id=sqladmin;Password=TU_PASSWORD_AQUI;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;MultipleActiveResultSets=True;"

az webapp config connection-string set \
    --name $APP_NAME \
    --resource-group $RESOURCE_GROUP \
    --connection-string-type SQLAzure \
    --settings DefaultConnection="$CONNECTION_STRING"

# 6. Publicar aplicación
dotnet publish ./WSSolucionesRecidenciales/WSSolucionesRecidenciales.csproj -c Release -o ./publish

# 7. Crear zip del publish
cd publish
zip -r ../deploy.zip .
cd ..

# 8. Deploy a Azure
az webapp deployment source config-zip \
    --resource-group $RESOURCE_GROUP \
    --name $APP_NAME \
    --src ./deploy.zip

echo "====================================="
echo "Despliegue completado!"
echo "URL: https://$APP_NAME.azurewebsites.net"
echo "Swagger: https://$APP_NAME.azurewebsites.net/swagger"
echo "====================================="
