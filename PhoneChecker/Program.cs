
using PhoneChecker.Abstractions;
using PhoneChecker.Repositories;
using PhoneChecker.Services;

namespace PhoneChecker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddSingleton<IPhoneNormalizer, PhoneNormalizer>();
            builder.Services.AddSingleton<IPhoneNumberChecker, PhoneNumberChecker>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Заблокированные номера телефонов
            using (var scope = app.Services.CreateScope())
            {
                var checker = scope.ServiceProvider.GetRequiredService<IPhoneNumberChecker>();
                checker.Insert("+79962911212");
                checker.Insert("+71231231231");
            }

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
