using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using GDGproj.Models;
using GDGproj.Data;
using System.Runtime.InteropServices.WindowsRuntime;

namespace GDGproj.Controllers
{
    [ApiController]
    [Route("api/comments")]
    public class CommentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] Comment comment)
        {
            if (comment == null)
                return BadRequest("Invalid comment data");

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, comment);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
                return NotFound();

            return Ok(comment);
        }

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetCommentsByPost(int postId)
        {
            var comments = await _context.Comments.Where(c => c.PostId == postId).ToListAsync();
            return Ok(comments);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] Comment updatedComment)
        {
            var existingComment = await _context.Comments.FindAsync(id);
            if (existingComment == null)
                return NotFound();

            existingComment.Content = updatedComment.Content;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
                return NotFound();

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
