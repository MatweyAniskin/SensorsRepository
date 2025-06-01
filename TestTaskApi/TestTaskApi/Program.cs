
using AutoMapper;
using Microsoft.AspNetCore.Http.Features;
using TestTaskApi.DataBase;
using TestTaskApi.Models.Mapping;
using TestTaskApi.Models.Repository.Variations.SensorRepository;
using TestTaskApi.Parser.Service;
using TestTaskApi.Parser.Variation;

namespace TestTaskApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //�������� ����� �������� ���������� ������
            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 204857600;
            });
            builder.Services.AddControllers();
           
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.AddSingleton<IMongoDb, MongoDbService>();
            //������� � ������� � ����� �����������
            builder.Services.AddTransient<ISensorRepository, SensorRepository>();
            
            builder.Services.AddTransient<IParserService, OneEntityParserService>();
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            //���� � ������ *
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
