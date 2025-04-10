using LumbiniCityTeacherSchedule.DataAccess.Data.ProgramData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterConfigData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SemesterInstanceData;
using LumbiniCityTeacherSchedule.DataAccess.Data.SubjectData;
using LumbiniCityTeacherSchedule.DataAccess.DbAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddScoped<IProgramData, ProgramData>();
builder.Services.AddScoped<ISemesterData, SemesterData>();
builder.Services.AddScoped<ISemesterConfigData, SemesterConfigData>();
builder.Services.AddScoped<ISemesterInstanceData, SemesterInstanceData>();
builder.Services.AddScoped<ISubjectData,SubjectData>();


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

