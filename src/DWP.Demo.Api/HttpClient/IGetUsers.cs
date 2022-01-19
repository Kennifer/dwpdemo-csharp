using System.Collections.Generic;
using System.Threading.Tasks;
using DWP.Demo.Api.Types;

namespace DWP.Demo.Api.HttpClient
{
    public interface IGetUsers
    {
        Task<IEnumerable<User>> Execute();
    }
}