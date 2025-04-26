using Dapper;
using LumbiniCityTeacherSchedule.DataAccess.Data.ClassScheduleData;
using LumbiniCityTeacherSchedule.DataAccess.Data.JoinedTeacherAndAvailabilityData;
using LumbiniCityTeacherSchedule.DataAccess.Data.ProgramData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterConfigData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterInstanceData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SubjectData;
using LumbiniCityTeacherSchedule.DataAccess.Data.TeacherAssignmentData;
using LumbiniCityTeacherSchedule.DataAccess.Data.TeacherData;
using LumbiniCityTeacherSchedule.DataAccess.Data.TimeSlotData;
using LumbiniCityTeacherSchedule.DataAccess.DbAccess;
using LumbiniCityTeacherSchedule.Service.ProgramService;
using LumbiniCityTeacherSchedule.Utility.JsonConverter;
using LumbiniCityTeacherSchedule.Utility.SqlMapperHandler;

var builder = WebApplication.CreateBuilder(args);
//Register Custom sql Type Handlers
SqlMapper.AddTypeHandler(new TimeOnlyHandler());

//Register custom jsonConverter
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // JSON Converters
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
    });

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddScoped<IProgramData, ProgramData>();
builder.Services.AddScoped<ISemesterData, SemesterData>();
builder.Services.AddScoped<ISemesterConfigData, SemesterConfigData>();
builder.Services.AddScoped<ISemesterInstanceData, SemesterInstanceData>();
builder.Services.AddScoped<ISubjectData,SubjectData>();
//builder.Services.AddScoped<ITeacherData, TeacherData>();
//builder.Services.AddScoped<ITeacherAvailabilityData, TeacherAvailabilityData>();
builder.Services.AddScoped<IJoinedTeacherAndAvailabilityData,JoinedTeacherAndAvailabilityData>();
builder.Services.AddScoped<ITeacherAssignmentData,TeacherAssignmentData>();
builder.Services.AddScoped<ITimeSlotData,TimeSlotData>();
builder.Services.AddScoped<IClassScheduleData, ClassScheduleData>();
builder.Services.AddScoped<IProgramService, ProgramService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Enable Swagger only in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable Swagger JSON
    app.UseSwaggerUI(); // Enable Swagger UI
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllers();

app.Run();

