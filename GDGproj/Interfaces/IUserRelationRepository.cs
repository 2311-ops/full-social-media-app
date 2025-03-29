public interface IUserRelationRepository
{
    Task<bool> SendFriendRequest(int senderId, int receiverId);
    Task<bool> AcceptFriendRequest(int requestId);
    Task<bool> RemoveFriend(int userId, int friendId);
    Task<bool> RejectRequest(int requestId);

}