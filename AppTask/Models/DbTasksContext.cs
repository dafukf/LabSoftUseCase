using Microsoft.EntityFrameworkCore;

namespace AppTask.Models;

public partial class DbTasksContext : DbContext
{
    public DbTasksContext()
    {
    }

    public DbTasksContext(DbContextOptions<DbTasksContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Departamento> Departamentos { get; set; } = null!;
    public virtual DbSet<Funcionario> Funcionarios { get; set; } = null!;
    public virtual DbSet<Tarefa> Tarefas { get; set; } = null!;
    public virtual DbSet<Incidente> Incidentes { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Name=ConexaoSqlServer");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.HasKey(e => e.Codigo);
            entity.ToTable("Departamento");

            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Sigla)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Funcionario>(entity =>
        {
            entity.HasKey(e => e.Codigo);
            entity.ToTable("Funcionario");

            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Cargo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(e => e.Departamento)
                .WithMany(e => e.Funcionarios)
                .HasForeignKey(e => e.DepartamentoId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Funcionario_Departamento");
        });

        modelBuilder.Entity<Tarefa>(entity =>
        {
            entity.HasKey(e => e.Codigo);
            entity.ToTable("Tarefa");

            entity.Property(e => e.Descricao)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Prazo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.StatusTarefa)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.DataPlanejada).HasColumnType("datetime");
            entity.Property(e => e.DataIniciada).HasColumnType("datetime");
            entity.Property(e => e.DataFinalizada).HasColumnType("datetime");
            entity.Property(e => e.DataCancelada).HasColumnType("datetime");

            entity.HasOne(e => e.Funcionario)
                .WithMany(e => e.Tarefas)
                .HasForeignKey(e => e.FuncionarioId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Tarefa_Funcionario");
        });

        modelBuilder.Entity<Incidente>(entity =>
        {
            entity.HasKey(e => e.Codigo);
            entity.ToTable("Incidente");

            entity.Property(e => e.DescricaoProblema)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.DataIncidente).HasColumnType("datetime");
            entity.Property(e => e.Solucao)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Resolvido)
                .HasMaxLength(3)
                .IsUnicode(false);
        });
    }
}
