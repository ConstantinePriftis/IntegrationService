using IntegrationService.Data.DbContexts;
using IntegrationService.Models.User;
using IntegrationService.Configuration;
using Microsoft.OpenApi.Models;
using Microsoft.CodeAnalysis.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IntegrationService.Data;
using ApplicationDbContext = IntegrationService.Data.DbContexts.ApplicationDbContext;

var builder = WebApplication.CreateBuilder(args);
//var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection");builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));// Add services to the container.
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

await builder.Services.AddCustomConfigurations()
                .AddInfrastructure(builder.Configuration)
                .AddRepositories()
                .AddCustomControllerServices()
                .AddCqrs()
                .AddCustomPolicy()
                .AddQuartzAsync(builder.Configuration);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v2", new OpenApiInfo { Title = "MVCCallWebAPI", Version = "v2" });
    //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    //{
    //    In = ParameterLocation.Header,
    //    Description = "Please enter a valid token",
    //    Name = "Authorization",
    //    Type = SecuritySchemeType.Http,
    //    BearerFormat = "JWT",
    //    Scheme = "Bearer"
    //});
    //c.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //    {
    //        new OpenApiSecurityScheme
    //        {
    //            Reference = new OpenApiReference
    //            {
    //                Type=ReferenceType.SecurityScheme,
    //                Id="Bearer"
    //            }
    //        },
    //        new string[]{}
    //    }
    //});
});

var app = builder.Build();
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
// specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "MVCCallWebAPI");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();


