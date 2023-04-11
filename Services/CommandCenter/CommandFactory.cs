using DistantEdu.Command;

namespace DistantEdu.Services
{
    public static class CommandFactory
    {
        public static bool Send<TResponse>(in IRequest<TResponse> request, HttpContext httpContext, out TResponse? response)
        {
            CommandService service = new CommandService(httpContext);
            try
            {
                response = service.Execute(request).GetAwaiter().GetResult() ?? default;
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.ToString());
                response = default;
                return false;
            }
            
            return true;
        }

        public static bool Send(in IRequest request, HttpContext httpContext) 
        {
            CommandService service = new CommandService(httpContext);
            try
            {
                service.Execute(request).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

            return true;
        }

        // Extenstion methods to be invoked on HttpContext (kinda handy)
        public static bool Send(this HttpContext httpContext, in IRequest request)
            => Send(request, httpContext);

        public static bool Send<TResponse>(this HttpContext httpContext, in IRequest<TResponse> request, out TResponse? response)
            => Send(httpContext, request, out response);
    }
}
