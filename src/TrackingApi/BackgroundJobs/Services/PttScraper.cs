using HtmlAgilityPack;
using System.Net;
using TrackingApi.Models;

namespace TrackingApi.BackgroundJobs.Services;


public class PttScraper : IPttScraper
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PttScraper> _logger;

    public PttScraper(HttpClient httpClient, ILogger<PttScraper> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
    }

    public async Task<TrackingItem> GetTrackingStatus(string trackingNumber)
    {
        try
        {
            // İlk sayfa için GET isteği
            var initialResponse = await _httpClient.GetAsync("https://gonderitakip.ptt.gov.tr");
            if (!initialResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Initial page load failed: {initialResponse.StatusCode}");
            }

            // Form verisi hazırlama
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("q", trackingNumber)
            });

            // Takip sorgusu için POST isteği
            var response = await _httpClient.PostAsync("https://gonderitakip.ptt.gov.tr/Track/summaryResult", content);
            var htmlContent = await response.Content.ReadAsStringAsync();


            var result = ParseTrackingPage(htmlContent,trackingNumber);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error tracking number: {TrackingNumber}", trackingNumber);
            throw;
        }
    }

    private TrackingItem ParseTrackingPage(string html,string trackingNumber)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var result = new TrackingItem()
        {
            TrackingNumber = string.IsNullOrEmpty(GetNodeText(doc, "//h8[contains(text(),'TAKİP NO')]/span")) 
                ? trackingNumber 
                : GetNodeText(doc, "//h8[contains(text(),'TAKİP NO')]/span"),
            Status = WebUtility.HtmlDecode(GetNodeText(doc, "//div[@class='col-8']//span")),
            LastUpdate = DateTime.Now,
            Details = new List<TrackingDetail>()
        };

        if (string.IsNullOrEmpty(result.Status))
        {
            result.Status = "Kayıt Bulunamadı";
            return result;
        }

        // Hareket tablosunu parse et
       
        var rows = doc.DocumentNode.SelectNodes("//div[contains(@class,'collapse')]//table//tbody//tr")
                   ?? doc.DocumentNode.SelectNodes("//table[contains(@class,'table')]//tbody//tr");
        if (rows != null)
        {
            foreach (var row in rows)
            {
                var cells = row.SelectNodes(".//td");
                if (cells?.Count >= 5)
                {
                    result.Details.Add(new TrackingDetail
                    {
                        Date = ParseDateTime(WebUtility.HtmlDecode(cells[0].InnerText.Trim())),
                        Operation = WebUtility.HtmlDecode(cells[1].InnerText.Trim()),
                        Branch = WebUtility.HtmlDecode(cells[2].InnerText.Trim()),
                        Location = WebUtility.HtmlDecode(cells[3].InnerText.Trim()),
                        Description = WebUtility.HtmlDecode(cells[4].InnerText.Trim())
                    });
                }
            }
        }

        return result;
    }

    private string GetNodeText(HtmlDocument doc, string xpath)
    {
        var node = doc.DocumentNode.SelectSingleNode(xpath);
        return WebUtility.HtmlDecode(node?.InnerText.Trim() ?? string.Empty);
    }

    private DateTime ParseDateTime(string dateTimeStr)
    {
        if (DateTime.TryParseExact(dateTimeStr, "dd/MM/yyyy - HH:mm:ss",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None,
            out DateTime result))
        {
            return result;
        }
        return DateTime.Now;
    }
}


