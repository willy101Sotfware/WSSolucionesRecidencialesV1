-- =============================================
-- Script para crear usuario SQL y dar permisos
-- Ejecutar en Azure SQL Database como administrador
-- =============================================

-- 1. Crear login en el servidor SQL
CREATE LOGIN apiuser WITH PASSWORD = 'ApiUser2025!Secure';

-- 2. Usar la base de datos
USE SolucionesRecidenciales;

-- 3. Crear usuario en la base de datos vinculado al login
CREATE USER apiuser FOR LOGIN apiuser;

-- 4. Agregar a roles con permisos necesarios
ALTER ROLE db_datareader ADD MEMBER apiuser;
ALTER ROLE db_datawriter ADD MEMBER apiuser;
ALTER ROLE db_ddladmin ADD MEMBER apiuser;

-- 5. (Opcional) Verificar permisos
EXEC sp_addrolemember 'db_owner', 'apiuser';

PRINT 'Usuario apiuser creado exitosamente con permisos';
