using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StuffyHelper.Data.Migrations
{
    public partial class MakeUnitTypeOptional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_purchase_unit-types_UnitTypeId",
                table: "purchase");

            migrationBuilder.AlterColumn<Guid>(
                name: "UnitTypeId",
                table: "purchase",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_purchase_unit-types_UnitTypeId",
                table: "purchase",
                column: "UnitTypeId",
                principalTable: "unit-types",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_purchase_unit-types_UnitTypeId",
                table: "purchase");

            migrationBuilder.AlterColumn<Guid>(
                name: "UnitTypeId",
                table: "purchase",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_purchase_unit-types_UnitTypeId",
                table: "purchase",
                column: "UnitTypeId",
                principalTable: "unit-types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
