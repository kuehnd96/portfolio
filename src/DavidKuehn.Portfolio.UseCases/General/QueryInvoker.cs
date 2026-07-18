using DavidKuehn.Portfolio.UseCases.General.Interfaces;

namespace DavidKuehn.Portfolio.UseCases.General
{
    public class QueryInvoker : IQueryInvoker
    {
        public async Task<IResult<TResultValue>> InvokeQueryAsync<TQuery, TQueryHandler, TResultValue>(TQueryHandler queryHandler, TQuery query)
            where TQuery : IQuery
            where TQueryHandler : IQueryHandler<TQuery, TResultValue>
        {
            if (queryHandler != null)
            {
                return await queryHandler.HandleAsync(query);
            }
            else
            {
                throw new InvalidOperationException("Query handler is not initialized.");
            }
        }
    }
}
