namespace FitnessPartner.Middleware
{
	public class GlobalExcpetionMiddleware : IMiddleware
	{
		private readonly ILogger<GlobalExcpetionMiddleware> _logger;

		public GlobalExcpetionMiddleware(ILogger<GlobalExcpetionMiddleware> logger)
		{
			_logger = logger;
		}

		public async Task InvokeAsync(
			HttpContext context, RequestDelegate next)
		{
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Noe gikk galt - test exception {@Machine} {@TraceId}",
				Environment.MachineName,
				System.Diagnostics.Activity.Current?.Id);

				var statusCode = StatusCodes.Status500InternalServerError;
				if (ex.GetType() == typeof(UnauthorizedAccessException)) 
				{
					statusCode = StatusCodes.Status401Unauthorized;
				}

				await Results.Problem(
					title: ex.Message,
					statusCode: statusCode,
					extensions: new Dictionary<string, object?>
					{
					{
						"traceId", System.Diagnostics.Activity.Current?.Id},
					}).ExecuteAsync(context);
			}
		}

	}
}
