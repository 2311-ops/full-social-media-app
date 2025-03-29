using GDGproj.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using GDGproj.Models;
using GDGproj.Interfaces;



public class LikeRepository : ILikeRepository
{
    private readonly AppDbContext _context;
    public LikeRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<bool> LikePost(int userId, int postId)
    {
        //checking if liked the post.
        var existingLike = await _context.Likes.FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId);
        if (existingLike != null)
        {
            return false;
        }
        _context.Likes.Add(new Like { UserId = userId, PostId = postId });
        await _context.SaveChangesAsync();
        return true;


    }
    public async Task<bool> UnlikePost(int userId, int postId)
    {
        var like = await _context.Likes
            .FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId);
        if (like == null)
        {
            return false;
        }
        _context.Likes.Remove(like);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<int> GetLikeCount( int postId)
    {
        return await _context.Likes.CountAsync(l => l.PostId == postId);

    }
}
