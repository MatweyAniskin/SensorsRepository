
using AutoMapper;
using Microsoft.AspNetCore.Http.Features;
using TestTaskApi.DataBase;
using TestTaskApi.Models.Repository.Variations.SensorRepository;
using TestTaskApi.Models.Repository.Variations.SensorValueRepository;
using TestTaskApi.Parser.Variations;

namespace TestTaskApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //expanded the size limit of received files
            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 204857600;
            });
            builder.Services.AddControllers();
           
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.AddSingleton<IMongoDb, MongoDbService>();
            //work with data through repositories
            builder.Services.AddScoped<ISensorRepository, SensorRepository>();
            builder.Services.AddScoped<ISensorValueRepository, SensorValueRepository>();

            builder.Services.AddTransient<ISensorParserFromCsvService, SensorParserFromCsvService>();


            //cors in mode *
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });


            var app = builder.Build();

            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
