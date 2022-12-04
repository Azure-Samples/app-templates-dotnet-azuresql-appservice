IF NOT EXISTS(SELECT 1 from sys.database_principals where [NAME] = '##user##')
BEGIN
    --Both of these require a AAD User, not a Service principal, to function. See https://github.com/MicrosoftDocs/sql-docs/issues/2323 for more information
    --CREATE USER [] FROM EXTERNAL PROVIDER WITH OBJECT_ID = ''
    --CREATE USER [<Azure_AD_principal_name>] FROM EXTERNAL PROVIDER

    --Not ideal but allows Managed Identity Users to be created by a Service Prinicpal in a pipeline
    CREATE USER [##user##] WITH default_schema=[dbo], SID=##mi_sid##, TYPE=E;
END

EXEC sp_addrolemember N'db_owner', N'##user##'
