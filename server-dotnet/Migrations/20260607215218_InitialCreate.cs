using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeoSat.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Banco Oracle já existe e está populado — esta migration é apenas um snapshot do estado atual.
            // Nenhuma operação DDL é executada aqui para evitar conflitos com as tabelas existentes.
            // A tabela __EFMigrationsHistory será criada automaticamente pelo EF Core ao rodar 'dotnet ef database update'.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Rollback não aplicável — o banco é gerenciado externamente pela equipe de infraestrutura.
        }
    }
}
