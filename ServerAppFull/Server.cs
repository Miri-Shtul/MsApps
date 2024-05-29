using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ServerApp.Data;
using ServerApp.Models;
using Newtonsoft.Json;

public class Server
{
    private readonly HttpListener _listener;
    private readonly IServiceProvider _serviceProvider;

    public Server(string[] prefixes, IServiceProvider serviceProvider)
    {
        if (prefixes == null || prefixes.Length == 0)
            throw new ArgumentException("prefixes");

        _listener = new HttpListener();
        foreach (string prefix in prefixes)
        {
            _listener.Prefixes.Add(prefix);
        }

        _serviceProvider = serviceProvider;
        _listener.Start();
    }

    public async Task StartAsync()
    {
        Console.WriteLine("Listening...");
        while (true)
        {
            HttpListenerContext context = await _listener.GetContextAsync();
            _ = HandleRequestAsync(context);
        }
    }

    private async Task HandleRequestAsync(HttpListenerContext context)
    {
        string responseString = "";
        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;
        string requestBody;

        using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
        {
            requestBody = await reader.ReadToEndAsync();
        }

        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            switch (request.HttpMethod)
            {
                case "POST":
                    if (request.Url.AbsolutePath == "/api/User")
                    {
                        var user = JsonConvert.DeserializeObject<User>(requestBody);
                        dbContext.Users.Add(user);
                        await dbContext.SaveChangesAsync();
                        responseString = JsonConvert.SerializeObject(user);
                    }
                    break;

                case "GET":
                    if (request.Url.AbsolutePath.StartsWith("/api/User/"))
                    {
                        int id = int.Parse(request.Url.AbsolutePath.Substring(10));
                        var user = await dbContext.Users.FindAsync(id);
                        responseString = user != null ? JsonConvert.SerializeObject(user) : "User not found";
                    }
                    break;

                case "PUT":
                    if (request.Url.AbsolutePath.StartsWith("/api/User/"))
                    {
                        int id = int.Parse(request.Url.AbsolutePath.Substring(10));
                        var user = JsonConvert.DeserializeObject<User>(requestBody);
                        user.ID = id;
                        dbContext.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        await dbContext.SaveChangesAsync();
                        responseString = "User updated successfully";
                    }
                    break;

                case "DELETE":
                    if (request.Url.AbsolutePath.StartsWith("/api/User/"))
                    {
                        int id = int.Parse(request.Url.AbsolutePath.Substring(10));
                        var user = await dbContext.Users.FindAsync(id);
                        if (user != null)
                        {
                            dbContext.Users.Remove(user);
                            await dbContext.SaveChangesAsync();
                            responseString = "User deleted successfully";
                        }
                        else
                        {
                            responseString = "User not found";
                        }
                    }
                    break;
            }
        }

        byte[] buffer = Encoding.UTF8.GetBytes(responseString);
        response.ContentLength64 = buffer.Length;
        await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        response.OutputStream.Close();
    }

    public void Stop()
    {
        _listener.Stop();
        _listener.Close();
    }
}
