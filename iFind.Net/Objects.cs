using iFind.Net.Consts;
using System.Text.Json;
using System.Text.Json.Serialization;

#pragma warning disable IDE0130
namespace iFind.Net.Objects;

public abstract class RestResult
{
    [JsonPropertyName("errorcode")]
    public int ErrorCode { get; set; }

    [JsonPropertyName("errmsg")]
    public string? ErrorMessage { get; set; }

    [JsonPropertyName("raw")]
    public string? Raw { get; set; }
}

public class DataResult<T> : RestResult
{
    [JsonPropertyName("data")]
    public T? Data { get; set; }
}

public class AccessToken
{
    [JsonPropertyName("access_token")]
    public string? Token { get; set; }

    [JsonPropertyName("expired_time")]
    public DateTime ExpiredTime { get; set; }
}


public class TablesResult<T> : RestResult
{
    [JsonPropertyName("tables")]
    public T? Tables { get; set; }

    [JsonPropertyName("inputParams")]
    public Dictionary<string, object>? InputParams { get; set; }

    [JsonPropertyName("outParams")]
    public Dictionary<string, string>? OutputParams { get; set; }

    [JsonPropertyName("dataVol")]
    public int DataVolume { get; set; }
}

public class Table
{
    [JsonPropertyName("time")]
    public DateTime[]? Time { get; set; }
}

public class Table<C> where C : Const
{
    [JsonPropertyName("time")]
    public DateTime[]? Time { get; set; }

    [JsonPropertyName("thscode")]
    public string? Code { get; set; }

    [JsonPropertyName("table")]
    public Dictionary<string, JsonElement[]>? Data { get; set; }

    public IEnumerable<T> GetCol<T>(C col)
    {
        if ((Data?.TryGetValue(col.Value, out var values)) != true)
            throw new Exception("col not exists");
        foreach (var value in values ?? [])
            yield return value.Deserialize<T>()!;
    }

    public T? GetValue<T>(C col, int row)
    {
        if ((Data?.TryGetValue(col.Value, out var values)) != true)
            throw new Exception("col not exists");
        else if (values is null)
            throw new Exception("col null");
        try
        {
            if (values.Length > row)
                return values.ElementAt(row).Deserialize<T>();
            else
                return default;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
