using System.Collections.Generic;
using System.Threading.Tasks;
using DWP.Demo.Api.Types;

namespace DWP.Demo.Api.HttpClient
{
    public interface IGetUsersByCity
    {
        Task<IEnumerable<User>> Execute(string city);
    }
}