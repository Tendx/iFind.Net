using iFind.Net.Consts;
using iFind.Net.Objects;
using iFind.Net.Utils;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace iFind.Net;

#pragma warning disable IDE1006
public class iFinDRestApi(string? baseUri = null, bool raw = false)
{
    public Action<RestResult>? OnError;

    public const string BaseUrl1 = "https://quantapi.51ifind.com";
    public const string BaseUrl2 = "https://ft.10jqka.com.cn";

    private readonly HttpClient _rest = new() { BaseAddress = new(baseUri ?? BaseUrl1) };
    private string? _accessToken;

    public async Task<DataResult<AccessToken>> GetAccessToken(string refreshToken)
    {
        var result = await Request<DataResult<AccessToken>>("api/v1/get_access_token", null, [("refresh_token", refreshToken)]);
        if (result.ErrorCode == 0)
            _accessToken = result.Data?.Token;
        return result;
    }

    public async Task<DataResult<AccessToken>> UpdateAccessToken(string refreshToken)
    {
        var result = await Request<DataResult<AccessToken>>("api/v1/update_access_token", null, [("refresh_token", refreshToken)]);
        if (result.ErrorCode == 0)
            _accessToken = result.Data?.Token;
        return result;
    }

    public async Task<TablesResult<Table>> GetTradeDates(DateTime startDate, DateTime endDate, MarketCode? market = null, DateType? dateType = null)
    {
        var body = new Dictionary<string, object>()
        {
            ["marketcode"] = market?.Value ?? MarketCode.SSE.Value,
            ["functionpara"] = new Dictionary<string, string>()
            {
                ["mode"] = "1",
                ["dateType"] = dateType?.Value ?? DateType.TradeDay.Value,
                ["period"] = "D",
                ["dateFormat"] = DateDelimiter.Slash.Value,
            },
            ["startdate"] = startDate.ToShortDateString(),
            ["enddate"] = endDate.ToShortDateString(),
        };
        return await Request<TablesResult<Table>>("api/v1/get_trade_dates", body);
    }

    public async Task<TablesResult<Table<ConstituentsOutputPara>[]>> DataPool_GetConstituents(DateTime date, BlockName blockName, IVType? ivType = null, IEnumerable<ConstituentsOutputPara>? paras = null)
    {
        ivType ??= IVType.AllContract;
        paras ??= [ConstituentsOutputPara.Date, ConstituentsOutputPara.ThsCode, ConstituentsOutputPara.Name];
        var body = new Dictionary<string, object>()
        {
            ["reportname"] = ReportName.Constituents.Value,
            ["functionpara"] = new Dictionary<string, string>()
            {
                ["date"] = date.ToString("yyyyMMdd"),
                ["blockname"] = blockName.Value,
                ["iv_type"] = ivType.Value,
            },
            ["outputpara"] = string.Join(',', paras.Select(p => p.Value)),
        };
        return await Request<TablesResult<Table<ConstituentsOutputPara>[]>>("api/v1/data_pool", body, null);
    }

    public async Task<TablesResult<Table<BasicDataIndicator>[]>> BasicDataService(IEnumerable<string> codes, IEnumerable<BasicDataIndicator> indicators)
    {
        var body = new Dictionary<string, object>()
        {
            ["codes"] = codes,
            ["indipara"] = indicators,
        };
        return await Request<TablesResult<Table<BasicDataIndicator>[]>>("api/v1/basic_data_service", body, null);
    }

    public async Task<TablesResult<Table<HistoryIndicator>[]>> CmdHistoryQuotation(DateTime startDate, DateTime endDate, IEnumerable<string> codes, IEnumerable<HistoryIndicator> indicators)
    {
        var body = new Dictionary<string, object>()
        {
            ["codes"] = string.Join(",", codes),
            ["indicators"] = string.Join(",", indicators.Select(i => i.Value)),
            ["startdate"] = startDate.ToString("yyyy-MM-dd"),
            ["enddate"] = endDate.ToString("yyyy-MM-dd"),
            ["functionpara"] = new Dictionary<string, string>()
            {
                ["Fill"] = "Omit", // Blank: null
            },
        };
        return await Request<TablesResult<Table<HistoryIndicator>[]>>("api/v1/cmd_history_quotation", body, null);
    }

    private async Task<T> Request<T>(string path, IDictionary<string, object>? body = null, IEnumerable<ValueTuple<string, string>>? headers = null) where T : RestResult, new()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, path)
        {
            Content = JsonContent.Create(body, new MediaTypeHeaderValue("application/json")),
        };
        if (headers is not null)
        {
            foreach (var header in headers)
                request.Headers.Add(header.Item1, header.Item2);
        }
        else if (!string.IsNullOrEmpty(_accessToken))
            request.Headers.Add("access_token", _accessToken);

        string? text = null;
        T? result;
        HttpResponseMessage? response;
        try
        {
            response = await _rest.SendAsync(request);
        }
        catch (Exception ex)
        {
            result = new T { ErrorCode = -1, ErrorMessage = ex.Message };
            OnError?.Invoke(result);
            return result;
        }
        try
        {
            if (raw)
                text = await response.Content.ReadAsStringAsync();
            result = await response.Content.ReadFromJsonAsync<T>(JsonHelper.JsonSerializerOptions);
            result!.Raw = text;
        }
        catch (Exception ex)
        {
            result = new T { ErrorCode = -1, ErrorMessage = ex.Message, Raw = text };
            OnError?.Invoke(result);
            return result;
        }
        if (result!.ErrorCode != 0)
            OnError?.Invoke(result);
        return result;
    }
}
