using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackingApi.Migrations
{
    /// <inheritdoc />
    public partial class FixedModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackingDetail_TrackingItems_TrackingItemId",
                table: "TrackingDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrackingDetail",
                table: "TrackingDetail");

            migrationBuilder.RenameTable(
                name: "TrackingDetail",
                newName: "TrackingDetails");

            migrationBuilder.RenameIndex(
                name: "IX_TrackingDetail_TrackingItemId",
                table: "TrackingDetails",
                newName: "IX_TrackingDetails_TrackingItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrackingDetails",
                table: "TrackingDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackingDetails_TrackingItems_TrackingItemId",
                table: "TrackingDetails",
                column: "TrackingItemId",
                principalTable: "TrackingItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackingDetails_TrackingItems_TrackingItemId",
                table: "TrackingDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrackingDetails",
                table: "TrackingDetails");

            migrationBuilder.RenameTable(
                name: "TrackingDetails",
                newName: "TrackingDetail");

            migrationBuilder.RenameIndex(
                name: "IX_TrackingDetails_TrackingItemId",
                table: "TrackingDetail",
                newName: "IX_TrackingDetail_TrackingItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrackingDetail",
                table: "TrackingDetail",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackingDetail_TrackingItems_TrackingItemId",
                table: "TrackingDetail",
                column: "TrackingItemId",
                principalTable: "TrackingItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
