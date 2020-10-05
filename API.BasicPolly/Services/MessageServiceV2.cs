using API.BasicPolly.Repositories;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using System;
using System.Threading.Tasks;

namespace API.BasicPolly.Services
{
    public interface IMessageServiceV2
    {
        Task<string> GetHelloMessage();
        Task<string> GetGoodbyeMessage();
    }

    public class MessageServiceV2 : IMessageServiceV2
    {
        private IMessageRepository _messageRepository;
        private AsyncRetryPolicy _retryPolicy;
        private AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

        public MessageServiceV2(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;

            _circuitBreakerPolicy = Policy.Handle<Exception>().AdvancedCircuitBreakerAsync(0.95, TimeSpan.FromSeconds(10), 3, TimeSpan.FromSeconds(10), OnBreak, Reset, OnHalfOpen);
        }

        private void OnBreak(Exception e,TimeSpan t)
        {
            Console.WriteLine($"Circuit broken! - {DateTime.Now}");
        }

        private void Reset()
        {
            Console.WriteLine($"Circuit Reset! - {DateTime.Now}");
        }

        private void OnHalfOpen()
        {
            Console.WriteLine($"Circuit OnHalfOpen! - {DateTime.Now}");
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
