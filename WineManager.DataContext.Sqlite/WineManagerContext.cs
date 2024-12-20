using Microsoft.EntityFrameworkCore;
using WineManager.DataContext.Sqlite;

namespace WineManager.EntityModels;

public partial class WineManagerContext : DbContext
{
    public WineManagerContext()
    {
    }

    public WineManagerContext(DbContextOptions<WineManagerContext> options)
        : base(options)
    {
    }

    //DbSets for each table of the database
    public virtual DbSet<Producer> Producers { get; set; }

    public virtual DbSet<Wine> Wines { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string database = "WineManager.db"; //Name of the database
            string dir = Environment.CurrentDirectory;
            string path = string.Empty;

            if (dir.EndsWith("net8.0"))
            {
                //In the <project>\bin\<Debug|Release>\net8.0\ directory.
                path = Path.Combine("..", "..", "..", "..", database);
            }

            else
            {
                //In the <project> directory
                path = Path.Combine("..", database);
            }

            path = Path.GetFullPath(path); //Get the full path
            WineManagerContextLogger.WriteLine($"Database path: {path}"); //Log the database path

            if (!File.Exists(path)) //Checks if the database file exists
            {
                throw new FileNotFoundException(message: $"{path} not found.", fileName: path);
            }

            optionsBuilder.UseSqlite($"Data Source={path}"); //Configure the DbContext to use SQLite
            optionsBuilder.LogTo(WineManagerContextLogger.WriteLine, new[] { Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting });
        }
    }

    /// <summary>
    /// Configures the models for the database
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Define the relationship between Wine and Producer entities
        modelBuilder.Entity<Wine>()
            .HasOne(w=>w.Producer)
            .WithMany(w=> w.Wines)
            .HasForeignKey(w=>w.ProducerName)
            .OnDelete(DeleteBehavior.Cascade);

        //Configure the producer entity
        modelBuilder.Entity<Producer>(entity =>
        {
            entity.ToTable("producer");
            entity.Property(p => p.ProducerName).HasColumnName("producerName");
            entity.Property(p => p.Country).HasColumnName("country");
            entity.Property(p => p.Region).HasColumnName("region");
        });

        //Configure the wine entity
        modelBuilder.Entity<Wine>(entity =>
        {
            entity.ToTable("wine");
            entity.Property(w => w.WineId).HasColumnName("wineId");
            entity.Property(w => w.WineName).HasColumnName("wineName");
            entity.Property(w => w.BottleSize).HasColumnName("bottleSize");
            entity.Property(w => w.AlcoholContent).HasColumnName("alcoholContent");
            entity.Property(w => w.ProducerName).HasColumnName("producerName");
        });

        modelBuilder.Entity<Wine>(entity =>
        {
            entity.Property(w => w.AlcoholContent)
                .HasPrecision(3, 1);
            entity.Property(w => w.WineName)
                .HasMaxLength(100)
                .IsRequired();
        });

        modelBuilder.Entity<Producer>(entity =>
        {
            entity.Property(p => p.ProducerName)
                .HasMaxLength(100)
                .IsRequired();
            entity.Property(p => p.Region)
                .HasMaxLength(50)
                .IsRequired();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
