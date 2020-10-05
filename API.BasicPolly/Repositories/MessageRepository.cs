using API.BasicPolly.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.BasicPolly.Repositories
{
    public interface IMessageRepository
    {
        Task<string> GetHelloMessage();
        Task<string> GetGoodbyeMessage();
    }

    public class MessageRepository : IMessageRepository
    {
        private MessageOptions _messageOptions;

        public MessageRepository(IOptions<MessageOptions> messageOptions)
        {
            _messageOptions = messageOptions.Value;
        }

        public async Task<string> GetHelloMessage()
        {
            Console.WriteLine($"MessageRepository GetHelloMessage running - {DateTime.Now}");
            await ThrowRandomException();
            return _messageOptions.HelloMessage;
        }

        public async Task<string> GetGoodbyeMessage()
        {
            Console.WriteLine($"MessageRepository GetGoodbyeMessage running - {DateTime.Now}");
            await ThrowRandomException();
            return _messageOptions.GoodbyeMessage;
        }

        private async Task ThrowRandomException()
        {
            var diceRoll = new Random().Next(0, 10);

            if (diceRoll > 5)
            {
                Console.WriteLine($"ERROR! Throwing Exception - {DateTime.Now}");
                throw new Exception("Exception in MessageRepository");
            }
        }
    }
}
