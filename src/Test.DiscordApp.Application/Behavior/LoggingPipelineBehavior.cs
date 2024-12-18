using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Model.Base;
using SharedKernel.Utility.Extensions;

namespace Test.DiscordApp.Application.Behavior;

public class LoggingPipelineBehavior<TRequest, TResponse>(
    ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger
): IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse: Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            new Dictionary<string, object> { ["Request"] = request },
            "Processing request {RequestName}", 
            request.GetType().Name
        );
        var requestName = request.GetType().Name;
        var timer = Stopwatch.StartNew();
        try
        {
            ArgumentNullException.ThrowIfNull(next);

            var response = await next().ConfigureAwait(true);
            timer.Stop();

            if (response.IsSuccess)
            {
                logger.LogInformation(
                    new Dictionary<string, object>
                    {
                        ["Response"] = response,
                        ["Elapsed"] = timer.Elapsed
                    },
                    "Request {RequestName} completed successfully in {ElapsedMilliseconds}ms",
                    requestName,
                    timer.ElapsedMilliseconds
                );
            }
            else
            {
                logger.LogWarning(
                    new Dictionary<string, object>
                    {
                        ["Error"] = response.Error,
                        ["Elapsed"] = timer.Elapsed
                    },
                    "Request {RequestName} failed in {ElapsedMilliseconds}ms",
                    requestName,
                    timer.ElapsedMilliseconds
                );
            }

            return response;
        }
        catch (ArgumentNullException e)
        {
            logger.LogError(
                new Dictionary<string, object> { ["Exception"] = e.Message },
                "Error processing request {RequestName}", requestName
            );
            return (TResponse)Result.Failure(Error.Problem(description: e.Message));
        }
        catch (Exception e)
        {
            logger.LogCritical(e, "Error processing request {RequestName}", requestName);
            return (TResponse)Result.Failure(Error.Problem(description: e.Message));
        }
    }
}