using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using NotesApi.Models;

namespace NotesApi.Repositories
{
    public class NoteRepository
    {
        private readonly IDbConnection _db;

        public NoteRepository(IConfiguration config)
        {
            _db = new SqlConnection(config.GetConnectionString("DefaultConnection"));
        }

        public async Task<IEnumerable<Note>> GetNotesAsync() =>
            await _db.QueryAsync<Note>("SELECT * FROM Notes");

        public async Task<Note?> GetNoteAsync(int id) =>
            await _db.QueryFirstOrDefaultAsync<Note>("SELECT * FROM Notes WHERE Id = @Id", new { Id = id });

        public async Task<int> CreateNoteAsync(Note note)
        {
            var sql = @"INSERT INTO Notes (Title, Content, CreatedAt, UserId) 
                    VALUES (@Title, @Content, @CreatedAt, @UserId)";
            return await _db.ExecuteAsync(sql, note);
        }

        public async Task<int> UpdateNoteAsync(Note note)
        {
            var sql = @"UPDATE Notes 
                    SET Title = @Title, Content = @Content, UpdatedAt = @UpdatedAt 
                    WHERE Id = @Id";
            return await _db.ExecuteAsync(sql, note);
        }

        public async Task<int> DeleteNoteAsync(int id) =>
            await _db.ExecuteAsync("DELETE FROM Notes WHERE Id = @Id", new { Id = id });
    }
}