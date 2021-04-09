using GoogleDrive.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleDrive
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<TrainingFiles> TrainingFiles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrainingFiles>().HasData(new TrainingFiles
            {
                FileID = 1,
                FileName = "Temp1",
                FilePath = "Test1"                

            }, new TrainingFiles
            {
                FileID = 2,
                FileName = "Temp2",
                FilePath = "Test2"
            });
        }


    }
}
