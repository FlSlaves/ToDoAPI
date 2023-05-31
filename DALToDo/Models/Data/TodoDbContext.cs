using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoAPI.DALToDo.Models.Data
{
    public class TodoDbContext : DbContext
    {

        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {

        }

        public DbSet<ToDoListModel> ToDoList1 { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{

        //    optionsBuilder.UseSqlServer("Server=vlad_pc; Database=TodoDB; Encrypt=False; Trusted_Connection=True");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoListModel>(
                eb =>
                {
                    eb.Property(b => b.Task).IsRequired().HasColumnType("varchar(150)");
                    eb.Property(b => b.Description).HasColumnType("varchar(300)");
                    eb.Property(b => b.Status).IsRequired().HasColumnType("varchar(15)");
                    eb.Property(b => b.Owner).IsRequired().HasColumnType("varchar(30)");
                });
        }
    }
}
