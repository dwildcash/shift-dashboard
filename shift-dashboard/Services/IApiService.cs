using System.Collections.Generic;
using System.Threading.Tasks;

namespace shift_dashboard.Services
{
    public interface IApiService
    {
        public Task<shift_dashboard.Model.Delegate[]> GetDelegatesFromDb();

        public Task<List<shift_dashboard.Model.Delegate>> GetDelegatesFromApiAsync();

        public Task<object> UpdateDelegateDb();

    }
}