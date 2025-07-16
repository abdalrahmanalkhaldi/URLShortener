
using Microsoft.EntityFrameworkCore;
using URLShortenerApiApplication.Data;
using URLShortenerApiApplication.Services;
using URLShortenerApiApplication.Services.TokenService;
using URLShortenerApiApplication.Services.RegisterService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using URLShortenerApiApplication.Models;
using URLShortenerApiApplication.Services.URLShortener;

namespace URLShortenerApiApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IRegisterService, RegisterService>();
            builder.Services.AddScoped<ILoginService,LoginService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IURLShortenerService, URLShortenerService>();

            /// // Configure JWT authentication
            /// 
            var TokenRes = builder.Configuration.GetSection("Jwt").Get<JwtToken>();
            builder.Services.AddSingleton(TokenRes);
            builder.Services.AddAuthentication();
            builder.Services.AddAuthentication().AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(TokenRes.SigningKey))
                };
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
