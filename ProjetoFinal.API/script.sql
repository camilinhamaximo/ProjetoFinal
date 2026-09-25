IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Categorias] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(200) NOT NULL,
    CONSTRAINT [PK_Categorias] PRIMARY KEY ([Id])
);

CREATE TABLE [Chamados] (
    [Id] int NOT NULL IDENTITY,
    [Titulo] nvarchar(200) NOT NULL,
    [Descricao] nvarchar(max) NOT NULL,
    [Prioridade] int NOT NULL,
    [Status] int NOT NULL,
    [SolicitanteNome] nvarchar(200) NOT NULL,
    [DataAbertura] datetime2 NOT NULL,
    [DataFechamento] datetime2 NULL,
    [Solucao] nvarchar(max) NULL,
    [CategoriaId] int NOT NULL,
    CONSTRAINT [PK_Chamados] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Chamados_Categorias_CategoriaId] FOREIGN KEY ([CategoriaId]) REFERENCES [Categorias] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Interacoes] (
    [Id] int NOT NULL IDENTITY,
    [ChamadoId] int NOT NULL,
    [Autor] nvarchar(200) NOT NULL,
    [Mensagem] nvarchar(max) NOT NULL,
    [DataRegistro] datetime2 NOT NULL,
    CONSTRAINT [PK_Interacoes] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Interacoes_Chamados_ChamadoId] FOREIGN KEY ([ChamadoId]) REFERENCES [Chamados] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Chamados_CategoriaId] ON [Chamados] ([CategoriaId]);

CREATE INDEX [IX_Interacoes_ChamadoId] ON [Interacoes] ([ChamadoId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260922002326_InitialCreate', N'10.0.12');

COMMIT;
GO

BEGIN TRANSACTION;
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260922193731_AjusteModels', N'10.0.12');

COMMIT;
GO

BEGIN TRANSACTION;
DECLARE @var nvarchar(max);
SELECT @var = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Interacoes]') AND [c].[name] = N'Autor');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Interacoes] DROP CONSTRAINT ' + @var + ';');
ALTER TABLE [Interacoes] DROP COLUMN [Autor];

DECLARE @var1 nvarchar(max);
SELECT @var1 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Chamados]') AND [c].[name] = N'SolicitanteNome');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Chamados] DROP CONSTRAINT ' + @var1 + ';');
ALTER TABLE [Chamados] DROP COLUMN [SolicitanteNome];

DECLARE @var2 nvarchar(max);
SELECT @var2 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Chamados]') AND [c].[name] = N'Solucao');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Chamados] DROP CONSTRAINT ' + @var2 + ';');
ALTER TABLE [Chamados] DROP COLUMN [Solucao];

EXEC sp_rename N'[Interacoes].[DataRegistro]', N'DataCriacao', 'COLUMN';

EXEC sp_rename N'[Chamados].[DataFechamento]', N'DataAtualizacao', 'COLUMN';

EXEC sp_rename N'[Chamados].[DataAbertura]', N'DataCriacao', 'COLUMN';

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260923150111_CreateInicial', N'10.0.12');

COMMIT;
GO

