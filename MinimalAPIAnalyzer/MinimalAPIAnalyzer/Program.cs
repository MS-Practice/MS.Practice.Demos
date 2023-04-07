using MinimalAPIAnalyzer;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/{id}", (string id, HttpRequest request, ISomeService service) => { }).AddEndpointFilter;

app.Run();

RequestDelegate handler = (context) => { return Task.CompletedTask; };

void MockMapGet()
{
    Task Invoke(HttpContext httpContext)
    {
        bool wasParamCheckFailure = false;
        string id = httpContext.Request.Query["id"];
        if (id == null)
        {
            wasParamCheckFailure |= true;
            //Log.RequiredParameterNotProvided(httpContext, "string", "id", "route");
        }
        if (wasParamCheckFailure)
        {
            httpContext.Response.StatusCode = 400;
            return Task.CompletedTask;
        }

        // handler是原始的lambda handler方法。
        // HttpRequest参数已经从HttpContext参数中自动创建，而ISomeService参数则从DI容器中获取。
        // 而ISomeService参数则是从DI容器中获取的。
        string text = (string)handler.DynamicInvoke(id, httpContext.Request, httpContext.RequestServices.GetRequiredService<ISomeService>())!;
        httpContext.Response.ContentType = "text/plain";
        return httpContext.Response.WriteAsync(text);
    }
}
