using Platform.API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiperRestApiPackSample.Models
{
    public class Model
    {
        public int Field1 { get; set; }
        public int Field2 { get; set; }
        public int Field3 { get; set; }
        public List<Item> Items { get; set; }

    }
    public class Item
    {
        public int Field1 { get; set; }
        public int Field2 { get; set; }
        public int Field3 { get; set; }
    }

    public class Group : ModelBase
    {
        public string Name { get; set; }
        public string MasterId { get; set; } = null;
        public string CompanyId { get; set; }
        public int Credit { get; set; }
        public bool CanFutureTrading { get; set; }
        public bool CanTrade { get; set; } = false;
        public int MaxHistory { get; set; }
        public string Message { get; set; }
        public Dictionary<string, GroupSymbol> GroupSymbols { get; set; }
    }

    public class GroupSymbol
    {
        public string Name { get; set; }
        public int FutureLimit { get; set; }
        public Markup Markup { get; set; } = new Markup();
        public NextTreeDays NextTreeDays { get; set; } = new NextTreeDays();
        public double LongSwap { get; set; }
        public double ShortSwap { get; set; }
        public int? MinVolume { get; set; }
        public int? MaxVolume { get; set; }

        public List<SpecialDay> SpecialDays { get; set; } = null;
        public int? MinimumSpread { get; set; } = null; //Maximum Spread (in Points)
        public int? MaximumSpread { get; set; } = null; //Maximum Spread (in Points)
        public int? MaximumDeviation { get; set; } = null; //Maximum Deviation (in Points)
        public string Collection { get; set; } = "Market Watch";
    }

    public class NextTreeDays
    {
        public double T1BidSpread { get; set; }
        public double T1AskSpread { get; set; }
        public double T2BidSpread { get; set; }
        public double T2AskSpread { get; set; }
        public double T3BidSpread { get; set; }
        public double T7AskSpread { get; set; }
        public double T7BidSpread { get; set; }
        public double T30AskSpread { get; set; }
        public double T30BidSpread { get; set; }
        public double T3AskSpread { get; set; }
    }

    public class SpecialDay
    {
        public DateTime Date { get; set; } = DateTime.Now.Date;
        public Markup Markup { get; set; }
        public bool CanTradeLong { get; set; }
        public bool CanTradeShort { get; set; }
    }

    public class Markup
    {
        public int Markup_ask { get; set; }
        public int Markup_bid { get; set; }
    }
}
