using DavidKuehn.Portfolio.UseCases.General.Interfaces;

namespace DavidKuehn.Portfolio.UseCases.General
{
    /// <summary>
    /// Defines a contract for invoking queries to retrieve data.
    /// </summary>
    public interface IQueryInvoker
    {
        /// <summary>
        /// Invokes a query to get data.
        /// </summary>
        /// <param name="query">The query to be executed.</param>
        /// <typeparam name="TQuery">The type of the query. Cannot be null.</typeparam>
        /// <typeparam name="TResultValue">The type of the result value.</typeparam>
        /// <returns>The result of the query execution.</returns>
        Task<IResult<TResultValue>> InvokeQueryAsync<TQuery, TResultValue>(TQuery query)
            where TQuery : IQuery;
    }
}
