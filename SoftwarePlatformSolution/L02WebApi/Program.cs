
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace L02WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // This enable the OpenAPI explorer page (Swagger)
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // This singleton service simulates a database server
        // In a real-case scenario, we would use Microsoft.EntityFrameworkCore
        // connected to a db such as sqlite, postgre, sql server, etc.
        builder.Services.AddSingleton<InMemoryStorage>();

        // This statement finalizes the setup of the dependency injection
        // Depenency Injection allows to "receive" a service in the functions below
        // For example "InMemoryStorage" has just been added as service and can be
        // requested by the GET/POST handlers below
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            // Swagger (OpenAPI) and its UI are enabled only for development purposes
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // This is commented to allow HTTP requests
        // In a real scenario, HTTPS should be turned on, which requires a certificate
        // For development purposes, it is possible to create a self-signed certificate
        // In real scenarios, we can leverage the certificates created by the "Let's encrypt" BOT
        //
        //app.UseHttpsRedirection();

        // This is the endpoint for getting all the Todos from the repository
        app.MapGet("/todos",
                (InMemoryStorage storage) =>
                {
                    app.Logger.LogInformation($"/todos is producing {storage.All.Count()} items");
                    return storage.All;
                })
            .WithName("Todos")
            .WithOpenApi();


        // This is the endpoint for getting the Todos matching a portion of the title string
        app.MapGet("/todos/{title}", async (InMemoryStorage storage, string title) =>
            {
                var result = await storage.GetByTitle(title);
                app.Logger.LogInformation($"/todos/{title} is producing {result.Count()} items");
                return result;
            })
            .WithName("GetTodo")
            .WithOpenApi();


        // This is the endpoint for adding a Todo to the repository
        app.MapPost("/todos", (InMemoryStorage storage, Todo todo) =>
            {
                app.Logger.LogInformation($"POST /todos is producing 1 item");
                return storage.Add(todo);
            })
            .WithName("PostTodo")
            .WithOpenApi();

        app.Run();
    }


}