﻿using Microsoft.EntityFrameworkCore;
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

        public DateTime Date { get; set; }

        public virtual ICollection<DelegateStat> DelegateStats { get; set; }
    }

    public partial class Delegate
    {
        public int NbVoters
        {
            get
            {
                try
                {
                    return this.DelegateStats.Where(x => x.Date >= DateTime.Now.AddDays(-1)).OrderByDescending(p => p.Date).FirstOrDefault().TotalVoters;
                }
                catch (Exception e)
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
                    return this.DelegateStats.Where(x => x.Date >= DateTime.Now.AddDays(-1)).OrderBy(p => p.Date).FirstOrDefault().TotalVoters - this.NbVoters;
                }
                catch (Exception e)
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
                    return this.Rank - this.DelegateStats.Where(x => x.Date >= DateTime.Now.AddDays(-1)).OrderBy(p => p.Date).FirstOrDefault().Rank;
                }
                catch (Exception e)
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
                    return (long.Parse(this.Vote) - this.DelegateStats.Where(x => x.Date >= DateTime.Now.AddDays(-1)).OrderBy(p => p.Date).FirstOrDefault().TotalVotes) / 100000000;
                }
                catch (Exception e)
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