using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EF_Assignment_04.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSampleData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "Code", "Address", "ManagerId", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { "CTR-01", "100 Market Street", 1, "Central Branch", "+1-202-555-0201" },
                    { "HBR-01", "250 Harbor Avenue", 2, "Harbor Branch", "+1-202-555-0202" }
                });

            migrationBuilder.Sql("UPDATE Accounts SET BranchCode = 'CTR-01' WHERE BranchCode = 'CAI-01';");
            migrationBuilder.Sql("UPDATE Accounts SET BranchCode = 'HBR-01' WHERE BranchCode = 'ALX-01';");

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "Code",
                keyValue: "ALX-01");

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "Code",
                keyValue: "CAI-01");

            migrationBuilder.UpdateData(
                table: "Managers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "FullName", "PhoneNumber" },
                values: new object[] { "alex.morgan@nationalbank.example", "Alex Morgan", "+1-202-555-0101" });

            migrationBuilder.UpdateData(
                table: "Managers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "FullName", "PhoneNumber" },
                values: new object[] { "jamie.lee@nationalbank.example", "Jamie Lee", "+1-202-555-0102" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "Code", "Address", "ManagerId", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { "ALX-01", "42 Fouad Street, Alexandria", 2, "Alexandria Branch", "034870002" },
                    { "CAI-01", "15 Tahrir Square, Cairo", 1, "Cairo Main Branch", "0227940001" }
                });

            migrationBuilder.Sql("UPDATE Accounts SET BranchCode = 'CAI-01' WHERE BranchCode = 'CTR-01';");
            migrationBuilder.Sql("UPDATE Accounts SET BranchCode = 'ALX-01' WHERE BranchCode = 'HBR-01';");

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "Code",
                keyValue: "CTR-01");

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "Code",
                keyValue: "HBR-01");

            migrationBuilder.UpdateData(
                table: "Managers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "FullName", "PhoneNumber" },
                values: new object[] { "tarek.masry@nationalbank.eg", "Tarek El-Masry", "01001112233" });

            migrationBuilder.UpdateData(
                table: "Managers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Email", "FullName", "PhoneNumber" },
                values: new object[] { "nadia.mansour@nationalbank.eg", "Nadia Mansour", "01004445566" });
        }
    }
}
