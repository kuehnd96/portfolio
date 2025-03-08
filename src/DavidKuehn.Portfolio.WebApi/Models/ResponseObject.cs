namespace DavidKuehn.Portfolio.WebApi.Models
{
    [Obsolete("Came with authentication instructions; Not sure I need this.")]
    public class ResponseObject<TEntity> where TEntity : class
    {
        public IEnumerable<TEntity>? Records { get; set; }
        public TEntity? Record { get; set; }
        public string? Message { get; set; }
        public int StatusCode { get; set; }
    }
}
