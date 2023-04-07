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

        // handler��ԭʼ��lambda handler������
        // HttpRequest�����Ѿ���HttpContext�������Զ���������ISomeService�������DI�����л�ȡ��
        // ��ISomeService�������Ǵ�DI�����л�ȡ�ġ�
        string text = (string)handler.DynamicInvoke(id, httpContext.Request, httpContext.RequestServices.GetRequiredService<ISomeService>())!;
        httpContext.Response.ContentType = "text/plain";
        return httpContext.Response.WriteAsync(text);
    }
}
