using GDGproj.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class LikeController : ControllerBase
{
    private readonly ILikeRepository _likeRepository;

    public LikeController(ILikeRepository likeRepository)
    {
        _likeRepository = likeRepository;
    }

    [HttpPost("like")]
    public async Task<IActionResult> LikePost(int userId, int postId)
    {
        var success = await _likeRepository.LikePost(userId, postId);
        if (!success) return BadRequest("Post already liked.");
        return Ok("The Post is liked successfully.");
    }

    [HttpPost("unlike")]
    public async Task<IActionResult> UnlikePost(int userId, int postId)
    {
        var success = await _likeRepository.UnlikePost(userId, postId);
        if (!success) return BadRequest("Like not found.");
        return Ok("Post unliked successfully.");
    }

    [HttpGet("count/{postId}")]
    public async Task<IActionResult> GetLikeCount(int postId)
    {
        var count = await _likeRepository.GetLikeCount(postId);
        return Ok(count);
    }
}
