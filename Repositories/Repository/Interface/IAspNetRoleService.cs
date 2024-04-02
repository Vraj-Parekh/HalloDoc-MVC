using Entities.Models;

namespace Repositories.Repository.Interface
{
    public interface IAspNetRoleService
    {
        Aspnetrole GetName(string name);
    }
}