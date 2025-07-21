using Microsoft.AspNetCore.Mvc;
using NotesApi.Models;
using NotesApi.Repositories;

namespace NotesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly NoteRepository _repo;

        public NotesController(NoteRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _repo.GetNotesAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var note = await _repo.GetNoteAsync(id);
            return note == null ? NotFound() : Ok(note);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Note note)
        {
            note.CreatedAt = DateTime.Now;
            await _repo.CreateNoteAsync(note);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Note note)
        {
            note.Id = id;
            note.UpdatedAt = DateTime.Now;
            await _repo.UpdateNoteAsync(note);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteNoteAsync(id);
            return Ok();
        }
    }
}