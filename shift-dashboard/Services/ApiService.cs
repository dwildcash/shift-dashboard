using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using shift_dashboard.Data;
using shift_dashboard.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Delegate = shift_dashboard.Model.Delegate;

namespace shift_dashboard.Services
{
    public class ApiService : IApiService
    {
        private DashboardConfig _dashboardOptions;
        private readonly ILogger<ApiService> _logger;
        private DashboardContext _dbcontext;

        public ApiService(ILogger<ApiService> logger, DashboardConfig dashboardconfig, DashboardContext dbcontext)
        {
            _dashboardOptions = dashboardconfig;
            _logger = logger;
            _dbcontext = dbcontext;
        }

        public async Task<object> UpdateDelegateDb()
        {
            try
            {
                var Scandate = DateTime.Now;

                var DelegatesFromApi = this.GetDelegatesFromApiAsync().Result;

                foreach (var sdelegate in DelegatesFromApi)
                {
                    var ss = _dbcontext.Delegates.FirstOrDefault(x => x.Address == sdelegate.Address);

                    if (ss == null)
                    {
                        sdelegate.Date = Scandate;
                        _dbcontext.Delegates.Add(sdelegate);
                        ss = sdelegate;
                    }
                    else
                    {
                        ss.Rank = sdelegate.Rank;
                        ss.Missedblocks = sdelegate.Missedblocks;
                        ss.Producedblocks = sdelegate.Producedblocks;
                        ss.Productivity = sdelegate.Productivity;
                        ss.Rate = sdelegate.Rate;
                        ss.Vote = sdelegate.Vote;
                        ss.Approval = sdelegate.Approval;
                        ss.Date = Scandate;
                    }

                    var voters = await this.GetVoters(ss.PublicKey);

                    var sdelegateStat = new DelegateStat
                    {
                        Date = Scandate,
                        Rank = ss.Rank,
                        TotalVotes = long.Parse(ss.Vote),
                        TotalVoters = voters.Count,
                    };

                    ss.DelegateStats.Add(sdelegateStat);
                }

                await _dbcontext.SaveChangesAsync();

                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException.ToString());
                return null;
            }
        }

        private async Task<List<Account>> GetVoters(string publickey)
        {
            VoterApiResult voterApiResult;

            try
            {
                // Retreive Quote
                using (var hc = new HttpClient())
                {
                    var result = JObject.Parse(await hc.GetStringAsync(_dashboardOptions.APIUrl + "/api/delegates/voters?publicKey=" + publickey));
                    voterApiResult = JsonConvert.DeserializeObject<VoterApiResult>(result.ToString());

                    return voterApiResult.Success ? voterApiResult.Accounts : null;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException.ToString());
                return null;
            }
        }

        /// <summary>
        /// Retreive All Delegate from Database
        /// </summary>
        /// <returns></returns>
        public async Task<Delegate[]> GetDelegatesFromDb()
        {
            return await _dbcontext.Delegates.Include(y=>y.DelegateStats.Where(x=>x.Date >= DateTime.Now.AddMinutes(-45)).OrderByDescending(s=>s.Id).Take(1)).OrderBy(x => x.Rank).ToArrayAsync();

         
        }

        /// <summary>
        ///  Retreive all Delegate From API
        /// </summary>
        /// <returns></returns>
        public async Task<List<Delegate>> GetDelegatesFromApiAsync()
        {
            try
            {
                // Retreive Quote
                using (var hc = new HttpClient())
                {
                    var result = JObject.Parse(await hc.GetStringAsync(_dashboardOptions.APIUrl + "/api/delegates"));
                    var delegateResult = JsonConvert.DeserializeObject<DelegateApiResult>(result.ToString());

                    var result102to203 = JObject.Parse(await hc.GetStringAsync(_dashboardOptions.APIUrl + "/api/delegates?offset=102"));
                    var delegate102to203 = JsonConvert.DeserializeObject<DelegateApiResult>(result102to203.ToString());

                    var result204to305 = JObject.Parse(await hc.GetStringAsync(_dashboardOptions.APIUrl + "/api/delegates?offset=204"));
                    var delegate204to305 = JsonConvert.DeserializeObject<DelegateApiResult>(result204to305.ToString());

                    
                    // Comvine all the Delegates in Response

                    foreach (var o in delegate102to203.Delegates)
                    {
                        delegateResult.Delegates.Add(o);
                    }

                    foreach (var o in delegate204to305.Delegates)
                    {
                        delegateResult.Delegates.Add(o);
                    }
                  

                    return delegateResult.Success && delegate102to203.Success && delegate204to305.Success ? delegateResult.Delegates : null;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException.ToString());
                return null;
            }
        }
    }
}