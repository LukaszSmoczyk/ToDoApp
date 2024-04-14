using Microsoft.Extensions.Configuration;

while (true)
{
    var apiUrl = "http://localhost:34035/api/Item/list";
    await GetAndSaveTodoItems(apiUrl);
    await Task.Delay(10000);
}

static async Task GetAndSaveTodoItems(string url)
{
    try
    {
        using (var client = new HttpClient())
        {
            // Zapytanie do API
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                File.WriteAllText("ToDoItemsCurrentSnapshot.json", json);
                Console.WriteLine("Pomyślnie zapisano aktualny stan elementów ToDo.");
            }
            else
            {
                Console.WriteLine("Błąd podczas pobierania danych z API. Kod błędu: " + response.StatusCode);
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Wystąpił błąd podczas komunikacji z API: " + ex.Message);
    }
}
