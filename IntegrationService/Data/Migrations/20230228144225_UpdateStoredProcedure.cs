using IntegrationService.Models.Imports;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntegrationService.Data.Migrations
{
    public partial class UpdateStoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("USE [IntegrationService2]\r\nGO\r\n\r\n/****** Object:  Table [dbo].[Imports2]    Script Date: 23/1/2023 4:13:06 μμ ******/\r\nIF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Imports2]') AND type in (N'U'))\r\nDROP TABLE [dbo].[Imports2]\r\nGO\r\n");
            migrationBuilder.Sql("\t\t\tUSE [IntegrationService2]\r\nGO\r\n\r\n/****** Object:  Table [dbo].[Imports2]    Script Date: 28/2/2023 4:48:46 μμ ******/\r\nSET ANSI_NULLS ON\r\nGO\r\n\r\nSET QUOTED_IDENTIFIER ON\r\nGO\r\n\r\nCREATE TABLE [dbo].[Imports2](\r\n\t[Id] [uniqueidentifier] NOT NULL,\r\n\t[WarehouseID] [nvarchar](max) NOT NULL,\r\n\t[Sku] [nvarchar](max) NOT NULL,\r\n\t[Title] [nvarchar](max) NOT NULL,\r\n\t[Type] [nvarchar](max) NOT NULL,\r\n\t[Step] [nvarchar](max) NOT NULL,\r\n\t[Status] [nvarchar](max) NOT NULL,\r\n\t[ImportDate] [datetime2](7) NOT NULL,\r\n\t[CreatedOn] [datetime2](7) NOT NULL,\r\n\t[ModifiedOn] [datetime2](7) NOT NULL,\r\n\t[Enabled] [bit] NOT NULL,\r\n\t[Description] [nvarchar](max) NOT NULL,\r\n\t[Digital] [nvarchar](max) NOT NULL,\r\n\t[Efood] [nvarchar](max) NOT NULL,\r\n CONSTRAINT [PK_Imports2] PRIMARY KEY CLUSTERED \r\n(\r\n\t[Id] ASC\r\n)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]\r\n) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]\r\nGO");
            var DeleteGetDiffsFunction = "DROP Function [dbo].[GetDiffs]";
            migrationBuilder.Sql(DeleteGetDiffsFunction);
            var GetDifferencesFunction = "USE [IntegrationService2]\r\nGO\r\nSET ANSI_NULLS ON\r\nGO\r\nSET QUOTED_IDENTIFIER ON\r\nGO\r\nCreate or alter function [dbo].[GetDiffs]()      \r\nreturns table      \r\nas\r\nreturn Select * from\r\n            (select distinct t1.Sku,t1.title,t1.Description,t1.Status,t1.Step,t1.WarehouseID,t1.Type from Imports as t1\r\n            Except\r\n            Select  distinct t2.Sku,t2.title,t2.Description,t2.Status,t2.Step,t2.WarehouseID,t2.Type from Imports2 as t2) as outputVar\r\nGO";
            migrationBuilder.Sql(GetDifferencesFunction);
            var InitDbSql = "USE [IntegrationService2]\r\nGO  \r\nCREATE OR ALTER PROCEDURE InitializeImports AS   BEGIN \r\n--GO\\r\\nIF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Imports2]') AND type in (N'U'))\\r\\n\\r\\ndelete from [dbo].[Imports2]\\r\\n--GO\r\nIF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Imports2]') AND type in (N'U')) delete from [dbo].[Imports2]\r\nINSERT INTO [dbo].[Imports2] ([Id]\r\n   ,[WarehouseID]\r\n   ,[Sku]\r\n   ,[Title]\r\n   ,[Type]\r\n   ,[Step]\r\n   ,[Status]\r\n   ,[ImportDate]\r\n   ,[CreatedOn]\r\n   ,[ModifiedOn]\r\n   ,[Enabled]\r\n   ,[Description]\r\n   ,[Digital]\r\n   ,[Efood])\r\n   SELECT[Id]\r\n      ,[WarehouseID]\r\n      ,[Sku]\r\n      ,[Title]\r\n      ,[Type]\r\n      ,[Step]\r\n      ,[Status]\r\n      ,[ImportDate]\r\n      ,[CreatedOn]\r\n      ,[ModifiedOn]\r\n      ,[Enabled]\r\n      ,[Description]\r\n      ,[Digital]\r\n      ,[Efood] FROM[dbo].[Imports]\r\n\t  END \r\n\t  GO";
            migrationBuilder.Sql(InitDbSql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
