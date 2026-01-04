using auth_service.authservice.api.Middlewares;
using auth_service.authservice.application.handler;
using auth_service.authservice.application.InterfaceApplication;
using auth_service.authservice.domain.Interfaces;
using auth_service.authservice.infastructure.dbcontexts;
using auth_service.authservice.infastructure.Repository;
using auth_service.authservice.infastructure.Securities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace auth_service.authservice.start
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotNetEnv.Env.Load();
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddDbContext<FoodAuthContext>(option =>
                option.UseSqlServer(builder.Configuration["URLSQL"]));

            
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
            });

            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = "Google";
                option.DefaultSignInScheme = "ExternalCookie";
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
            })
      
            .AddCookie("ExternalCookie", options =>
            {
                options.Cookie.Name = "FoodAuth.External";
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.Path = "/";
            })
            .AddGoogle("Google", options =>
            {
                options.ClientId = builder.Configuration["ClientID"]!;
                options.ClientSecret = builder.Configuration["Clientsecret"]!;
                options.SignInScheme = "ExternalCookie";

                // Khớp với DownstreamPath trong ocelot.json
                options.CallbackPath = "/api/users/signin-google";

                options.ClaimActions.MapJsonKey("avatar", "picture", "url");

                options.Events = new OAuthEvents
                {
                    OnRedirectToAuthorizationEndpoint = context =>
                    {
                        // Đánh tráo Port nội bộ (7191) bằng Port Gateway (7150) gửi cho Google
                        string decodedUrl = WebUtility.UrlDecode(context.RedirectUri);
                        string interceptedUrl = decodedUrl
                            .Replace("localhost:7191/api/users", "localhost:7150/users")
                            .Replace("127.0.0.1:7191/api/users", "localhost:7150/users");

                        context.Response.Redirect(interceptedUrl);
                        return Task.CompletedTask;
                    },
                    OnRemoteFailure = context =>
                    {
                        // Khi lỗi (ví dụ Correlation failed), nhảy về Gateway để xử lý
                        context.Response.Redirect("https://localhost:7150/users/google-error?message=" +
                                                   WebUtility.UrlEncode(context.Failure?.Message));
                        context.HandleResponse();
                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

    
            builder.Services.AddTransient<ICreateUserHandler, CreateUserHandler>();
            builder.Services.AddTransient<IHashPassword, EncodePassword>();
            builder.Services.AddTransient<IUserRepositories, UserRepositories>();
            builder.Services.AddTransient<IUserLogin, UserSignInHandler>();
            builder.Services.AddTransient<IAuthenticationToken, AuthenticationJWT>();
            builder.Services.AddTransient<IRefreshTokensRepositories, RefreshTokensRepositories>();
            builder.Services.AddTransient<IRoleUser, RoleUser>();
            builder.Services.AddTransient<IProvideoAccessToken, ProvideoAccessToken>();
            builder.Services.AddTransient<IAuthenticationByGoogle, AuthenticationByGoogle>();

            var app = builder.Build();

           
            app.UseForwardedHeaders();
            app.UseCookiePolicy(); 

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}