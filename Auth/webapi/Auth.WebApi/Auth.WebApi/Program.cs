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
/// ��������
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

#region Jwt����
//��appsettings.json�е�JwtSettings�����ļ���ȡ��JwtSettings�У����Ǹ������ط��õ�
builder.Services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

//���ڳ�ʼ����ʱ�����Ǿ���Ҫ�ã�����ʹ��Bind�ķ�ʽ��ȡ����
//�����ð󶨵�JwtSettingsʵ����
var jwtSettings = new JwtSettings();
configuration.Bind("JwtSettings", jwtSettings);

//��������֤
builder.Services.AddAuthentication(options =>
{
    //��֤middleware����
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(o =>
{
    //jwt token��������
    o.TokenValidationParameters = new TokenValidationParameters
    {
        //Token�䷢����
        ValidIssuer = jwtSettings.Issuer,
        //�䷢��˭
        ValidAudience = jwtSettings.Audience,
        //�����keyҪ���м���
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

        /***********************************TokenValidationParameters�Ĳ���Ĭ��ֵ***********************************/
        // RequireSignedTokens = true,
        // SaveSigninToken = false,
        // ValidateActor = false,
        // ������������������Ϊfalse�����Բ���֤Issuer��Audience�����ǲ�������������
        // ValidateAudience = true,
        // ValidateIssuer = true, 
        // ValidateIssuerSigningKey = false,
        // �Ƿ�Ҫ��Token��Claims�б������Expires
        // RequireExpirationTime = true,
        // ����ķ�����ʱ��ƫ����
        // ClockSkew = TimeSpan.FromSeconds(300),
        // �Ƿ���֤Token��Ч�ڣ�ʹ�õ�ǰʱ����Token��Claims�е�NotBefore��Expires�Ա�
        // ValidateLifetime = true
    };
});

#endregion

#region AutoMapper����
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

#region ��������
// ��������
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
