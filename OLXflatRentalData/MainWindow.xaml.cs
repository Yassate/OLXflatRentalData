using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OLXflatRentalData
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string path = @"C:\Users\Bartek\Desktop\htmlForDebug2.html";
        private OlxDataCollector dataCollector;
        List<FlatData> flats;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CollectData_Click(object sender, RoutedEventArgs e)
        {
            string inputHtml;
            string myHardCodedURL = "https://www.olx.pl/oferta/al-jaworowa-mieszkanie-po-remoncie-dla-pary-CID3-IDu0j39.html#95adc7dfe0;promoted";
            inputHtml = GetResponse(myHardCodedURL).Result;
            dataCollector = new OlxDataCollector(inputHtml);
            //inputHtml = File.ReadAllText(path);
            //saveToFile(inputHtml);
            //processOlxOfferList(inputHtml);
            dataCollector.processOffer();
            flats = dataCollector.GetCollectedData();
        }

        private static async Task<string> GetResponse(string url)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");

            var response = await httpClient.GetAsync(new Uri(url)).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
            using (var decompressedStream = new GZipStream(responseStream, CompressionMode.Decompress))
            using (var streamReader = new StreamReader(decompressedStream))
            {
                return await streamReader.ReadToEndAsync().ConfigureAwait(false);
            }
        }

        private void saveToFile(string inputHTML)
        {
            File.WriteAllText(path, inputHTML);
        }

    }
}
