using UnityEngine;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class HTTPREQUEST : MonoBehaviour
{
    private string LIST_URL = "https://www.htl-salzburg.ac.at/lehrerinnen.html";
    private string BASE_URL = "https://www.htl-salzburg.ac.at";

    private void Start()
    {
        LadeLehrer();
    }

    async void LadeLehrer()
    {
        string listHtml = await LadeSeite(LIST_URL);
        if (listHtml == "Seite nicht gefunden.")
        {
            Debug.LogError("Lehrerübersicht konnte nicht geladen werden.");
            return;
        }

        // Alle Links zu Lehrerprofilen finden
        MatchCollection links = Regex.Matches(
            listHtml,
            "href=\"(\\/lehrerinnen-details\\/[^\\\"]+\\.html)\""
        );

        int menge = Mathf.Min(5, links.Count);

        for (int i = 0; i < menge; i++)
        {
            string url = BASE_URL + links[i].Groups[1].Value;
            string html = await LadeSeite(url);

            string name = ExtrahiereName(html);

            AusgabeLehrer(i + 1, name);
        }
    }

    // Nur Name + Titel extrahieren
    string ExtrahiereName(string html)
    {
        string regex = @"<h1 class=""value"">\s*<span class=""text"">(.*?)<\/span>";
        return Regex.Match(html, regex, RegexOptions.Singleline).Groups[1].Value.Trim();
    }

    // Ausgabe nur Name + Titel in der Unity-Konsole
    void AusgabeLehrer(int index, string name)
    {
        Debug.Log($"Lehrer {index}: {name}");
    }

    async Task<string> LadeSeite(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch
            {
                Debug.LogError("Fehler beim Laden von: " + url);
                return "Seite nicht gefunden.";
            }
        }
    }
}
