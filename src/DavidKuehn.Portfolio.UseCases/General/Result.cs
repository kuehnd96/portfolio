using DavidKuehn.Portfolio.UseCases.General.Enums;
using DavidKuehn.Portfolio.UseCases.General.Interfaces;

namespace DavidKuehn.Portfolio.UseCases.General
{
    public class Result<TValue> : IResult<TValue>
    {
        public ResultStatus Status { get; set; }
        public TValue Value { get; set; }
        public string? ErrorMessage { get; set; }

        public Result(ResultStatus status, string? errorMessage)
        {
            Status = status;
            Value = default!;
            ErrorMessage = errorMessage;
        }

        public Result(TValue value)
        {
            Status = ResultStatus.Ok;
            Value = value;
        }
    }
}