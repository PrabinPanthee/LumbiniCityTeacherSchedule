using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LumbiniCityTeacherSchedule.Utility.JsonConverter
{
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private const string Format = "yyyy-MM-dd";
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => DateOnly.ParseExact(reader.GetString()!,Format,CultureInfo.InvariantCulture);
        
        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(Format));
          
        
    }

    //Summary As Date only and time only doesn't bind or work properly in json serialization and deserialization we modify the existing logic
    //to work with our set up this is for the json or web api 
}
