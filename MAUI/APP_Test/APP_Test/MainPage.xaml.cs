using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using APP_Test.Resources.Data;
using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;



namespace APP_Test { 



    public partial class MainPage : ContentPage
    {
        HttpClient _httpClient = new HttpClient();
        public MainPage()
        {
            InitializeComponent(); // Dies sollte jetzt gefunden werden
        }

        private async void OnChangeStopIdClicked(object sender, EventArgs e)
        {
            var stopId = StopIdEntry.Text; // Stellt sicher, dass StopIdEntry existiert
            var departures = await FetchDeparturesAsync(stopId); // Beispielmethode
            DeparturesListView.ItemsSource = departures; // Stellt sicher, dass DeparturesListView existiert
        }

        private async Task<List<Departure>> FetchDeparturesAsync(string stopId)
        {
            var url = $"https://smartinfo.ivb.at/api/JSON/PASSAGE?stopID={stopId}";
            var response = await _httpClient.GetFromJsonAsync<ApiResponse>(url);

            // Hier wandelst du die SmartinfoContainer in Departure um
            var departures = response?.Smartinfo
                                    .Select(container => Departure.FromSmartinfo(container.smartinfo))
                                    .ToList();

            return departures;
        }
    }
}
