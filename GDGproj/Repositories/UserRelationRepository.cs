using GDGproj.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class UserRelationRepository : IUserRelationRepository
{
    private readonly AppDbContext _context;

    public UserRelationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> SendFriendRequest(int senderId, int receiverId)
    {
        if (senderId == receiverId)
        {
            return false;
        }

        var existingRequest = await _context.FriendRequests
            .FirstOrDefaultAsync(fr => fr.SenderId == senderId && fr.ReceiverId == receiverId);

        if (existingRequest != null) return false;

        _context.FriendRequests.Add(new FriendRequests
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Status = "Pending"
        });

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AcceptFriendRequest(int requestId)
    {
        var request = await _context.FriendRequests.FindAsync(requestId);
        if (request == null)
        {
            return false;
        }

        request.Status = "Accepted";
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveFriend(int userId, int friendId)
    {
        var request = await _context.FriendRequests
            .FirstOrDefaultAsync(fr =>
                (fr.SenderId == userId && fr.ReceiverId == friendId) ||
                (fr.SenderId == friendId && fr.ReceiverId == userId));

        if (request == null || request.Status != "Accepted")
        {
            return false;
        }

        _context.FriendRequests.Remove(request);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RejectRequest(int requestId)
    {
        var request = await _context.FriendRequests.FindAsync(requestId);
        if (request == null)
        {
            return false;
        }

        request.Status = "Rejected";
        await _context.SaveChangesAsync();
        return true;
    }
}
