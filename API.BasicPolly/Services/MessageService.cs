using API.BasicPolly.Repositories;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using System;
using System.Threading.Tasks;

namespace API.BasicPolly.Services
{
    public interface IMessageService
    {
        Task<string> GetHelloMessage();
        Task<string> GetGoodbyeMessage();
    }

    public class MessageService : IMessageService
    {
        private IMessageRepository _messageRepository;
        private AsyncRetryPolicy _retryPolicy;
        private AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;

            _circuitBreakerPolicy = Policy.Handle<Exception>()
                .CircuitBreakerAsync(1, TimeSpan.FromSeconds(10),
                (ex, t) =>
                {
                    Console.WriteLine($"Circuit broken! - {DateTime.Now}");
                },
                () =>
                {
                    Console.WriteLine($"Circuit Reset! - {DateTime.Now}");
                });
        }

        public async Task<string> GetHelloMessage()
        {
            try
            {
                Console.WriteLine($"Circuit State: {_circuitBreakerPolicy.CircuitState} - {DateTime.Now}");
                return await _circuitBreakerPolicy.ExecuteAsync<string>(async () =>
                {
                    return await _messageRepository.GetHelloMessage();
                });
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> GetGoodbyeMessage()
        {
            try
            {
                Console.WriteLine($"Circuit State: {_circuitBreakerPolicy.CircuitState} - {DateTime.Now}");
                return await _circuitBreakerPolicy.ExecuteAsync<string>(async () =>
                {
                    return await _messageRepository.GetGoodbyeMessage();
                });
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
