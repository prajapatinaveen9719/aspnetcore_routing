using System.Collections.Specialized;
using System.Net;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseRouting();

Dictionary<int, string> listdict = new Dictionary<int, string>()
        {
            { 1,"India"},
            { 2,"Canada"},
            { 3,"Russia"}
        };

app.MapGet("countries", async context =>
   {
       foreach (var item in listdict)
       {
           await context.Response.WriteAsync(item.Key + item.Value +"\n");
       }
     

   });

app.MapGet("/countries/{id:int:range(1,100)}", async (HttpContext context) =>
{
    if (!context.Request.RouteValues.ContainsKey("id"))
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("invalid countryid");

    }
     Int32 routevalues =Convert.ToInt32( context.Request.RouteValues["id"]);
    if(listdict.ContainsKey(routevalues))
    {
        await context.Response.WriteAsync(listdict[routevalues]);

    }
    else
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsync($"[No country]");
    }

});

//When request path is "countries/{countryID}"
app.MapGet("/countries/{id:max(101)}", async context =>
{
    context.Response.StatusCode = 400;
    await context.Response.WriteAsync("The CountryID should be between 1 and 100 - min");
});


app.MapGet("/", () => "Hello World!");


app.Run();
