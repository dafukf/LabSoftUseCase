USE dbTaskZero;
GO

IF OBJECT_ID('dbo.CentralDeCusto', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.CentralDeCusto
    (
        Codigo INT IDENTITY(1,1) NOT NULL
            CONSTRAINT PK_CentralDeCusto PRIMARY KEY,
        NomeCentral VARCHAR(250) NOT NULL,
        ValorMetaAnual DECIMAL(18,2) NOT NULL
            CONSTRAINT DF_CentralDeCusto_ValorMetaAnual DEFAULT (0)
    );
END;
GO
