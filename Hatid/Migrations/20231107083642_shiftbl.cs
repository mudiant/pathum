using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hatid.Migrations
{
    /// <inheritdoc />
    public partial class shiftbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shift",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeInYear = table.Column<int>(type: "int", nullable: false),
                    TimeInMonth = table.Column<int>(type: "int", nullable: false),
                    TimeInDay = table.Column<int>(type: "int", nullable: false),
                    TimeInHour = table.Column<int>(type: "int", nullable: false),
                    TimeInMinute = table.Column<int>(type: "int", nullable: false),
                    TimeInSecond = table.Column<int>(type: "int", nullable: false),
                    TimeOut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeOutYear = table.Column<int>(type: "int", nullable: false),
                    TimeOutMonth = table.Column<int>(type: "int", nullable: false),
                    TimeOutDay = table.Column<int>(type: "int", nullable: false),
                    TimeOutHour = table.Column<int>(type: "int", nullable: false),
                    TimeOutMinute = table.Column<int>(type: "int", nullable: false),
                    TimeOutSecond = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shift", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shift");
        }
    }
}
