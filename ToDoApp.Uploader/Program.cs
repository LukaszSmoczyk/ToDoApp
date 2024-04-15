using Azure.Messaging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ToDoApp.Data.Models;
using static System.Net.Mime.MediaTypeNames;
using CloudEvent = ToDoApp.Data.Models.CloudEvent;

while (true)
{
    Console.WriteLine($"====================================");
    Console.WriteLine($"Nowy run");
    var apiUrl = "http://localhost:34035/api/Item/feed-list";
    var timeOut = 10000; //Miliseconds
    var existingEventsList = await LoadFile();
    var lastEventId = await RetrieveLastEventId(existingEventsList);
    var newEventsList = await GetTodoItems(apiUrl, timeOut, lastEventId);
    await MergeResults(existingEventsList, newEventsList);
    await Task.Delay(10000);
    Console.WriteLine($"====================================");
    Console.WriteLine($"Koniec runu");
}

static async Task<List<CloudEvent>?> GetTodoItems(string baseUrl, int timeOut, Guid? lastEventId)
{
    try
    {
        using (var client = new HttpClient())
        {
            var url = string.Empty;
            if (lastEventId != null)
            {
                url = baseUrl + "?lastEventId=" + lastEventId + "&timeout=" + timeOut;
            }
            else
            {
                url = baseUrl;
            }
            // Zapytanie do API
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string json = await response.Content.ReadAsStringAsync();
                var cloudEvents = JsonConvert.DeserializeObject<List<CloudEvent>>(json);
                Console.WriteLine($"Pomyślnie pobrano {cloudEvents.Count} elementów");
                return cloudEvents;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                Console.WriteLine("Brak nowych elementów");
                return null;
            }
            else
            {
                Console.WriteLine("Błąd podczas pobierania danych z API. Kod błędu: " + response.StatusCode + response.Content);
                return null;
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Wystąpił błąd podczas komunikacji z API: " + ex.Message);
        return null;
    }
}

static async Task<Guid?> RetrieveLastEventId(List<CloudEvent>? cloudEvents)
{
    if (cloudEvents != null && cloudEvents.Count > 0)
    {
        var lastEventId = cloudEvents
            .OrderByDescending(c => c.Data?.UpdatedAt)
            .Select(c => c.Id)
            .FirstOrDefault();

        return lastEventId;
    }
    else
    {
        Console.WriteLine("Plik nie zawiera żadnych elementów");
        return null;
    }
}

static async Task<List<CloudEvent>?> LoadFile()
{
    var text = File.ReadAllText("ToDoItemsCurrentSnapshot.json");

    if (!string.IsNullOrEmpty(text))
    {
        var cloudEvents = JsonConvert.DeserializeObject<List<CloudEvent>>(text);
        return cloudEvents;
    }
    else
    {
        Console.WriteLine("Plik nie zawiera żadnych elementów");
        return null;
    }
}

static async Task MergeResults(List<CloudEvent>? existingEvents, List<CloudEvent>? newEvents)
{
    var result = string.Empty;
    try
    {
        if (existingEvents != null &&  newEvents != null)
        {
            foreach (var newEvent in newEvents)
            {
                var existingEvent = existingEvents
                    .Find(x => x.Data?.Id == newEvent.Data?.Id);

                if (existingEvent != null)
                {
                    existingEvents.Remove(existingEvent); 
                }
            }
            existingEvents.AddRange(newEvents);
            var list = existingEvents
                .OrderBy(x => x.Data?.UpdatedAt)
                .ToList();
            result = JsonConvert.SerializeObject(list, Formatting.Indented);

        }
        else if (existingEvents != null)
        {
            result  = JsonConvert.SerializeObject(existingEvents, Formatting.Indented);
        }
        else if (newEvents != null)
        {
            result = JsonConvert.SerializeObject(newEvents, Formatting.Indented);
        }
        await File.WriteAllTextAsync("ToDoItemsCurrentSnapshot.json", result);
        Console.WriteLine($"Zapisano plik");
    }
    catch (Exception ex)
    {
        await File.WriteAllTextAsync("ToDoItemsCurrentSnapshot.json", null);
        Console.WriteLine($"Błąd: {ex.Message}");
    }
}
