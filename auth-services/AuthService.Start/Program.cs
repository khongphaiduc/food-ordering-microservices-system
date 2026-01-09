using auth_services.AuthService.Application.Interfaces;
using auth_services.AuthService.Application.Service;
using auth_services.AuthService.Domain.Interface;
using auth_services.AuthService.Infastructure.DbContextAuth;
using auth_services.AuthService.Infastructure.Reposistory;
using auth_services.AuthService.Infastructure.Security;
using auth_services.AuthService.Infastructure.ServiceImpelemt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace auth_services.AuthService.Start
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotNetEnv.Env.Load();
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<FoodAuthContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["URLSQLFOOD_ORDRING_AUTH_SERVICE"]);
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!))
                };
            }); ;


            builder.Services.AddScoped<IGenarateSalt, GenarateSalt>();
            builder.Services.AddScoped<IHashPassword, HashPassword>();
            builder.Services.AddScoped<ISignUpUser, SignUpUser>();
            builder.Services.AddScoped<ICheckLogin, CheckLogin>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();





            builder.Services.AddControllers();

            var app = builder.Build();


            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
