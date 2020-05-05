using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseLibrary.API.Migrations
{
    public partial class AddDateOfDeathToAuthor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateOfDeath",
                table: "Authors",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("102b566b-ba1f-404c-b2df-e2cde39ade09"),
                column: "DateOfBirth",
                value: new DateTimeOffset(new DateTime(1702, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("2902b665-1190-4c70-9915-b9c2d7680450"),
                column: "DateOfBirth",
                value: new DateTimeOffset(new DateTime(1701, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("2aadd2df-7caf-45ab-9355-7f6332985a87"),
                column: "DateOfBirth",
                value: new DateTimeOffset(new DateTime(1723, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("2ee49fe3-edf2-4f91-8409-3eb25ce6ca51"),
                column: "DateOfBirth",
                value: new DateTimeOffset(new DateTime(1721, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("5b3621c0-7b12-4e80-9c8b-3398cba7ee05"),
                column: "DateOfBirth",
                value: new DateTimeOffset(new DateTime(1690, 11, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                column: "DateOfBirth",
                value: new DateTimeOffset(new DateTime(1650, 7, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"),
                column: "DateOfBirth",
                value: new DateTimeOffset(new DateTime(1668, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfDeath",
                table: "Authors");

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("102b566b-ba1f-404c-b2df-e2cde39ade09"),
                column: "DateOfBirth",
                value: new DateTimeOffset(new DateTime(1702, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("2902b665-1190-4c70-9915-b9c2d7680450"),
                column: "DateOfBirth",
                value: new DateTimeOffset(new DateTime(1701, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("2aadd2df-7caf-45ab-9355-7f6332985a87"),
                column: "DateOfBirth",
                value: new DateTimeOffset(new DateTime(1723, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("2ee49fe3-edf2-4f91-8409-3eb25ce6ca51"),
                column: "DateOfBirth",
                value: new DateTimeOffset(new DateTime(1721, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("5b3621c0-7b12-4e80-9c8b-3398cba7ee05"),
                column: "DateOfBirth",
                value: new DateTimeOffset(new DateTime(1690, 11, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                column: "DateOfBirth",
                value: new DateTimeOffset(new DateTime(1650, 7, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"),
                column: "DateOfBirth",
                value: new DateTimeOffset(new DateTime(1668, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)));
        }
    }
}
