using System;
using System.Threading.Tasks;
using Supabase;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;
using DotNetEnv;

namespace SupabaseTestApp
{
    [Table("events")]
    public class ACLEDEvent : BaseModel
    {
        [PrimaryKey("event_id_cnty")]
        public string EventIdCnty { get; set; } = string.Empty;

        [Column("event_date")]
        public DateTime? EventDate { get; set; }

        [Column("year")]
        public int? Year { get; set; }

        [Column("time_precision")]
        public int? TimePrecision { get; set; }

        [Column("disorder_type")]
        public string? DisorderType { get; set; }

        [Column("event_type")]
        public string? EventType { get; set; }

        [Column("sub_event_type")]
        public string? SubEventType { get; set; }

        [Column("actor1")]
        public string? Actor1 { get; set; }

        [Column("assoc_actor_1")]
        public string? AssocActor1 { get; set; }

        [Column("inter1")]
        public int? Inter1 { get; set; }

        [Column("actor2")]
        public string? Actor2 { get; set; }

        [Column("assoc_actor_2")]
        public string? AssocActor2 { get; set; }

        [Column("inter2")]
        public int? Inter2 { get; set; }

        [Column("interaction")]
        public int? Interaction { get; set; }

        [Column("civilian_targeting")]
        public string? CivilianTargeting { get; set; }

        [Column("iso")]
        public string? ISO { get; set; }

        [Column("region")]
        public string? Region { get; set; }

        [Column("country")]
        public string? Country { get; set; }

        [Column("admin1")]
        public string? Admin1 { get; set; }

        [Column("admin2")]
        public string? Admin2 { get; set; }

        [Column("admin3")]
        public string? Admin3 { get; set; }

        [Column("location")]
        public string? Location { get; set; }

        [Column("latitude")]
        public decimal? Latitude { get; set; }

        [Column("longitude")]
        public decimal? Longitude { get; set; }

        [Column("geo_precision")]
        public int? GeoPrecision { get; set; }

        [Column("source")]
        public string? Source { get; set; }

        [Column("source_scale")]
        public string? SourceScale { get; set; }

        [Column("notes")]
        public string? Notes { get; set; }

        [Column("fatalities")]
        public int? Fatalities { get; set; }

        [Column("tags")]
        public string? Tags { get; set; }

        [Column("timestamp")]
        public long? Timestamp { get; set; }
    }

    [Table("countries")]
    public class CIAFactbookCountry : BaseModel
    {
        [PrimaryKey("country_name")]
        public string CountryName { get; set; } = string.Empty;

        [Column("category")]
        public string Category { get; set; } = string.Empty;

        [Column("subcategory_level1")]
        public string SubcategoryLevel1 { get; set; } = string.Empty;

        [Column("subcategory_level2")]
        public string SubcategoryLevel2 { get; set; } = string.Empty;

        [Column("subcategory_level3")]
        public string SubcategoryLevel3 { get; set; } = string.Empty;

        [Column("data")]
        public string? Data { get; set; }

        [Column("last_updated")]
        public DateTime? LastUpdated { get; set; }
    }

    [Table("indicators")]
    public class WorldBankIndicator : BaseModel
    {
        [PrimaryKey("country_name")]
        public string CountryName { get; set; } = string.Empty;

        [Column("country_code")]
        public string? CountryCode { get; set; }

        [Column("indicator_name")]
        public string? IndicatorName { get; set; }

        [Column("indicator_code")]
        public string? IndicatorCode { get; set; }

        [Column("year")]
        public int? Year { get; set; }

        [Column("value")]
        public decimal? Value { get; set; }
    }

    class Program
    {
        private static Supabase.Client _acledClient = null!;
        private static Supabase.Client _ciaFactbookClient = null!;
        private static Supabase.Client _worldBankClient = null!;

        static async Task Main(string[] args)
        {
            try
            {
                DotNetEnv.Env.Load();
                string supabaseUrl = Environment.GetEnvironmentVariable("SUPABASE_URL") ?? throw new Exception("SUPABASE_URL is not set in .env file");
                string supabaseKey = Environment.GetEnvironmentVariable("SUPABASE_KEY") ?? throw new Exception("SUPABASE_KEY is not set in .env file");

                _acledClient = await InitializeClient(supabaseUrl, supabaseKey, "acled");
                _ciaFactbookClient = await InitializeClient(supabaseUrl, supabaseKey, "cia_factbook");
                _worldBankClient = await InitializeClient(supabaseUrl, supabaseKey, "world_bank");

                await QueryAcledEvents();
                await QueryCIAFactbookCountries();
                await QueryWorldBankIndicators();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task<Supabase.Client> InitializeClient(string url, string key, string schema)
        {
            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true,
                Schema = schema
            };
            var client = new Supabase.Client(url, key, options);
            await client.InitializeAsync();
            return client;
        }

        static async Task QueryAcledEvents()
        {
            Console.WriteLine("Fetching data from 'acled.events'...");
            var response = await _acledClient
                .From<ACLEDEvent>()
                .Select(x => new object?[] { x.EventIdCnty, x.EventDate, x.DisorderType, x.Fatalities })
                .Where(x => x.EventDate >= new DateTime(2000, 1, 1))
                .Limit(5)
                .Get();

            foreach (var acledEvent in response.Models)
            {
                Console.WriteLine($"Event ID: {acledEvent.EventIdCnty}, Date: {acledEvent.EventDate}, Disorder: {acledEvent.DisorderType ?? "N/A"}, Fatalities: {acledEvent.Fatalities}");
            }
        }

        static async Task QueryCIAFactbookCountries()
        {
            Console.WriteLine("\nFetching data from 'cia_factbook.countries'...");
            var response = await _ciaFactbookClient
                .From<CIAFactbookCountry>()
                .Select(x => new object?[] { x.CountryName, x.Category, x.Data })
                .Filter(x => x.CountryName, Supabase.Postgrest.Constants.Operator.Equals, "Jordan")
                .Get();

            foreach (var country in response.Models)
            {
                Console.WriteLine($"Country: {country.CountryName}, Category: {country.Category}, Data: {country.Data ?? "N/A"}");
            }
        }

        static async Task QueryWorldBankIndicators()
        {
            Console.WriteLine("\nFetching data from 'world_bank.indicators'...");
            var response = await _worldBankClient
                .From<WorldBankIndicator>()
                .Select(x => new object?[] { x.CountryName, x.IndicatorName, x.Year, x.Value })
                .Where(x => x.CountryName == "Jordan" && x.Year >= 2000)
                .Limit(5)
                .Get();

            foreach (var indicator in response.Models)
            {
                Console.WriteLine($"Country: {indicator.CountryName}, Indicator: {indicator.IndicatorName ?? "N/A"}, Year: {indicator.Year}, Value: {indicator.Value}");
            }
        }
    }
}