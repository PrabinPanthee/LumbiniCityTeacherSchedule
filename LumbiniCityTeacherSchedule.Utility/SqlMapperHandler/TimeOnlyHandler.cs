using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LumbiniCityTeacherSchedule.Utility.SqlMapperHandler
{
    public class TimeOnlyHandler : SqlMapper.TypeHandler<TimeOnly>
    {
        public override TimeOnly Parse(object value)
        {
            return TimeOnly.FromTimeSpan((TimeSpan)value);
        }

        public override void SetValue(IDbDataParameter parameter, TimeOnly value)
        {
            parameter.Value = value.ToTimeSpan();
        }
    }
}
