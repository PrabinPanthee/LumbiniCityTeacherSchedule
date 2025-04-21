using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LumbiniCityTeacherSchedule.Utility.JsonConverter
{
    public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>

    {
        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => TimeOnly.Parse(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString("HH:mm"));

    }

    //Summary As Date only and time only doesn't bind or work properly in json serialization and deserialization we modify the existing logic
    //to work with our set up this is for the json or web api 
}
