#pragma warning disable IDE0130
using System.Text.Json.Serialization;

namespace iFind.Net.Consts;

public abstract class Const(string value)
{
    public string Value = value;
    public override string? ToString() => Value;
}

public class MarketCode(string value) : Const(value)
{
    public static MarketCode SSE { get; } = new("212001");
    public static MarketCode SZSE { get; } = new("212100");
    public static MarketCode BSE { get; } = new("212300");

    public static MarketCode SHFE { get; } = new("212020008");
    public static MarketCode DCE { get; } = new("212020004");
    public static MarketCode CZCE { get; } = new("212020003");
    public static MarketCode GFEX { get; } = new("212020019");
    public static MarketCode CFFEX { get; } = new("212020001");
}

public class DateType(string value) : Const(value)
{
    public static DateType TradeDay { get; } = new("0");
    public static DateType CalendarDay { get; } = new("1");
    public static DateType WorkDay { get; } = new("2");
}

public class DateDelimiter(string value) : Const(value)
{
    public static DateDelimiter Dash { get; } = new("0");
    public static DateDelimiter Slash { get; } = new("1");
    public static DateDelimiter None { get; } = new("2");
}

public class ReportName(string value) : Const(value)
{
    public static ReportName Constituents { get; } = new("p03291");
}

public class BlockName(string value) : Const(value)
{
    public static BlockName CnAShares { get; } = new("001005010");
    public static BlockName CnContracts { get; } = new("091013");
    public static BlockName CnETF { get; } = new("051001006");
    public static BlockName ThsCommodityIndex { get; } = new("081005009");
    public static BlockName ThsIndustryIndex { get; } = new("081001109001");
    public static BlockName FxBasic { get; } = new("101003");
    public static BlockName LdGold { get; } = new("091017");
}

public class IVType(string value) : Const(value)
{
    public static IVType AllContract { get; } = new("allcontract");
    public static IVType MonthContract { get; } = new("monthcontract");
    public static IVType ProcessContract { get; } = new("processcontract");
}

public class ConstituentsOutputPara(string value) : Const(value)
{
    public static ConstituentsOutputPara Date { get; } = new("p03291_f001");
    public static ConstituentsOutputPara ThsCode { get; } = new("p03291_f002");
    public static ConstituentsOutputPara Name { get; } = new("p03291_f003");
}

public class HistoryIndicator(string value) : Const(value)
{
    public static HistoryIndicator PreClose { get; } = new("preClose");
    public static HistoryIndicator PreSettle { get; } = new("preSettlement");
    public static HistoryIndicator Open { get; } = new("open");
    public static HistoryIndicator High { get; } = new("high");
    public static HistoryIndicator Low { get; } = new("low");
    public static HistoryIndicator Close { get; } = new("close");
    public static HistoryIndicator Settle { get; } = new("settlement");
    public static HistoryIndicator Volume { get; } = new("volume");
    public static HistoryIndicator Amount { get; } = new("amount");
    public static HistoryIndicator PE { get; } = new("pe");
    public static HistoryIndicator PB { get; } = new("pb");
    public static HistoryIndicator Adjust { get; } = new("adjustmentFactorBackward1");
    public static HistoryIndicator TotalShares { get; } = new("totalShares");
    public static HistoryIndicator FloatShares { get; } = new("floatSharesOfAShares");
    public static HistoryIndicator OpenInterest { get; } = new("openInterest");
    public static HistoryIndicator TotalCapital { get; } = new("totalCapital");
    public static HistoryIndicator FloatCapital { get; } = new("floatCapital");
    public static HistoryIndicator PE_Index_TTM { get; } = new("pe_ttm_index");
    public static HistoryIndicator PE_Index_Publisher { get; } = new("pe_indexPublisher");
    public static HistoryIndicator PB_MRQ { get; } = new("pb_mrq");
}

public class RealTimeIndicator(string value) : Const(value)
{
    public static RealTimeIndicator Open { get; } = new("open");
    public static RealTimeIndicator Latest { get; } = new("latest");
    public static RealTimeIndicator Amount { get; } = new("amount");
    public static RealTimeIndicator LastestPrice { get; } = new("lastest_price");
    public static RealTimeIndicator Adjust { get; } = new("af_backward");
}

public class BasicDataIndicator(string value) : Const(value)
{
    [JsonPropertyName("indicator")]
    public string Indicator { get; } = value;

    public static BasicDataIndicator TrackingIndexThscode { get; } = new("ths_tracking_index_thscode_fund");
    public static BasicDataIndicator IndexShortName { get; } = new("ths_index_short_name_index");
    public static BasicDataIndicator VarietyFuture { get; } = new("ths_td_variety_future");
}
