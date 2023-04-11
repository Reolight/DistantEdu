namespace DistantEdu.Command
{
    public abstract class BaseCommand
    {
        /// <summary>
        /// Used for resolving dependencies
        /// </summary>
        protected readonly HttpContext _context;
        public BaseCommand(HttpContext httpContext)
            => _context = httpContext;
    }

    public abstract class Command<TRequest> : BaseCommand
        where TRequest : IRequest
    {
        public Command(HttpContext httpContext) : base(httpContext) { }
        public abstract Task<object?> Execute(in TRequest request);
        public abstract Task<bool> CanExecute(in TRequest request);
    }

    public abstract class Command<TRequest, TResponse> : BaseCommand
        where TRequest : IRequest<TResponse>
    {
        public Command(HttpContext httpContext) : base(httpContext) { }
        public abstract Task<TResponse> Execute(TRequest request);
        public abstract Task<bool> CanExecute(TRequest request);
    }
}
