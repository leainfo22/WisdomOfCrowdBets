using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WisdomOfCrowndBets.Core.DTO.Api;
public class Odds 
{
    public string? key { get; set; } 
    public string? group { get; set; }
    public string? title { get; set; }
    public string? description { get; set; }
    public bool? active { get; set; }
    public bool? has_outrights { get; set; }
}

