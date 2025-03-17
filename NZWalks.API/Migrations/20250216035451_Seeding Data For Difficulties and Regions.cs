using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZWalks.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDataForDifficultiesandRegions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("02f2826e-0626-4994-8681-8698d4c41580"), "Easy" },
                    { new Guid("21fe0ec8-d430-4dbe-a2f7-184b057408db"), "Medium" },
                    { new Guid("67fdeb1c-f8f8-4c4c-9fba-f412d785c951"), "Hard" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "Name", "RegionImageUrl" },
                values: new object[,]
                {
                    { new Guid("0e112dd5-2aba-4225-8f33-90c289826164"), "KOR", "Korea", "kor-img.com" },
                    { new Guid("151948d0-99d6-481a-98f1-b481eb849fdb"), "AKL", "Auckland", "akl-img.com" },
                    { new Guid("2145a429-1ea6-48f0-a99b-7245c110d624"), "VN", "VietNam", "vn-img.com" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("02f2826e-0626-4994-8681-8698d4c41580"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("21fe0ec8-d430-4dbe-a2f7-184b057408db"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("67fdeb1c-f8f8-4c4c-9fba-f412d785c951"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("0e112dd5-2aba-4225-8f33-90c289826164"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("151948d0-99d6-481a-98f1-b481eb849fdb"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("2145a429-1ea6-48f0-a99b-7245c110d624"));
        }
    }
}
