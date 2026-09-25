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

BEGIN TRANSACTION;
DECLARE @var3 nvarchar(max);
SELECT @var3 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Chamados]') AND [c].[name] = N'DataAtualizacao');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Chamados] DROP CONSTRAINT ' + @var3 + ';');
ALTER TABLE [Chamados] DROP COLUMN [DataAtualizacao];

DECLARE @var4 nvarchar(max);
SELECT @var4 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Chamados]') AND [c].[name] = N'Titulo');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Chamados] DROP CONSTRAINT ' + @var4 + ';');
ALTER TABLE [Chamados] ALTER COLUMN [Titulo] nvarchar(max) NOT NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260923230815_AtualizacaoModelChamado', N'10.0.12');

COMMIT;
GO

BEGIN TRANSACTION;
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260923231218_TesteDbContextFactory', N'10.0.12');

COMMIT;
GO

BEGIN TRANSACTION;
DECLARE @var5 nvarchar(max);
SELECT @var5 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Interacoes]') AND [c].[name] = N'DataCriacao');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Interacoes] DROP CONSTRAINT ' + @var5 + ';');
ALTER TABLE [Interacoes] DROP COLUMN [DataCriacao];

DECLARE @var6 nvarchar(max);
SELECT @var6 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Chamados]') AND [c].[name] = N'DataCriacao');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Chamados] DROP CONSTRAINT ' + @var6 + ';');
ALTER TABLE [Chamados] DROP COLUMN [DataCriacao];

ALTER TABLE [Interacoes] ADD [Autor] nvarchar(200) NOT NULL DEFAULT N'';

ALTER TABLE [Interacoes] ADD [DataRegistro] datetimeoffset NOT NULL DEFAULT '0001-01-01T00:00:00.0000000+00:00';

DECLARE @var7 nvarchar(max);
SELECT @var7 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Chamados]') AND [c].[name] = N'Titulo');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Chamados] DROP CONSTRAINT ' + @var7 + ';');
ALTER TABLE [Chamados] ALTER COLUMN [Titulo] nvarchar(200) NOT NULL;

ALTER TABLE [Chamados] ADD [DataAbertura] datetimeoffset NOT NULL DEFAULT '0001-01-01T00:00:00.0000000+00:00';

ALTER TABLE [Chamados] ADD [DataFechamento] datetimeoffset NULL;

ALTER TABLE [Chamados] ADD [SolicitanteNome] nvarchar(200) NOT NULL DEFAULT N'';

ALTER TABLE [Chamados] ADD [Solucao] nvarchar(max) NULL;

ALTER TABLE [Chamados] ADD CONSTRAINT [CK_Chamados_Prioridade] CHECK ([Prioridade] IN (1, 2, 3));

ALTER TABLE [Chamados] ADD CONSTRAINT [CK_Chamados_Status] CHECK ([Status] IN (1, 2, 3));

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260925123252_AtualizacaoEstrutura', N'10.0.12');

COMMIT;
GO

BEGIN TRANSACTION;
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260925123450_AtualizacaoEstrutura1', N'10.0.12');

COMMIT;
GO

BEGIN TRANSACTION;
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260925123909_AtualizacaoEstrutura2', N'10.0.12');

COMMIT;
GO

