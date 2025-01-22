using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulateProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Insert into Products(Name,Description,Price,ImageUrl,Stock,RegistrationDate,CategoryId)" +
                                 "Values('Suco de Laranja','Suco de Laranja natural de 350 ml',7.00,'suco_laranja.jpg',50,now(),1)");

            migrationBuilder.Sql("Insert into Products(Name,Description,Price,ImageUrl,Stock,RegistrationDate,CategoryId)" +
                                 "Values('Lanche de Presunto','Lanche de Presunto com maionese',8.50,'presunto.jpg',10,now(),2)");

            migrationBuilder.Sql("Insert into Products(Name,Description,Price,ImageUrl,Stock,RegistrationDate,CategoryId)" +
                                 "Values('Pudim 100 g','Pudim de leite condensado 100g',6.75,'pudim.jpg',20,now(),3)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Products");
        }
    }
}
