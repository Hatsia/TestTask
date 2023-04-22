using Microsoft.EntityFrameworkCore.Migrations;

namespace TestTask.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9e1b619f-aac7-4ab1-b7a3-8459b42f628b", "fc6ab58f-da6e-4295-b06b-278090b6ed06", "Admin", "ADMIN" },
                    { "fb28c681-e980-48b4-879b-5ce10fb04e5f", "f9af14d0-8dbf-4a9e-afea-df6fa86bc01f", "Basic", "BASIC" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TeamId", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "d77c3597-37aa-4270-8af1-b7cde64307bf", 0, "c9312e52-483d-4bfd-856e-874020f19df4", "admin@gamil.com", false, "Peter", "Petrov", false, null, "ADMIN@GAMIL.COM", "PETER PETROV", "AQAAAAEAACcQAAAAEKUrUJpg08RTUyneiML1FrbPMZUDn/Btqrpyy5DsCm7RLQrP18+vK8BHRdYpDPckCg==", null, false, "E5BBMDK3I3PX6MZCUDSP2TGQMJNHIOU7", null, false, "Peter Petrov" },
                    { "4846352d-ff1e-4529-842d-2aa70bdb8e05", 0, "58cb5f60-6536-4d1c-b4ab-f78f2f71cfd8", "bory@gamil.com", false, "Boris", "Britva", false, null, "BORY@GAMIL.COM", "BORIS BRITVA", "AQAAAAEAACcQAAAAEHwEd5vT2gklX1DrH3pPP2TytUJj/vIayTrCKswprmy7juJvPnqMUYpDZ4T2CANEhg==", null, false, "M3ZDA3WQP6J2ZVGKBIZHOE7GKC4BR2ZF", null, false, "Boris Britva" }
                });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Team1" },
                    { 2, "Team2" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "9e1b619f-aac7-4ab1-b7a3-8459b42f628b", "d77c3597-37aa-4270-8af1-b7cde64307bf" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "fb28c681-e980-48b4-879b-5ce10fb04e5f", "4846352d-ff1e-4529-842d-2aa70bdb8e05" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "fb28c681-e980-48b4-879b-5ce10fb04e5f", "4846352d-ff1e-4529-842d-2aa70bdb8e05" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "9e1b619f-aac7-4ab1-b7a3-8459b42f628b", "d77c3597-37aa-4270-8af1-b7cde64307bf" });

            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9e1b619f-aac7-4ab1-b7a3-8459b42f628b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb28c681-e980-48b4-879b-5ce10fb04e5f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4846352d-ff1e-4529-842d-2aa70bdb8e05");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d77c3597-37aa-4270-8af1-b7cde64307bf");
        }
    }
}
