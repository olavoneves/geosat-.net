using GeoSat.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GeoSat.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UsuarioNet> UsuariosNet { get; set; }
    public DbSet<RefreshTokenNet> RefreshTokensNet { get; set; }
    public DbSet<Produtor> Produtores { get; set; }
    public DbSet<Propriedade> Propriedades { get; set; }
    public DbSet<Talhao> Talhoes { get; set; }
    public DbSet<Sensor> Sensores { get; set; }
    public DbSet<LeituraSensor> LeiturasSeensores { get; set; }
    public DbSet<ImagemSatelital> ImagensSatelitais { get; set; }
    public DbSet<Configuracao> Configuracoes { get; set; }
    public DbSet<Alerta> Alertas { get; set; }
    public DbSet<LogAlerta> LogsAlerta { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UsuarioNet>()
            .HasIndex(u => u.DsEmail).IsUnique();

        modelBuilder.Entity<RefreshTokenNet>()
            .HasOne(r => r.Usuario)
            .WithMany()
            .HasForeignKey(r => r.IdUsuario)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Propriedade>()
            .HasOne(p => p.Produtor)
            .WithMany(pr => pr.Propriedades)
            .HasForeignKey(p => p.IdProdutor);

        modelBuilder.Entity<Talhao>()
            .HasOne(t => t.Propriedade)
            .WithMany(p => p.Talhoes)
            .HasForeignKey(t => t.IdPropriedade);

        modelBuilder.Entity<Sensor>()
            .HasOne(s => s.Talhao)
            .WithMany(t => t.Sensores)
            .HasForeignKey(s => s.IdTalhao);

        modelBuilder.Entity<LeituraSensor>()
            .HasOne(l => l.Sensor)
            .WithMany(s => s.Leituras)
            .HasForeignKey(l => l.IdSensor);

        modelBuilder.Entity<ImagemSatelital>()
            .HasOne(i => i.Talhao)
            .WithMany(t => t.ImagensSatelitais)
            .HasForeignKey(i => i.IdTalhao);

        modelBuilder.Entity<Configuracao>()
            .HasOne(c => c.Talhao)
            .WithOne(t => t.Configuracao)
            .HasForeignKey<Configuracao>(c => c.IdTalhao);

        modelBuilder.Entity<Alerta>()
            .HasOne(a => a.Talhao)
            .WithMany(t => t.Alertas)
            .HasForeignKey(a => a.IdTalhao);

        modelBuilder.Entity<LogAlerta>()
            .HasOne(l => l.Alerta)
            .WithMany(a => a.Logs)
            .HasForeignKey(l => l.IdAlerta);
    }
}
