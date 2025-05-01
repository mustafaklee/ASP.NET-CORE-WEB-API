using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyFirstApiProject.Mappings;
using MyFirstApiProject.Repositories;
using MyFirstApiProjects.Data;
using System.Text;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using MyFirstApiProject.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//dependency injection // bağımlılık enjeksiyonu
builder.Services.AddDbContext<NZWalksDbContext>(options => 
    options.UseMySql(
        builder.Configuration.GetConnectionString("NZWalksDbContext"),
        new MySqlServerVersion(new Version(8, 0, 0))
));

builder.Services.AddDbContext<NZWalksAuthDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("NZWalksAuthDbContext"),
        new MySqlServerVersion(new Version(8, 0, 0))
));


//repository pattern'i kullanmak icin inject etmemiz gerekiyor. 
//bu satır ile IRegionRepository interface'ni ve SqlRegionRepositories class'ını enjekte eder.
builder.Services.AddScoped<IRegionRepository, SqlRegionRepositories>();
builder.Services.AddScoped<IWalkRepository, SqlWalkRepositories>();
//bu satır farklı bir veritabanına geildiği seneryoda 2.repo olarak değişim kolaylığını gstermek için oluşturulmuştur.
//builder.Services.AddScoped<IRegionRepository, InMemoryRegionRepository>();

builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IImageRepository, LocalImageRepository>();

//AutoMapper i programa enjekte etmemiz gerekiyor.
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));


//guvenlik ayarları
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<NZWalksAuthDbContext>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),"Images")),
    RequestPath = "/Images"
});
app.MapControllers();

app.Run();
