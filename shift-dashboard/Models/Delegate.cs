using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace shift_dashboard.Models
{
    [Index(nameof(Address), Name = "Index_Address", IsUnique = true)]
    public partial class Delegate
    {
        public Delegate()
        {
            this.DelegateStats = new HashSet<DelegateStat>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [JsonProperty("username")]
        public string Username { get; set; }

        [Required]
        [MaxLength(25)]
        [JsonProperty("address")]
        public string Address { get; set; }

        [Required]
        [MaxLength(64)]
        [JsonProperty("publicKey")]
        public string PublicKey { get; set; }

        [MaxLength(24)]
        [JsonProperty("vote")]
        public string Vote { get; set; }

        [JsonProperty("producedblocks")]
        public int Producedblocks { get; set; }

        [JsonProperty("missedblocks")]
        public int Missedblocks { get; set; }

        [JsonProperty("rate")]
        public int Rate { get; set; }

        [JsonProperty("rank")]
        public int Rank { get; set; }

        [JsonProperty("approval")]
        public double Approval { get; set; }

        [JsonProperty("productivity")]
        public double Productivity { get; set; }

        public int PoolShare { get; set; }

        public DateTime Date { get; set; }

        public virtual ICollection<DelegateStat> DelegateStats { get; set; }
    }

    public partial class Delegate
    {
        private IEnumerable<DelegateStat> DailyDelegateStats
        {
            get
            {
                try
                {
                    DateTime StatsDate = DateTime.Now.AddDays(-1);

                    return this.DelegateStats.Where(x => x.Date >= StatsDate).OrderBy(p => p.Date) ?? null;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }


        public IEnumerable<long> VotePerDay
        {
            get
            {
                try
                {
                    DateTime StatsDate = DateTime.Now.AddDays(-30);
                    var result =  this.DelegateStats.Where(x => x.Date >= StatsDate).GroupBy(i => new { i.Date.Year, i.Date.Month, i.Date.Day }).Select(g => Convert.ToInt64(g.Average(p=>p.TotalVotes)/100000000));
                    return result;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public int NbVoters
        {
            get
            {
                try
                {
                    if (this.DailyDelegateStats != null && this.DailyDelegateStats.Any())
                    {
                        return this.DailyDelegateStats.OrderByDescending(p => p.Date).FirstOrDefault().TotalVoters;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch
                {
                    return 0;
                }
            }
        }

        public int VotersDailyChange
        {
            get
            {
                try
                {
                    if (this.DailyDelegateStats.Any())
                    {
                        return this.NbVoters - this.DailyDelegateStats.FirstOrDefault().TotalVoters ;
                    }
                    else
                    {
                        return this.NbVoters;
                    }
                }
                catch
                {
                    return 0;
                }
            }
        }

        public int RankDailyChange
        {
            get
            {
                try
                {
                    if (this.DailyDelegateStats.Any())
                    {
                        return this.Rank - this.DailyDelegateStats.FirstOrDefault().Rank;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch
                {
                    return 0;
                }
            }
        }

        public long VotesDailyChange
        {
            get
            {
                try
                {
                    if (this.DailyDelegateStats.Any())
                    {
                        return (long.Parse(this.Vote) - this.DailyDelegateStats.FirstOrDefault().TotalVotes) / 100000000;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch
                {
                    return 0;
                }
            }
        }

        public class DelegateApiResult
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("delegates")]
            public List<Delegate> Delegates { get; set; }

            [JsonProperty("totalCount")]
            public int TotalCount { get; set; }
        }
    }
}