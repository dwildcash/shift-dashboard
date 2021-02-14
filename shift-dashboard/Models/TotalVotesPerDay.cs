using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shift_dashboard.Models
{
    public class TotalVotesPerDay
    {
        public DateTime Date { get; set; }
        public long TotalVotes { get; set; }
    }
}
