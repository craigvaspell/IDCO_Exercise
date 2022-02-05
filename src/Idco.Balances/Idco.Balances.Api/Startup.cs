using AutoMapper;
using Idco.Balances.Api.AccountBalanceReport;
using Idco.Balances.Domain.Accounts;
using Idco.Balances.Domain.BalanceReports;
using Idco.Balances.Domain.Requests;
using Idco.Balances.Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using System.IO;

namespace Idco.Balances.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public bool SwaggerEnabled { get; }
        public string ApiVersion { get; }
        public string ApiTitle => $"IDCO Get Accounts Balance Report API {ApiVersion}";

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

            var versionInfo = GetType().Assembly.GetName().Version;
            SwaggerEnabled = Configuration.GetValue<bool>("SwaggerEnabled");
            ApiVersion = $"v{versionInfo.Major}.{versionInfo.Minor}";
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            ConfigureDomainRegistrations(services);
            ConfigureAutoMapper(services);

            if (SwaggerEnabled)
            {
                services.AddSwaggerGen(setup =>
                {
                    setup.SwaggerDoc(ApiVersion, new Microsoft.OpenApi.Models.OpenApiInfo() { Title = ApiTitle, Version = ApiVersion });
                    setup.IncludeXmlComments(GetXmlCommentsPath(Environment));
                });
                services.AddSwaggerGenNewtonsoftSupport();
            }
        }

        public static void ConfigureDomainRegistrations(IServiceCollection services)
        {
            services.AddScoped<IAccountsBalanceReportService, AccountsBalanceReportService>();
            services.AddScoped<IAccountBalanceReportService, AccountBalanceReportService>();
        }

        public static void ConfigureAutoMapper(IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg => ConfigureAutoMapperMaps(cfg));
            var mapper = config.CreateMapper();

            services.AddSingleton(mapper);
        }

        public static void ConfigureAutoMapperMaps(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<AccountsEodBalanceRequestDto, AccountsBalanceRequest>();
            cfg.CreateMap<AccountDto, Account>();
            cfg.CreateMap<IdentifiersDto, Identifiers>();
            cfg.CreateMap<PartyDto, Party>();
            cfg.CreateMap<StandingOrderDto, StandingOrder>();
            cfg.CreateMap<DirectDebitDto, DirectDebit>();
            cfg.CreateMap<BalancesDto, Domain.Accounts.Balances>();
            cfg.CreateMap<BalanceDto, Balance>();
            cfg.CreateMap<CreditLineDto, CreditLine>();
            cfg.CreateMap<TransactionDto, Transaction>();
            cfg.CreateMap<MerchantDetailsDto, MerchantDetails>();


            cfg.CreateMap<EodBalanceListReport, EodBalanceListReportDto>()
                .ForMember(dest => dest.EndOfDayBalances, opt => opt.MapFrom(src => src.Balances));
            cfg.CreateMap<EodBalanceReport, EodBalanceReportDto>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage()
                   .UseHttpsRedirection();
            }

            app.UseRouting()
               .UseAuthorization()
               .UseEndpoints(endpoints =>
               {
                   endpoints.MapControllers();
               });

            if (SwaggerEnabled)
            {
                app.UseSwagger()
                   .UseStaticFiles()
                   .UseSwaggerUI(setup =>
                   {
                       setup.SwaggerEndpoint($"/swagger/{ApiVersion}/swagger.json", ApiTitle);
                   });
            }
        }


        private string GetXmlCommentsPath(IWebHostEnvironment environment)
            => Path.Combine(environment.WebRootPath, "Xml", "Idco.Balances.Api.xml");
    }
}
