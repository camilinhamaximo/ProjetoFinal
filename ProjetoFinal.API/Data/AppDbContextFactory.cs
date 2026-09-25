using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ProjetoFinal.API.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // Constrói a configuração para ler o ficheiro appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.Development.json", optional: true)
                .Build();

            // Obtém a string de conexão configurada
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Configura as opções do DbContext
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            
            // Caso utilize SQL Server (ajuste para UseSqlite, UseNpgsql, etc., conforme o seu banco):
            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}