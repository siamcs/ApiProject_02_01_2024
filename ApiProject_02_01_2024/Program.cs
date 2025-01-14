

//using ApiProject_02_01_2024.Data;
//using ApiProject_02_01_2024.Repository;
//using ApiProject_02_01_2024.Services.BankService;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;


//var builder = WebApplication.CreateBuilder(args);

//// Register the DbContext and the IBankService with DI container
//builder.Services.AddDbContext<AppDBContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
//builder.Services.AddScoped<IBankService, BankService>(); // Add this line to inject the service

//// Add services to the container
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
////builder.Services.AddCors();
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAngularOrigin", policy =>
//        policy.AllowAnyOrigin()// Your Angular app URL
//              .AllowAnyHeader()
//              .AllowAnyMethod());
//});

////
//var jwtSettings = builder.Configuration.GetSection("JwtSettings");
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = jwtSettings["Issuer"],
//            ValidAudience = jwtSettings["Audience"],
//            IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
//        };
//    });
////
//builder.Services.AddAuthorization();
//var app = builder.Build();

//// Configure the HTTP request pipeline
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//    app.UseDeveloperExceptionPage();
//}

//app.UseHttpsRedirection();
////app.UseCors();
//app.UseCors("AllowAngularOrigin");
//app.UseStaticFiles();
//app.UseAuthentication();
//app.UseAuthorization();
//app.MapControllers();

//app.Run();


using ApiProject_02_01_2024.Data;
using ApiProject_02_01_2024.Repository;
using ApiProject_02_01_2024.Services.BankService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Register the DbContext and services
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add CORS with specific configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", policy =>
        policy.WithOrigins("http://localhost:4200") // Allow your frontend origin
              .AllowAnyHeader()
              .AllowAnyMethod());
});


// Add services for your repositories and services
builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
builder.Services.AddScoped<IBankService, BankService>(); // Injecting the BankService




// Add authentication (JWT configuration example)
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger in Development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseCors("AllowAnyOrigin");
// Apply CORS middleware before Authentication


app.UseAuthentication();  // Enable authentication
app.UseAuthorization();   // Enable authorization

app.MapControllers();  // Map your controllers

app.Run();
