using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// see https://launchlibrary.net/docs/1.4.1/api.html#launch

namespace Space_Launches.Models
{
    // with help from https://quicktype.io/
    public class LaunchModel
    {
        public Guid Id { get; set; }
        public Uri Url { get; set; }
        public long Launch_Library_Id { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }

        public Status Status { get; set; }
        public DateTimeOffset Net { get; set; }
        
        public DateTimeOffset WindowEnd { get; set; }
        
        public DateTimeOffset WindowStart { get; set; }
        
        public bool Inhold { get; set; }
        
        public bool Tbdtime { get; set; }
        
        public bool Tbddate { get; set; }
        
        public long Probability { get; set; }
        
        public string Holdreason { get; set; }
        
        public string Failreason { get; set; }
        
        public string Hashtag { get; set; }
        
        // Keeping the use case simple for now

        /*
        public LaunchServiceProvider LaunchServiceProvider { get; set; }
        
        public Rocket Rocket { get; set; }
        
        public object Mission { get; set; }
        
        public Pad Pad { get; set; }
        
        public bool WebcastLive { get; set; }
        
        public Uri Image { get; set; }
        
        public object Infographic { get; set; }
        */
    }

    /*
    public partial class LaunchServiceProvider
    {
        
        public long Id { get; set; }
        
        public Uri Url { get; set; }
        
        public string Name { get; set; }
        
        public string Type { get; set; }
    }
    public partial class Pad
    {
        
        public long Id { get; set; }
        
        public Uri Url { get; set; }
        
        public long? AgencyId { get; set; }
        
        public string Name { get; set; }
        
        public object InfoUrl { get; set; }
        
        public string WikiUrl { get; set; }
        
        public Uri MapUrl { get; set; }
        
        public string Latitude { get; set; }
        
        public string Longitude { get; set; }
        
        public Location Location { get; set; }
        
        public Uri MapImage { get; set; }
        
        public long TotalLaunchCount { get; set; }
    }
    public partial class Location
    {
        
        public long Id { get; set; }
        
        public Uri Url { get; set; }
        
        public string Name { get; set; }
        
        public CountryCode CountryCode { get; set; }
        
        public Uri MapImage { get; set; }
        
        public long TotalLaunchCount { get; set; }
        
        public long TotalLandingCount { get; set; }
    }
    public partial class Rocket
    {
        
        public long Id { get; set; }
        
        public Configuration Configuration { get; set; }
    }
    public partial class Configuration
    {
        
        public long Id { get; set; }
        
        public long LaunchLibraryId { get; set; }
        
        public Uri Url { get; set; }
        
        public string Name { get; set; }
        
        public string Family { get; set; }
        
        public string FullName { get; set; }
        
        public string Variant { get; set; }
    }
    */

    public partial class Status
    {        
        public long Id { get; set; }
        
        public string Name { get; set; }
    }

    /*
    public enum CountryCode { Kaz, Usa };
    public enum Name { Failure, PartialFailure, Success };
    */

    /*
    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                CountryCodeConverter.Singleton,
                NameConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class CountryCodeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(CountryCode) || t == typeof(CountryCode?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "KAZ":
                    return CountryCode.Kaz;
                case "USA":
                    return CountryCode.Usa;
            }
            throw new Exception("Cannot unmarshal type CountryCode");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (CountryCode)untypedValue;
            switch (value)
            {
                case CountryCode.Kaz:
                    serializer.Serialize(writer, "KAZ");
                    return;
                case CountryCode.Usa:
                    serializer.Serialize(writer, "USA");
                    return;
            }
            throw new Exception("Cannot marshal type CountryCode");
        }

        public static readonly CountryCodeConverter Singleton = new CountryCodeConverter();
    }

    internal class NameConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Name) || t == typeof(Name?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Failure":
                    return Name.Failure;
                case "Partial Failure":
                    return Name.PartialFailure;
                case "Success":
                    return Name.Success;
            }
            throw new Exception("Cannot unmarshal type Name");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Name)untypedValue;
            switch (value)
            {
                case Name.Failure:
                    serializer.Serialize(writer, "Failure");
                    return;
                case Name.PartialFailure:
                    serializer.Serialize(writer, "Partial Failure");
                    return;
                case Name.Success:
                    serializer.Serialize(writer, "Success");
                    return;
            }
            throw new Exception("Cannot marshal type Name");
        }

        public static readonly NameConverter Singleton = new NameConverter();
    }
    */

}

