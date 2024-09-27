using API.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddDbContext<MGFContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("APIContext"))
);

//Deactivate Session, Session will be called in MVC 
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddCors(opt => opt.AddPolicy("EnableCors", opt =>
{
    //enable to add cors;
    var corsAllowedOrigins = builder.Configuration.GetSection("CORSAllowedOrigin").Get<string[]>();
    //opt.WithOrigins(corsAllowedOrigins);
    opt.AllowAnyOrigin();
    opt.AllowAnyHeader();
    opt.AllowAnyMethod();
    opt.AllowCredentials();
}));


// JWT
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option => {
    option.RequireHttpsMetadata = false;
    option.SaveToken = true;
    option.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseRouting();

//Deactivate Session, Session will be called in MVC 
app.UseSession();

//app.Use(async (context, next) =>
//{
//    var JWToken = context.Session.GetString("JWToken");
//    if (!string.IsNullOrEmpty(JWToken))
//    {
//        context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
//    }
//    await next();
//});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
