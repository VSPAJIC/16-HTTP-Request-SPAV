using UnityEngine;
using System.Net.Http;
using System.Text.RegularExpressions;
using TMPro;
using System.Threading.Tasks;
 
public class httpreq : MonoBehaviour
{
    private string[] URLS = new string[]
    {
        "https://www.htl-salzburg.ac.at/lehrerinnen-details/schweiberer-franz-prof-dipl-ing-c-205.html",
        "https://www.htl-salzburg.ac.at/lehrerinnen-details/meerwald-stadler-susanne-prof-dipl-ing-g-009.html"
    };
 
    public TMP_Text lehrer1;
    public TMP_Text lehrer2;
 
    private void Start()
    {
        LehrerDatenLoader();
    }
 
    async void LehrerDatenLoader()
    {
        int lehrerIndex = 0;
       
        foreach (string url in URLS)
        {
            string daten = await DatenVonWebseite(url);
 
            TMP_Text textfeld = null;
            if (lehrerIndex == 0)
            {
                textfeld = lehrer1;
            }
            else if (lehrerIndex == 1)
            {
                textfeld = lehrer2;
            }
 
            AktualisiereFeld(textfeld, daten);
           
            lehrerIndex++;
        }
    }
 
    async Task<string> DatenVonWebseite(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Fehler beim Laden von {url}: {e.Message}");
                return "Seite nicht gefunden.";
            }
        }
    }
 
    void AktualisiereFeld(TMP_Text feld, string htmlContent)
    {
        if (feld == null) return;
 
        string raumMuster = @"<div class=""field Raum"">.*?<span class=""text"">(.*?)<\/span>";
        string stundeMuster = @"<div class=""field SprStunde"">.*?<span class=""text"">(.*?)<\/span>";
 
        string raum = Regex.Match(htmlContent, raumMuster, RegexOptions.Singleline).Groups[1].Value.Trim();
        string stunde = Regex.Match(htmlContent, stundeMuster, RegexOptions.Singleline).Groups[1].Value.Trim();
 
        string nameMuster = @"<h1 class=""value"">\s*<span class=""text"">(.*?)<\/span>";
        string name = Regex.Match(htmlContent, nameMuster, RegexOptions.Singleline).Groups[1].Value.Trim();
 
        feld.text = $"{name}\n\n" +
                    $"Raum:" + " " + $"{raum}\n" +
                    $"Sprechstunde:" + " " + $"{stunde}";
    }
}
 