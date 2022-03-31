using IA.Restaurant.Entities;
using IA.Restaurant.Entities.Tables;
using Microsoft.EntityFrameworkCore;

namespace IA.Restaurant.Data
{
    public partial class RestaurantContext : DbContext
    {
        public RestaurantContext()
        {
        }

        public RestaurantContext(DbContextOptions<RestaurantContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#pragma warning disable CS1030 // #warning directive
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseMySql("Server=3.86.233.166;port=3306;uid=eslabon_base;password=password;Database=EslabonSuite");
            }
#pragma warning restore CS1030 // #warning directive
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasOne(d => d.IdStatusNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.IdStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ORDERS-STATUS");
            });

            modelBuilder.Entity<OrderDetails>(entity =>
            {
                /// add haskey
                entity.HasKey(d => new { d.IdOrder, d.IdProduct});
                entity.HasOne(d => d.IdOrderNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdOrder)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ORDER_DETAILS-ORDERS");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdProduct)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ORDER_DETAILS-PRODUCTS");
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Sku).IsUnicode(false);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        /// <summary> 
        /// Esta función guarda la información y automatiza las fechas de modificacion / creación de registros.
        /// Referencia: https://stackoverflow.com/questions/46927542/ef-core-built-in-created-and-edited-timestamps
        /// </summary>
        public override int SaveChanges()
        {
            var newEntities = this.ChangeTracker.Entries()
                .Where(
                    x => x.State == EntityState.Added &&
                    x.Entity != null &&
                    x.Entity as IAutomaticModel != null
                    )
                .Select(x => x.Entity as IAutomaticModel);

            var modifiedEntities = this.ChangeTracker.Entries()
                .Where(
                    x => x.State == EntityState.Modified &&
                    x.Entity != null &&
                    x.Entity as IAutomaticModel != null
                    )
                .Select(x => x.Entity as IAutomaticModel);

            foreach (var newEntity in newEntities)
            {
                // Automatizando el guardado
                newEntity.CreationDate = DateTime.Now;
                newEntity.ModificationDate = DateTime.Now;
            }

            foreach (var modifiedEntity in modifiedEntities)
            {
                // Automatizando el guardado
                modifiedEntity.ModificationDate = DateTime.Now;
            }

            return base.SaveChanges();
        }

        /// <summary>
        /// Sobreescribe el método para guardar los cambios en el track asíncronamente, para configurar los campos de fechas
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación</param>
        /// <returns>Número de cambios procesados</returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Asigna la fecha de registro antes de guardar los cambios
            var newEntities = this.ChangeTracker.Entries()
                .Where(
                    x => x.State == EntityState.Added &&
                    x.Entity != null &&
                    x.Entity as IAutomaticModel != null
                    )
                .Select(x => x.Entity as IAutomaticModel);

            var modifiedEntities = this.ChangeTracker.Entries()
                .Where(
                    x => x.State == EntityState.Modified &&
                    x.Entity != null &&
                    x.Entity as IAutomaticModel != null
                    )
                .Select(x => x.Entity as IAutomaticModel);

            foreach (var newEntity in newEntities)
            {
                // Automatizando el guardado
                newEntity.CreationDate = DateTime.Now;
                newEntity.ModificationDate = DateTime.Now;
            }

            foreach (var modifiedEntity in modifiedEntities)
            {
                // Automatizando el guardado
                modifiedEntity.ModificationDate = DateTime.Now;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
