using Core.Repository;
using Infrastructure.Repository;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;
using System.Threading;

namespace EmailService
{
    class EmailService
    {
        private readonly IInvestmentRepository _investmentRepository;
        private readonly ICustomerRepository _customerRepository;

        public EmailService(IInvestmentRepository investmentRepository, ICustomerRepository customerRepository)
        {
            _investmentRepository = investmentRepository;
            _customerRepository = customerRepository;
        }

        public async Task SendEmailAsync()
        {
            var ListaInvestimentos = _investmentRepository.ObterInvestimentosAtivos();
            var ListaOperadores = _customerRepository.ObterTodosOperadores();

            foreach (var operador in ListaOperadores)
            {
                var message = new MimeMessage();
                message.Subject = $"Atualização diária de vencimentos - {DateTime.Now.ToString("dd/MM/yyyy")} ";
                message.From.Add(new MailboxAddress("XP Investimentos", "kauazinhonaxp@hotmail.com"));
                message.To.Add(new MailboxAddress(operador.FirstName, operador.Email));

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = "Atualização diária do vencimento dos produtos/investimentos: \n\n";

                foreach (var investimento in ListaInvestimentos)
                {
                    bodyBuilder.TextBody += $"Produto: {investimento.Name} - Vencimento: {investimento.ExpiryDate.ToShortDateString()}\n";
                }

                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                try
                {
                    await client.ConnectAsync("smtp-mail.outlook.com", 587, MailKit.Security.SecureSocketOptions.StartTls); // Usando SMTP do Gmail
                    await client.AuthenticateAsync("kauazinhonaxp@hotmail.com", "KauaNaXP2024");
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                    Console.WriteLine($"E-mail enviado para {operador.FirstName} ({operador.Email}) no dia {DateTime.Now.ToString("dd/MM/yyyy")}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao enviar e-mail para {operador.FirstName} ({operador.Email}): {ex.Message}");
                }
            }
        }

        static async Task Main(string[] args)
        {
            var serviceProvider = ConfigureServices();
            var emailService = serviceProvider.GetService<EmailService>();

            while (true)
            {
                try
                {
                    await emailService.SendEmailAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao enviar e-mails: {ex.Message}");
                }

                Thread.Sleep(86400000); // Envia o e-mail novamente em 24h
            }
        }

        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Configuração do appsettings.json
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            services.AddSingleton(configuration);

            // Configuração do DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

            services.AddScoped<IInvestmentRepository, InvestmentRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();


            services.AddScoped<EmailService>();

            return services.BuildServiceProvider();
        }

    }
}
