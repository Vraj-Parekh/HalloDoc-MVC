namespace Repositories.Repository.Interface
{
    public interface IHelperService
    {
        string GetLoggedinEmail();
        List<string> GetRoles();
        bool IsAdmin();
        bool IsPhysician();
    }
}