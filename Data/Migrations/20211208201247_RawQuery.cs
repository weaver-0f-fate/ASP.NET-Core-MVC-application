﻿using System.IO;
using Microsoft.EntityFrameworkCore.Migrations;


namespace Data.Migrations
{
    public partial class RawQuery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string script = File.ReadAllText(@"..\Data\Queries\InsertDataQuery.sql");

            migrationBuilder.Sql(script);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
