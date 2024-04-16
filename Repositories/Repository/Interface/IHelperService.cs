namespace Repositories.Repository.Interface
{
    public interface IHelperService
    {
        string GetLoggedinEmail();
        string GetRegionById(int regionId);
        List<string> GetRoles();
        bool IsAdmin();
        bool IsPhysician();
    }
}