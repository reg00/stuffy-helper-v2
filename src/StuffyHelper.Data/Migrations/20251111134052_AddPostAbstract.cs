using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StuffyHelper.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPostAbstract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Amount",
                table: "purchase-usage",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "purchase",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<long>(
                name: "Cost",
                table: "purchase",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<long>(
                name: "Amount",
                table: "purchase",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EventDateStart",
                table: "event",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EventDateEnd",
                table: "event",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "event",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<long>(
                name: "Amount",
                table: "debts",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "purchase-usage",
                type: "double precision",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "purchase",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<double>(
                name: "Cost",
                table: "purchase",
                type: "double precision",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "purchase",
                type: "double precision",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EventDateStart",
                table: "event",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EventDateEnd",
                table: "event",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "event",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "debts",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
