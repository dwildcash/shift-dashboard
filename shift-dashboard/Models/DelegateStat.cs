using System;
using System.Collections.Generic;

namespace shift_dashboard.Model
{
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