using Newtonsoft.Json;

string apiKey = "YOUR_API_KEY";
string zoneId = "ZONE_ID";
string apiUrl = $"https://api.cloudflare.com/client/v4/zones/{zoneId}/dns_records/";

using (HttpClient client = new HttpClient())
{
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

    HttpResponseMessage response = await client.GetAsync(apiUrl);

    if (response.IsSuccessStatusCode)
    {
        string responseBody = await response.Content.ReadAsStringAsync();
        CloudFlareApiResponse cloudFlareResponse = JsonConvert.DeserializeObject<CloudFlareApiResponse>(responseBody);

        foreach (var item in cloudFlareResponse.result)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine($"DNS Record Id: {item.id}");
            HttpResponseMessage delResponse = await client.DeleteAsync($"{apiUrl}/{item.id}");
            if (delResponse.IsSuccessStatusCode)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Deleted: {item.id}");
            }
            Console.ResetColor();
            Console.WriteLine("------------------->");
        }
    }

    else
    {
        Console.WriteLine($"ErrorCode: {response.StatusCode}");
    }
}


public class CloudFlareApiResponse
{
    public List<CloudFlareDnsRecord> result { get; set; }
    public bool success { get; set; }
    public List<object> errors { get; set; }
    public List<object> messages { get; set; }
    public ResultInfo result_info { get; set; }
}

public class CloudFlareDnsRecord
{
    public string id { get; set; }
    public string zone_id { get; set; }
    public string zone_name { get; set; }
    public string name { get; set; }
    public string type { get; set; }
    public string content { get; set; }
    public bool proxiable { get; set; }
    public bool proxied { get; set; }
    public int ttl { get; set; }
    public bool locked { get; set; }
    public Meta meta { get; set; }
    public object comment { get; set; }
    public List<object> tags { get; set; }
    public DateTime created_on { get; set; }
    public DateTime modified_on { get; set; }
}

public class Meta
{
    public bool auto_added { get; set; }
    public bool managed_by_apps { get; set; }
    public bool managed_by_argo_tunnel { get; set; }
    public string source { get; set; }
}

public class ResultInfo
{
    public int page { get; set; }
    public int per_page { get; set; }
    public int count { get; set; }
    public int total_count { get; set; }
    public int total_pages { get; set; }
}