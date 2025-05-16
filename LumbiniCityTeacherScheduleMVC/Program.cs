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
using LumbiniCityTeacherScheduleMVC.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using LumbiniCityTeacherSchedule.Utility.StaticData;
using LumbiniCityTeacherSchedule.Service.SemesterInstanceService;
using LumbiniCityTeacherSchedule.Service.ClassScheduleService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);
//Register Custom sql Type Handlers
SqlMapper.AddTypeHandler(new TimeOnlyHandler());
SqlMapper.AddTypeHandler(new DateOnlyHandler());

//register custom ModelBinder
builder.Services.AddControllersWithViews(options=>
    {
        options.ModelBinderProviders.Insert(0, new DateOnlyModelBinderProvider());
        options.ModelBinderProviders.Insert(1, new TimeOnlyModelBinderProvider());
        var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
        options.Filters.Add(new AuthorizeFilter(policy));
    });


// Add services to the container.

builder.Services.AddDbContext<ApplicationIdentityDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<ApplicationIdentityDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole(SD.Role_Admin));
    options.AddPolicy("Department", policy => policy.RequireRole(SD.Role_Department));
    // Add more policies as needed
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";         // redirects to login
    options.AccessDeniedPath = "/Identity/Account/AccessDenied"; // for unauthorized access
});
builder.Services.AddRazorPages();
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
builder.Services.AddScoped<ISemesterInstanceService, SemesterInstanceService>();
builder.Services.AddScoped<IClassScheduleService,ClassScheduleService>();
builder.Services.AddScoped<IEmailSender,EmailSender>();
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
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Department}/{controller=Home}/{action=Index}/{id?}");

app.Run();
