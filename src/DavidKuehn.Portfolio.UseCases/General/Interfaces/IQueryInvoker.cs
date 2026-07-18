using DavidKuehn.Portfolio.UseCases.General.Interfaces;

namespace DavidKuehn.Portfolio.UseCases.General
{
    public interface IQueryInvoker
    {
        /// <summary>
        /// Invokes a query to get data.
        /// </summary>
        /// <param name="handler">The query handler.</param>
        /// <param name="query">The query to be executed.</param>
        /// <typeparam name="TQuery">The type of the query.</typeparam>
        /// <typeparam name="TQueryHandler">The type of the query handler.</typeparam>
        /// <typeparam name="TResultValue">The type of the result value.</typeparam>
        /// <returns></returns>
        Task<IResult<TResultValue>> InvokeQueryAsync<TQuery, TQueryHandler, TResultValue>(TQueryHandler handler, TQuery query)
            where TQuery : IQuery
            where TQueryHandler : IQueryHandler<TQuery, TResultValue>;
    }
}
