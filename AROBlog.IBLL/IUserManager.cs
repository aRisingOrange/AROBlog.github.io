namespace AROBlog.IBLL
{
    public interface IUserManager
    {
        Task Register(string account, string password);
        bool Login(string account,string password,out Guid userid);
        Task ChangePassword(string account, string oldPassword,string newPassword);
    }
}