using Azure;
using DistantEdu.Command;
using DistantEdu.Command.CommandHandlers.Subjects;
using Duende.IdentityServer.ResponseHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Concurrent;

namespace DistantEdu.Services
{
    public class CommandService
    {
        private readonly HttpContext _context;
        private static readonly ConcurrentDictionary<Type, Type> _commandHandlers = new(
            new List<KeyValuePair<Type, Type>>
            {
                new KeyValuePair<Type, Type>(typeof(GetSubjectQuery), typeof(GetSubjectsHandler))
            }
        );

        public CommandService(HttpContext context)
        {
            _context = context;
        }

        private object GetCommand(Type request)
        {
            var commandType = _commandHandlers.GetValueOrDefault(request);
            ArgumentNullException.ThrowIfNull(commandType, nameof(commandType));
            return Activator.CreateInstance(commandType, _context) 
                ?? throw new InvalidOperationException($"Can not create instance of {commandType}");
        }

        public async Task<TResponse?> Execute<TResponse>(IRequest<TResponse> request)
        {
            var commandObj = GetCommand(request.GetType());
            if (await ((Command<IRequest<TResponse>, TResponse>)commandObj).CanExecute(request))
                return await ((Command<IRequest<TResponse>, TResponse>)commandObj).Execute(request);
            return default;
        }

        public async Task Execute(IRequest request)
        {
            var commandObj = GetCommand(request.GetType());
            if (!await ((Command<IRequest>)commandObj).CanExecute(request)) return;
            await ((Command<IRequest>)commandObj).Execute(request);
        }
    }
}
