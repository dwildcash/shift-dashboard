using Microsoft.EntityFrameworkCore;
using System;

namespace shift_dashboard.Models
{
    [Index(nameof(Date), Name = "Index_Date", IsUnique = false)]
    public class DelegateStat
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int TotalVoters { get; set; }

        public int Rank { get; set; }

        public long TotalVotes { get; set; }

        public int DelegateId { get; set; }

        public Delegate Delegate { get; set; }
    }
}