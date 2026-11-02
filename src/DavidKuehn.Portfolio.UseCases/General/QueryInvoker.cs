using DavidKuehn.Portfolio.UseCases.General.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DavidKuehn.Portfolio.UseCases.General
{
    public class QueryInvoker : IQueryInvoker
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryInvoker(IServiceProvider serviceProvider)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider, nameof(serviceProvider));
            _serviceProvider = serviceProvider;
        }
        
        public async Task<IResult<TResultValue>> InvokeQueryAsync<TQuery, TResultValue>(TQuery query)
            where TQuery : IQuery
        {
            ArgumentNullException.ThrowIfNull(query, nameof(query));
            
            var queryHandler = _serviceProvider.GetService<IQueryHandler<TQuery, TResultValue>>();
            if (queryHandler == null)
            {
                throw new InvalidOperationException("Query handler is not found.");
            }

            return await queryHandler.HandleAsync(query);
        }
    }
}
