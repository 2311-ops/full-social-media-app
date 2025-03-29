public class UserRelationService : IUserRelationService
{
    private readonly IUserRelationRepository _userRelationRepository;
    public UserRelationService(IUserRelationRepository userRelationRepository)
    {
        _userRelationRepository = userRelationRepository;
    }
    public async Task<bool> SendFriendRequest(int senderId, int reciverId)
    {
        return await _userRelationRepository.SendFriendRequest(senderId, reciverId);
    }
    public async Task<bool> AcceptFriendRequest(int requestId)
    {
        return await _userRelationRepository.AcceptFriendRequest(requestId);
    }
    public async Task<bool> RemoveFriend(int senderId, int reciverId)
    {
        return await _userRelationRepository.RemoveFriend(senderId, reciverId);
    }
    public async Task<bool> RejectRequest(int requestId)
    {
        return await _userRelationRepository.RejectRequest(requestId);
    }
}