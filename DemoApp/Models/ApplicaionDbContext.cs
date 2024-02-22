using Microsoft.EntityFrameworkCore;
using System;


namespace DemoApp.Models
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }


        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<Notes> Notes { get; set; }


        public DbSet<UserNotes> UserNotes { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region EntityConfig

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.Id).HasName("user_pkey");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserName).HasColumnName("user_name");
                entity.HasIndex(e => e.UserEmail, "uq_users_email").IsUnique();
                entity.Property(e => e.UserEmail).HasColumnName("email");
                entity.Property(e => e.UserPassword).HasColumnName("password");
                entity.Property(e => e.UserSalt).HasColumnName("user_salt");
                entity.Property(e => e.UserHashedPassword).HasColumnName("user_hashed_password");

            });


            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");
                entity.HasKey(e => e.Id).HasName("role_pkey");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.RoleName).HasColumnName("user_name");
                entity.HasIndex(e => e.RoleName, "uq_roles_name").IsUnique();


            });


            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("permissions");
                entity.HasKey(e => e.Id).HasName("permission_pkey");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.PermissionName).HasColumnName("permission_name");
                entity.HasIndex(e => e.PermissionName, "uq_permission_name").IsUnique();


            });


            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.ToTable("role_permission");
                entity.HasKey(e => e.Id).HasName("role_permission_pkey");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.PermissionId).HasColumnName("permission_id");
                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.HasOne(e => e.Permission).WithMany(e => e.RolePermissions)
                      .HasForeignKey(k => k.PermissionId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("fk_role_permissison_permisssion_id");

                entity.HasOne(e => e.Role).WithMany(e => e.RolePermissions)
                      .HasForeignKey(k => k.RoleId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("fk_role_permissison_role_id");

            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("user_role");
                entity.HasKey(e => e.Id).HasName("user_role_pkey");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.HasOne(e => e.Role).WithMany(e => e.UserRoles)
                      .HasForeignKey(k => k.RoleId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("fk_user_role_role_id");

                entity.HasOne(e => e.User).WithOne(e => e.UserRoles)
                      .HasForeignKey<UserRole>(k => k.UserId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("fk_user_role_user_id");

            });



            modelBuilder.Entity<Notes>(entity =>
            {
                entity.ToTable("notes");
                entity.HasKey(e => e.Id).HasName("notes_pkey");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Topic).HasColumnName("topic");
                entity.Property(e => e.Description).HasColumnName("decription");

            });



            modelBuilder.Entity<UserNotes>(entity =>
            {
                entity.ToTable("user_notes");
                entity.HasKey(e => e.Id).HasName("user_notes_pkey");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.NoteId).HasColumnName("note_id");

                entity.HasOne(e => e.Note).WithMany(e => e.UserNotes)
                    .HasForeignKey(e => e.NoteId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_user_notes_note_id");

                entity.HasOne(e => e.User).WithMany(e => e.UserNotes)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_user_notes_user_id");
            });


            #endregion

            // Seed some initial data
            // SeedData(modelBuilder);

        }

        #region SeedData

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Check if there are no Users in the database
           
                // Add seed Users
                modelBuilder.Entity<User>().HasData(
                    new User
                    {
                        Id = Guid.NewGuid(),
                        UserName = "John Doe",
                        UserEmail = "john@example.com",
                        UserPassword = "hashedPassword"
                    },
                    new User
                    {
                        Id = Guid.NewGuid(),
                        UserName = "Jane Doe",
                        UserEmail = "jane@example.com",
                        UserPassword = "hashedPassword"
                    }
                );
            

            // Check if there are no Roles in the database
            
                // Add seed Roles
                modelBuilder.Entity<Role>().HasData(
                    new Role
                    {
                        Id = Guid.NewGuid(),
                        RoleName = "Admin"
                    },
                    new Role
                    {
                        Id = Guid.NewGuid(),
                        RoleName = "User"
                    }
                );
            

            // Check if there are no Permissions in the database
            
                // Add seed Permissions
                modelBuilder.Entity<Permission>().HasData(
                    new Permission
                    {
                        Id = Guid.NewGuid(),
                        PermissionName = "Read"
                    },
                    new Permission
                    {
                        Id = Guid.NewGuid(),
                        PermissionName = "Write"
                    },
                    new Permission
                    {
                        Id = Guid.NewGuid(),
                        PermissionName = "Update"
                    },
                    new Permission
                    {
                        Id = Guid.NewGuid(),
                        PermissionName = "Delete"
                    }
                );
            
        }

        #endregion

    }
}
