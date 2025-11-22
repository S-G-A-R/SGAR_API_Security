using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SGAR_Seguridad.Properties.EndPoints;
using SGAR_Seguridad.Properties.Models;
using SGAR_Seguridad.Properties.Services.Ciudadanos;
using SGAR_Seguridad.Properties.Services.Operadores;
using SGAR_Seguridad.Properties.Services.Administradores;
using SGAR_Seguridad.Properties.Services.Organizations;
using SGAR_Seguridad.Properties.Services.Users;
using System.Reflection;
using System.Text;
using SGAR_Seguridad.Properties.Services.Solicitudes;
using SGAR_Seguridad.Properties.Services.Puntuaciones;
using SGAR_Seguridad.Properties.Services.Rols;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option => {

    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT en el siguiente formato: Bearer {token}"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

});

builder.Services.AddDbContext<SgarSecurityDbContext>(
    o => o.UseSqlServer(builder.Configuration.GetConnectionString("SGARConnection"))
);

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IOrganizacionServices, OrganizacionServices>();
builder.Services.AddScoped<ICiudadanoServices, CiudadanoServices>();
builder.Services.AddScoped<IOperadorServices, OperadorServices>();
builder.Services.AddScoped<IAdministradorServices, AdministradorServices>();
builder.Services.AddScoped<ISolicitudServices, SolicitudServices>();
builder.Services.AddScoped<IPuntuacionServices, PuntuacionServices>();
builder.Services.AddScoped<IRoleService, RoleService>();

var JwtSetting = builder.Configuration.GetSection("JwtSetting");
var secretKey = JwtSetting.GetValue<string>("SecretKey");

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(
    option =>
    {
        option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }

).AddJwtBearer(
    option =>
    {
        option.RequireHttpsMetadata = false;
        option.SaveToken = true;
        option.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = JwtSetting.GetValue<string>("Issuer"),
            ValidAudience = JwtSetting.GetValue<string>("Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };

    });

var app = builder.Build();

app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseEndPoints();

app.MapControllers();

app.Run();
