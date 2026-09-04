IF DB_ID('dbEmpresa') IS NULL
BEGIN
    CREATE DATABASE dbEmpresa;
END;
GO

USE dbEmpresa;
GO

-- ATENÇÃO: os comandos abaixo apagam as tabelas do laboratório e seus dados.
IF OBJECT_ID('dbo.Incidente', 'U') IS NOT NULL DROP TABLE dbo.Incidente;
IF OBJECT_ID('dbo.Tarefa', 'U') IS NOT NULL DROP TABLE dbo.Tarefa;
IF OBJECT_ID('dbo.Funcionario', 'U') IS NOT NULL DROP TABLE dbo.Funcionario;
IF OBJECT_ID('dbo.Departamento', 'U') IS NOT NULL DROP TABLE dbo.Departamento;
GO

CREATE TABLE dbo.Departamento (
    Codigo INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Departamento PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Sigla VARCHAR(10) NOT NULL
);
GO

CREATE TABLE dbo.Funcionario (
    Codigo INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Funcionario PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Cargo VARCHAR(50) NOT NULL,
    DepartamentoId INT NOT NULL,
    CONSTRAINT FK_Funcionario_Departamento
        FOREIGN KEY (DepartamentoId) REFERENCES dbo.Departamento(Codigo)
);
GO

CREATE TABLE dbo.Tarefa (
    Codigo INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Tarefa PRIMARY KEY,
    Descricao VARCHAR(200) NOT NULL,
    DataPlanejada DATETIME NOT NULL,
    DataIniciada DATETIME NULL,
    DataFinalizada DATETIME NULL,
    DataCancelada DATETIME NULL,
    StatusTarefa VARCHAR(30) NOT NULL,
    Prazo VARCHAR(20) NOT NULL,
    FuncionarioId INT NOT NULL,
    CONSTRAINT FK_Tarefa_Funcionario
        FOREIGN KEY (FuncionarioId) REFERENCES dbo.Funcionario(Codigo)
);
GO

CREATE TABLE dbo.Incidente (
    Codigo INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Incidente PRIMARY KEY,
    DescricaoProblema VARCHAR(250) NOT NULL,
    DataIncidente DATETIME NOT NULL,
    Solucao VARCHAR(250) NULL,
    Resolvido VARCHAR(3) NOT NULL
);
GO

INSERT INTO dbo.Departamento (Nome, Sigla) VALUES
('Tecnologia da Informacao', 'TI'),
('Recursos Humanos', 'RH'),
('Financeiro', 'FIN');

INSERT INTO dbo.Funcionario (Nome, Cargo, DepartamentoId) VALUES
('Carlos Silva', 'Desenvolvedor Senior', 1),
('Ana Oliveira', 'Analista de QA', 1),
('Roberto Santos', 'Gerente de RH', 2);

INSERT INTO dbo.Tarefa
(Descricao, DataPlanejada, DataIniciada, DataFinalizada, DataCancelada, StatusTarefa, Prazo, FuncionarioId)
VALUES
('Criar tela de Login', '2026-08-10', '2026-08-01', NULL, NULL, 'Em Andamento', 'Em dia', 1),
('Homologar Release 1.0', '2026-08-05', NULL, NULL, NULL, 'Pendente', 'Em atraso', 2);
GO
