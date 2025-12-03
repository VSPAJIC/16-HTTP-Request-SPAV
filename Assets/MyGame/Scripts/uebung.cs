using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;

public class uebung : MonoBehaviour
{
    private string url = "";

    public string responsemsg;

    async void Start()
    {
        await Aufgabe();
    }

    async Task<string> Aufgabe()
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            responsemsg = await response.Content.ReadAsStringAsync();
            Debug.Log("Response:" + responsemsg);
        }
     return responsemsg;
    }

}