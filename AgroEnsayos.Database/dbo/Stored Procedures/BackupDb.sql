CREATE PROCEDURE [dbo].[BackupDb]
AS

DBCC SHRINKDATABASE(AgroEnsayos)

DECLARE @path varchar(200) = 'C:\Backup\AgroEnsayos_' + CAST(CONVERT(date,GETDATE(),101) AS VARCHAR(10)) + '.bak'

BACKUP DATABASE AgroEnsayos TO DISK = @path WITH INIT




