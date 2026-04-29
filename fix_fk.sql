-- Primero ejecuta ESTE query para DESHABILITAR la comprobación de FK
EXEC sp_msforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT all"

-- Luego intenta inserting
-- Luego si quieres vuelve a habilitar
EXEC sp_msforeachtable "ALTER TABLE ? CHECK CONSTRAINT all"