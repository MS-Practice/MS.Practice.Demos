// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using static System.Console;

var order = new Order() { Id = TypeId.TypeId.New(), Name = "Strong Type Order Id Demo" };
var json = JsonSerializer.Serialize(order);
WriteLine(json);
var norder = JsonSerializer.Deserialize<Order>(json);
WriteLine(norder!.ToString());
WriteLine("Hello, World!");

public class Order
{
    public TypeId.TypeId Id { get; set; }
    public string? Name { get; set; }

    public override string ToString()
    {
        return $"""
            Id={Id.Value}
            Name={Name}
            """;
    }
}
