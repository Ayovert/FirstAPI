using System;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace APIDemo.Services
{
    public class LocalMailService : IMailService
    {
       

        private readonly IConfiguration _configuration;

        public LocalMailService(IConfiguration configuration)
        {
            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
        }
        public void Send(string message, string subject)
        {

            Debug.WriteLine($"Mail was sent from {_configuration["mailSettings:mailFromAddress"]} to {_configuration["mailSettings:mailToAddress"]} from Local Mail Service");
            Debug.WriteLine($"{subject}");
            Debug.WriteLine($"{message}");

        }
    }
}
