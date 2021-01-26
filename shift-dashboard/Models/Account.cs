using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace shift_dashboard.Model
{
    [Index(nameof(Address), Name = "Index_Address", IsUnique = true)]
    public class Account
    {
        [Key]
        public int Id { get; set; }

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
        [JsonProperty("balance")]
        public string Balance { get; set; }

        [Required]
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        public virtual ICollection<Delegate> DelegatesVote { get; set; }
    }

    public class VoterApiResult
    {
        [JsonProperty("Success")]
        public bool Success { get; set; }

        [JsonProperty("accounts")]
        public List<Account> Accounts { get; set; }
    }
}