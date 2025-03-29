namespace GDGproj.Interfaces
{
    public interface ILikeRepository
    {
        Task<bool> LikePost(int userId, int postId);
        Task<bool> UnlikePost(int userId, int postId);

        Task<int> GetLikeCount(int postId);
    }
}
    