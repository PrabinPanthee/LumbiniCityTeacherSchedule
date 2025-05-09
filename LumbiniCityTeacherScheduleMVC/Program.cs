using Dapper;
using LumbiniCityTeacherSchedule.DataAccess.Data.ClassScheduleData;
using LumbiniCityTeacherSchedule.DataAccess.Data.JoinedTeacherAndAvailabilityData;
using LumbiniCityTeacherSchedule.DataAccess.Data.ProgramData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterConfigData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterInstanceData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SubjectData;
using LumbiniCityTeacherSchedule.DataAccess.Data.TeacherAssignmentData;
using LumbiniCityTeacherSchedule.DataAccess.Data.TimeSlotData;
using LumbiniCityTeacherSchedule.DataAccess.DbAccess;
using LumbiniCityTeacherSchedule.Service.ConfigurationService;
using LumbiniCityTeacherSchedule.Service.ProgramService;
using LumbiniCityTeacherSchedule.Service.SemesterScheduleConfigService;
using LumbiniCityTeacherSchedule.Service.SemesterService;
using LumbiniCityTeacherSchedule.Service.SubjectService;
using LumbiniCityTeacherSchedule.Service.TeacherAssignmentService;
using LumbiniCityTeacherSchedule.Service.TeacherService;
using LumbiniCityTeacherSchedule.Utility.ModelBinders;
using LumbiniCityTeacherSchedule.Utility.SqlMapperHandler;

var builder = WebApplication.CreateBuilder(args);
//Register Custom sql Type Handlers
SqlMapper.AddTypeHandler(new TimeOnlyHandler());

//register custom ModelBinder
builder.Services.AddControllersWithViews(options=>
    {
        options.ModelBinderProviders.Insert(0, new DateOnlyModelBinderProvider());
        options.ModelBinderProviders.Insert(1, new TimeOnlyModelBinderProvider());
    });


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddScoped<IProgramData, ProgramData>();
builder.Services.AddScoped<ISemesterData, SemesterData>();
builder.Services.AddScoped<ISemesterConfigData, SemesterConfigData>();
builder.Services.AddScoped<ISemesterInstanceData, SemesterInstanceData>();
builder.Services.AddScoped<ISubjectData, SubjectData>();
//builder.Services.AddScoped<ITeacherData, TeacherData>();
//builder.Services.AddScoped<ITeacherAvailabilityData, TeacherAvailabilityData>();
builder.Services.AddScoped<IJoinedTeacherAndAvailabilityData, JoinedTeacherAndAvailabilityData>();
builder.Services.AddScoped<ITeacherAssignmentData, TeacherAssignmentData>();
builder.Services.AddScoped<ITimeSlotData, TimeSlotData>();
builder.Services.AddScoped<IClassScheduleData, ClassScheduleData>();
builder.Services.AddScoped<IProgramService, ProgramService>();
builder.Services.AddScoped<ISemesterService,SemesterService>();
builder.Services.AddScoped<ISemesterScheduleConfigService,SemesterScheduleConfigService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<ITeacherWithAvailabilityService,TeacherWithAvailabilityService>();
builder.Services.AddScoped<ITeacherAssignmentService, TeacherAssignmentService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
