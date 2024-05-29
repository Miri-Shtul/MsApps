
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ServerApp.Models;

class Program
{
    static async Task Main(string[] args)
    {
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:8080/");

        // Test adding a new user
        User newUser = new User { Name = "John Doe", Email = "john.doe@example.com", Password = "password123" };
        var response = await client.PostAsync("api/User", new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json"));
        string result = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Add User Response: " + result);

        User createdUser = JsonConvert.DeserializeObject<User>(result);

        // Test retrieving the user by ID
        response = await client.GetAsync($"api/User/{createdUser.ID}");
        result = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Get User Response: " + result);

        // Test updating the user
        createdUser.Name = "Jane Doe";
        response = await client.PutAsync($"api/User/{createdUser.ID}", new StringContent(JsonConvert.SerializeObject(createdUser), Encoding.UTF8, "application/json"));
        result = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Update User Response: " + result);

        // Test deleting the user
        response = await client.DeleteAsync($"api/User/{createdUser.ID}");
        result = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Delete User Response: " + result);

        // Test retrieving a user with an invalid ID
        response = await client.GetAsync("api/User/99999");
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("Get Invalid User Response: User not found");
        }
        else
        {
            result = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Get Invalid User Response: " + result);
        }

        // Test deleting a non-existent user
        response = await client.DeleteAsync("api/User/99999");
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("Delete Invalid User Response: User not found");
        }
        else
        {
            result = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Delete Invalid User Response: " + result);
        }
    }
}
