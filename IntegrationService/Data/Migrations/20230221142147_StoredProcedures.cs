using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntegrationService.Data.Migrations
{
    public partial class StoredProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("USE [IntegrationService2]\r\nGO\r\n\r\n/****** Object:  Table [dbo].[Imports2]    Script Date: 23/1/2023 4:03:40 μμ ******/\r\nSET ANSI_NULLS ON\r\nGO\r\n\r\nSET QUOTED_IDENTIFIER ON\r\nGO\r\n\r\nCREATE TABLE [dbo].[Imports2](\r\n\t[Id] [uniqueidentifier] NOT NULL,\r\n\t[WarehouseID] [nvarchar](max) NOT NULL,\r\n\t[Sku] [nvarchar](max) NOT NULL,\r\n\t[ProductDescription] [nvarchar](max) NOT NULL,\r\n\t[Type] [nvarchar](max) NOT NULL,\r\n\t[Step] [nvarchar](max) NOT NULL,\r\n\t[Status] [nvarchar](max) NOT NULL,\r\n\t[ProductEnabled] [bit] NOT NULL,\r\n\t[ImportDate] [datetime2](7) NOT NULL,\r\n\t[CreatedOn] [datetime2](7) NOT NULL,\r\n\t[ModifiedOn] [datetime2](7) NOT NULL,\r\n\t[Enabled] [bit] NOT NULL,\r\n CONSTRAINT [PK_Imports2] PRIMARY KEY CLUSTERED \r\n(\r\n\t[Id] ASC\r\n)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]\r\n) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]\r\nGO\r\n\r\n\r\n");

            var InitDbSql = "USE [IntegrationService2]\r\n\r\nGO  \r\nCREATE OR ALTER PROCEDURE InitializeImports\r\n\r\nAS   \r\nBEGIN\r\n--GO\r\nIF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Imports2]') AND type in (N'U'))\r\n\r\ndelete from [dbo].[Imports2]\r\n--GO\r\nINSERT INTO [dbo].[Imports2]\r\n           ([Id]\r\n           ,[WarehouseID]\r\n           ,[Sku]\r\n           ,[ProductDescription]\r\n           ,[Type]\r\n           ,[Step]\r\n           ,[Status]\r\n           ,[ProductEnabled]\r\n           ,[ImportDate]\r\n           ,[CreatedOn]\r\n           ,[ModifiedOn]\r\n           ,[Enabled])\r\n\tSELECT  [Id]\r\n\t\t\t,[WarehouseID]\r\n           ,[Sku]\r\n           ,[ProductDescription]\r\n           ,[Type]\r\n           ,[Step]\r\n           ,[Status]\r\n           ,[ProductEnabled]\r\n           ,[ImportDate]\r\n           ,[CreatedOn]\r\n           ,[ModifiedOn]\r\n           ,[Enabled]\r\nFROM [dbo].[Imports]\r\nEND\r\nGO";
            migrationBuilder.Sql(InitDbSql);
            var FetchDiffs = "USE [IntegrationService2]\r\nGO\r\n\r\nSET ANSI_NULLS ON\r\nGO\r\nSET QUOTED_IDENTIFIER ON\r\nGO\r\n\r\nCREATE OR ALTER PROCEDURE [dbo].[FetchDifferences]  \r\nAS \r\n\tBEGIN\r\n\t\tSelect * from \r\n\t\t\t(select t1.Enabled,t1.ProductDescription, t1.ProductEnabled, t1.Sku, t1.Status,t1.Step,t1.WarehouseID,t1.Type from Imports as t1\r\n\t\t\tExcept\r\n\t\t\tSelect t2.Enabled,t2.ProductDescription, t2.ProductEnabled, t2.Sku, t2.Status,t2.Step,t2.WarehouseID,t2.Type from Imports2 as t2) as OutPutVar\r\nEND\t";
            migrationBuilder.Sql(FetchDiffs);
            var DeleteImports = "USE [IntegrationService2]\r\nGO\r\n\r\n/****** Object:  StoredProcedure [dbo].[DeleteImports]    Script Date: 9/12/2022 4:08:26 μμ ******/\r\nSET ANSI_NULLS ON\r\nGO\r\n\r\nSET QUOTED_IDENTIFIER ON\r\nGO\r\n\r\nCreate OR ALTER PROCEDURE [dbo].[DeleteImports] \r\n\r\nAS   \r\ndelete from Imports\r\nGO";
            migrationBuilder.Sql(DeleteImports);
            var GetDifferencesFunction = "USE [IntegrationService2]\r\nGO\r\nSET ANSI_NULLS ON\r\nGO\r\nSET QUOTED_IDENTIFIER ON\r\nGO\r\nCreate or alter function [dbo].[GetDiffs]()      \r\nreturns table      \r\nas\r\nreturn Select * from\r\n            (select distinct t1.Sku,t1.Enabled,t1.ProductDescription, t1.ProductEnabled,  t1.Status,t1.Step,t1.WarehouseID,t1.Type from Imports as t1\r\n            Except\r\n            Select  distinct t2.Sku,t2.Enabled,t2.ProductDescription, t2.ProductEnabled, t2.Status,t2.Step,t2.WarehouseID,t2.Type from Imports2 as t2) as outputVar\r\nGO";
            migrationBuilder.Sql(GetDifferencesFunction);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            var InitDbSql = "DROP PROCEDURE [dbo].[InitializeImports]";
            migrationBuilder.Sql(InitDbSql);

            var FetchDiffs = "DROP PROCEDURE [dbo].[FetchDifferences]";
            migrationBuilder.Sql(FetchDiffs);

            var DeleteImports = "DROP PROCEDURE [dbo].[DeleteImports]";
            migrationBuilder.Sql(DeleteImports);
            var DeleteGetDiffsFunction = "DROP Function [dbo].[GetDiffs]";
            migrationBuilder.Sql(DeleteGetDiffsFunction);
            migrationBuilder.Sql("USE [IntegrationService2]\r\nGO\r\n\r\n/****** Object:  Table [dbo].[Imports2]    Script Date: 23/1/2023 4:13:06 μμ ******/\r\nIF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Imports2]') AND type in (N'U'))\r\nDROP TABLE [dbo].[Imports2]\r\nGO\r\n");

        }
    }
}
