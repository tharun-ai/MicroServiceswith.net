using Azure.Messaging.ServiceBus;
using Mango.Services.EmailAPI.Models.Dto;
using Mango.Services.EmailAPI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string _serviceBusConnectionString;
        private readonly string _emailCartQueue;
        private readonly string _registerUserQueue;
        private readonly IConfiguration _configuration;
        private ServiceBusProcessor _emailCartProcessor;
        private ServiceBusProcessor _registerUserProcessor;
        private readonly EmailService _emailService;

        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));

            _serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            _emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");
            _registerUserQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailRegisterQueue");

            var client = new ServiceBusClient(_serviceBusConnectionString);
            _emailCartProcessor = client.CreateProcessor(_emailCartQueue);
 
            //_registerUserProcessor = client.CreateProcessor(_registerUserQueue);
        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync();

            // Uncomment the following lines if needed
            // _registerUserProcessor.ProcessMessageAsync += OnRegisterUserRequestReceived;
            // _registerUserProcessor.ProcessErrorAsync += ErrorHandler;
            // await _registerUserProcessor.StartProcessingAsync();
        }

        private async Task OnRegisterUserRequestReceived(ProcessMessageEventArgs arg)
        {
            try
            {
                var message = arg.Message;
                var body = Encoding.UTF8.GetString(message.Body);
                string email = JsonConvert.DeserializeObject<string>(body);

                await _emailService.RegisterUserEmailAndLog(email);
                await arg.CompleteMessageAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing register user request: {ex.Message}");
                // Log or handle the error as needed
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs arg)
        {
            Console.WriteLine($"An error occurred: {arg.Exception.ToString()}");
            // Log or handle the error as needed
            return Task.CompletedTask;
        }

        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs arg)
        {
            try
            {
                var message = arg.Message;
                var body = Encoding.UTF8.GetString(message.Body);
                CartDto objMessage = JsonConvert.DeserializeObject<CartDto>(body);

               await _emailService.EmailCartAndLog(objMessage);
                await arg.CompleteMessageAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing email cart request: {ex.Message}");
                // Log or handle the error as needed
            }
        }

        public async Task Stop()
        {
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();
        }
    }
}
