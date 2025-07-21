using NotesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace NotesApi.Data
{
    public class NotesDbContext : DbContext
    {
        public NotesDbContext(DbContextOptions<NotesDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; } // Optional if you already have Notes
    }
}