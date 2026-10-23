using DavidKuehn.Portfolio.UseCases.General.Interfaces;

namespace DavidKuehn.Portfolio.UseCases.General
{
    public interface IQueryHandler<TQuery, TResultValue> where TQuery : IQuery
    {
        Task<IResult<TResultValue>> HandleAsync(TQuery query);
    }
}