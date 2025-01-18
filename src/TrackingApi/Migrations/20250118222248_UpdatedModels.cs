using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackingApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "TrackingItems",
                newName: "LastUpdate");

            migrationBuilder.CreateTable(
                name: "TrackingDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Operation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Branch = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrackingNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrackingItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackingDetail_TrackingItems_TrackingItemId",
                        column: x => x.TrackingItemId,
                        principalTable: "TrackingItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackingDetail_TrackingItemId",
                table: "TrackingDetail",
                column: "TrackingItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrackingDetail");

            migrationBuilder.RenameColumn(
                name: "LastUpdate",
                table: "TrackingItems",
                newName: "LastUpdated");
        }
    }
}
