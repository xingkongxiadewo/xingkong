using Auth.DTO;
using Auth.Service;
using Auth.WebApi.Common;
using Auth.WebApi.Model;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

/// <summary>
/// 跨域名称
/// </summary>
var corsPolicyName = "AuthCorsPolicy";

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

//builder.Services.AddAuthentication(options => options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();


builder.Services.AddControllers();

#region Jwt配置
//将appsettings.json中的JwtSettings部分文件读取到JwtSettings中，这是给其他地方用的
builder.Services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

//由于初始化的时候我们就需要用，所以使用Bind的方式读取配置
//将配置绑定到JwtSettings实例中
var jwtSettings = new JwtSettings();
configuration.Bind("JwtSettings", jwtSettings);

//添加身份验证
builder.Services.AddAuthentication(options =>
{
    //认证middleware配置
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(o =>
{
    //jwt token参数设置
    o.TokenValidationParameters = new TokenValidationParameters
    {
        //Token颁发机构
        ValidIssuer = jwtSettings.Issuer,
        //颁发给谁
        ValidAudience = jwtSettings.Audience,
        //这里的key要进行加密
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

        /***********************************TokenValidationParameters的参数默认值***********************************/
        // RequireSignedTokens = true,
        // SaveSigninToken = false,
        // ValidateActor = false,
        // 将下面两个参数设置为false，可以不验证Issuer和Audience，但是不建议这样做。
        // ValidateAudience = true,
        // ValidateIssuer = true, 
        // ValidateIssuerSigningKey = false,
        // 是否要求Token的Claims中必须包含Expires
        // RequireExpirationTime = true,
        // 允许的服务器时间偏移量
        // ClockSkew = TimeSpan.FromSeconds(300),
        // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
        // ValidateLifetime = true
    };
});

#endregion

#region AutoMapper配置
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
#endregion

#region Swagger
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

#region DbContext
builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Auth")));
#endregion

#region Autofac
// autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    Assembly assembly = Assembly.Load("Auth.Service");
    builder.RegisterAssemblyTypes(assembly)
    .AsImplementedInterfaces()
    .InstancePerDependency();
});
#endregion

#region 跨域设置
// 跨域设置
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, builder =>
    {
        builder.SetPreflightMaxAge(TimeSpan.FromSeconds(1800L));
        builder.WithOrigins("*")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsPolicyName);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCustomExceptionMiddleware();

app.Run();
