using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StuffyHelper.EntityFrameworkCore.Migrations
{
    public partial class fixAmount2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "unit-types",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[,]
                {
                    { new Guid("320053b6-110a-4358-9289-21e64d718b60"), true, "Мл." },
                    { new Guid("32939f8e-3818-4753-8d1b-4ba8ab7783f9"), true, "Кг." },
                    { new Guid("6700cac9-f36e-4697-a6fe-27fdbaebd267"), true, "Л." },
                    { new Guid("7142d1aa-53b1-416c-80b8-18b5d3ba33ab"), true, "Шт." },
                    { new Guid("eda2a0fe-539c-471d-9941-e0ce8982e923"), true, "Уп." },
                    { new Guid("f73043eb-9e20-4934-845a-7722557f164e"), true, "Гр." }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "unit-types",
                keyColumn: "Id",
                keyValue: new Guid("320053b6-110a-4358-9289-21e64d718b60"));

            migrationBuilder.DeleteData(
                table: "unit-types",
                keyColumn: "Id",
                keyValue: new Guid("32939f8e-3818-4753-8d1b-4ba8ab7783f9"));

            migrationBuilder.DeleteData(
                table: "unit-types",
                keyColumn: "Id",
                keyValue: new Guid("6700cac9-f36e-4697-a6fe-27fdbaebd267"));

            migrationBuilder.DeleteData(
                table: "unit-types",
                keyColumn: "Id",
                keyValue: new Guid("7142d1aa-53b1-416c-80b8-18b5d3ba33ab"));

            migrationBuilder.DeleteData(
                table: "unit-types",
                keyColumn: "Id",
                keyValue: new Guid("eda2a0fe-539c-471d-9941-e0ce8982e923"));

            migrationBuilder.DeleteData(
                table: "unit-types",
                keyColumn: "Id",
                keyValue: new Guid("f73043eb-9e20-4934-845a-7722557f164e"));
        }
    }
}
