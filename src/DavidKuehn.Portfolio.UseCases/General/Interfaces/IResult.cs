using DavidKuehn.Portfolio.UseCases.General.Enums;

namespace DavidKuehn.Portfolio.UseCases.General.Interfaces
{
    public interface IResult<TValue>
    {
        ResultStatus Status { get; }
        TValue Value { get; }
        string? ErrorMessage { get; }
    }
}