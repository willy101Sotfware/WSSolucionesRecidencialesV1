param(
    [string]$ResourceGroup = "solucionesGroup",
    [string]$Location = "brazilsouth",
    [string]$PlanName = "plan-soluciones-api",
    [string]$WebAppName = "",
    [string]$SqlServer = "servidor-sl",
    [string]$SqlDatabase = "SOLUCIONES_RECIDENCIALES.DB",
    [string]$SqlUser = "SolucionesRecidencialesV1",
    [Parameter(Mandatory = $true)]
    [string]$SqlPassword
)

$ErrorActionPreference = "Stop"

function Assert-LastExitCode {
    param([string]$StepName)
    if ($LASTEXITCODE -ne 0) {
        throw "Fallo en paso: $StepName (exit code $LASTEXITCODE)"
    }
}

if ([string]::IsNullOrWhiteSpace($WebAppName)) {
    $suffix = Get-Random -Minimum 10000 -Maximum 99999
    $WebAppName = "soluciones-api-$suffix"
}

Write-Host "==> Validando herramientas..."
dotnet --version | Out-Null
Assert-LastExitCode "dotnet --version"
az --version | Out-Null
Assert-LastExitCode "az --version"

Write-Host "==> Validando sesion de Azure..."
az account show | Out-Null
Assert-LastExitCode "az account show"

$conn = "Server=tcp:$SqlServer.database.windows.net,1433;Initial Catalog=$SqlDatabase;Persist Security Info=False;User ID=$SqlUser;Password=$SqlPassword;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

Write-Host "==> Creando regla firewall SQL (AllowAzureServices)..."
az sql server firewall-rule create `
    --resource-group $ResourceGroup `
    --server $SqlServer `
    --name "AllowAzureServices" `
    --start-ip-address 0.0.0.0 `
    --end-ip-address 0.0.0.0 | Out-Null
Assert-LastExitCode "az sql server firewall-rule create"

Write-Host "==> Creando App Service Plan (Linux B1)..."
az appservice plan create `
    --name $PlanName `
    --resource-group $ResourceGroup `
    --location $Location `
    --sku B1 `
    --is-linux | Out-Null
Assert-LastExitCode "az appservice plan create"

Write-Host "==> Creando Web App..."
az webapp create `
    --resource-group $ResourceGroup `
    --plan $PlanName `
    --name $WebAppName `
    --runtime "DOTNETCORE:8.0" | Out-Null
Assert-LastExitCode "az webapp create"

Write-Host "==> Configurando ConnectionStrings__DefaultConnection..."
az webapp config appsettings set `
    --resource-group $ResourceGroup `
    --name $WebAppName `
    --settings "ConnectionStrings__DefaultConnection=$conn" | Out-Null
Assert-LastExitCode "az webapp config appsettings set"

Write-Host "==> Publicando API..."
if (Test-Path ".\publish") { Remove-Item ".\publish" -Recurse -Force }
dotnet clean "WSSolucionesRecidenciales/WSSolucionesRecidenciales.csproj" -c Release
Assert-LastExitCode "dotnet clean"
dotnet publish "WSSolucionesRecidenciales/WSSolucionesRecidenciales.csproj" -c Release -f net8.0 -o ".\publish"
Assert-LastExitCode "dotnet publish"

if (Test-Path ".\api.zip") { Remove-Item ".\api.zip" -Force }
Compress-Archive -Path ".\publish\*" -DestinationPath ".\api.zip" -Force

Write-Host "==> Desplegando ZIP al Web App..."
az webapp deployment source config-zip `
    --resource-group $ResourceGroup `
    --name $WebAppName `
    --src ".\api.zip" | Out-Null
Assert-LastExitCode "az webapp deployment source config-zip"

Write-Host "==> Generando migracion inicial si no existe..."
$migrationExists = Get-ChildItem -Path ".\Infrastructure\Persistence\Migrations" -Filter "*.cs" -ErrorAction SilentlyContinue
if (-not $migrationExists) {
    dotnet ef migrations add InitialAzureSql `
        --project "Infrastructure/Infrastructure.csproj" `
        --startup-project "WSSolucionesRecidenciales/WSSolucionesRecidenciales.csproj" `
        --output-dir "Persistence/Migrations"
    Assert-LastExitCode "dotnet ef migrations add"
}

Write-Host "==> Aplicando migraciones en Azure SQL..."
dotnet ef database update `
    --project "Infrastructure/Infrastructure.csproj" `
    --startup-project "WSSolucionesRecidenciales/WSSolucionesRecidenciales.csproj" `
    --connection $conn
Assert-LastExitCode "dotnet ef database update"

$url = "https://$WebAppName.azurewebsites.net/swagger"
Write-Host ""
Write-Host "Despliegue completado."
Write-Host "Swagger: $url"
