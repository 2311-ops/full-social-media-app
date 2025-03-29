using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/user-relations")]
public class UserRelationController : ControllerBase
{
    private readonly IUserRelationService _userRelationService;

    public UserRelationController(IUserRelationService userRelationService)
    {
        _userRelationService = userRelationService;
    }

    [HttpPost("send-friend-request")]
    public async Task<IActionResult> SendFriendRequest(int senderId, int receiverId)
    {
        var success = await _userRelationService.SendFriendRequest(senderId, receiverId);
        return success ? Ok("Friend request sent.") : BadRequest("Request failed.");
    }

    [HttpPost("accept-friend-request")]
    public async Task<IActionResult> AcceptFriendRequest(int requestId)
    {
        var success = await _userRelationService.AcceptFriendRequest(requestId);
        return success ? Ok("Friend request accepted.") : NotFound("Request not found.");
    }

    [HttpPost("reject-friend-request")]
    public async Task<IActionResult> RejectFriendRequest(int requestId)
    {
        var success = await _userRelationService.RejectRequest(requestId);
        return success ? Ok("Friend request rejected.") : NotFound("Request not found.");
    }

    [HttpPost("remove-friend")]
    public async Task<IActionResult> RemoveFriend(int userId, int friendId)
    {
        var success = await _userRelationService.RemoveFriend(userId, friendId);
        return success ? Ok("Friend removed.") : NotFound("Friendship does not exist.");

    }
}
    

    