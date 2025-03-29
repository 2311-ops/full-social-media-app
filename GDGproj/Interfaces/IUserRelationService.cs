public interface IUserRelationService
{
    Task<bool> SendFriendRequest(int senderId, int reciverId);
    Task<bool> AcceptFriendRequest(int requestId);
    Task<bool> RemoveFriend(int senderId, int reciverId);
    Task<bool> RejectRequest(int requestId);
}