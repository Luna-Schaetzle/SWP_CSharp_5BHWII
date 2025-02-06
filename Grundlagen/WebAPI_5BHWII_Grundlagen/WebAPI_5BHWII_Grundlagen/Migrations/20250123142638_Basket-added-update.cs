using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI_5BHWII_Grundlagen.Migrations
{
    /// <inheritdoc />
    public partial class Basketaddedupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Baskets_BasketId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_BasketId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "BasketId",
                table: "Articles");

            migrationBuilder.AddColumn<int>(
                name: "ArticleId",
                table: "Baskets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_ArticleId",
                table: "Baskets",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_Articles_ArticleId",
                table: "Baskets",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "ArticleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_Articles_ArticleId",
                table: "Baskets");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_ArticleId",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "Baskets");

            migrationBuilder.AddColumn<int>(
                name: "BasketId",
                table: "Articles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_BasketId",
                table: "Articles",
                column: "BasketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Baskets_BasketId",
                table: "Articles",
                column: "BasketId",
                principalTable: "Baskets",
                principalColumn: "BasketId");
        }
    }
}
