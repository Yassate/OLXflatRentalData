using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLXflatRentalData
{
    public class OlxDataCollector
    {
        private string inputHtml;
        List<FlatData> flats = new List<FlatData>();
        HtmlDocument htmlDoc = new HtmlDocument();

        public OlxDataCollector(string inputHtml)
        {
            this.inputHtml = inputHtml;
            htmlDoc.LoadHtml(inputHtml);
        }

        private void processOlxOfferList()
        {
            HtmlDocument htmlPage = new HtmlDocument();
            htmlPage.LoadHtml(inputHtml);
            List<string> offerLinks = new List<string>();
            string currentLink;

            foreach (HtmlNode link in htmlPage.DocumentNode.SelectNodes("//a[@href]"))
            {
                HtmlAttribute att = link.Attributes["href"];
                currentLink = att.Value;
                if (currentLink.Contains("oferta") && !currentLink.Contains("promoted") && !offerLinks.Contains(currentLink))
                {
                    offerLinks.Add(currentLink);
                }
            }
        }

        public List<FlatData> GetCollectedData()
        {
            return flats;
        }

        public void processOffer()
        {
            HtmlNode infoNode;
            HtmlNodeCollection childNodes;
            HtmlNodeCollection valueContainerNodes;
            string value;
            FlatData currentFlat = new FlatData();

            valueContainerNodes = htmlDoc.DocumentNode.SelectNodes("//td[contains(@class, 'value')]");

            for (int i = 0; i < valueContainerNodes.Count; i++)
            {
                childNodes = valueContainerNodes[i].ChildNodes;
                infoNode = childNodes.FindFirst("a");
                if (infoNode == null)
                {
                    infoNode = childNodes.FindFirst("strong");
                }
                value = clearValue(infoNode.InnerText);
                switch (i)
                {
                    case 0:
                        currentFlat.OfferFrom = value;
                        break;
                    case 4:
                        currentFlat.Area = areaToInt(value);
                        break;
                    case 5:
                        currentFlat.RoomsCount = roomsToInt(value);
                        break;
                    case 6:
                        currentFlat.AdditionalPrice = priceToInt(value);
                        break;
                }
            }
            currentFlat.Title = collectTitle();
            flats.Add(currentFlat);
        }

        private string collectTitle()
        {
            HtmlNode titleBox;
            HtmlNode h1Value;

            titleBox = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'offer-titlebox')]");
            h1Value = titleBox.ChildNodes.FindFirst("h1");
            return clearValue(h1Value.InnerHtml);
        }

        private void collectApproxAddress()
        {

        }

        private void collectNominalPrice()
        {

        }

        private void collectId()
        {

        }

        private void collectCreationDate()
        {

        }



        private string clearValue(string value)
        {
            char[] charsToTrim = { '\n', '\t' };
            value = value.TrimStart(charsToTrim);
            value = value.TrimEnd(charsToTrim);
            value = value.Trim();
            return value;
        }
        private int priceToInt(string priceText)
        {
            int price;
            string[] splitted;
            splitted = priceText.Split(' ');
            priceText = splitted[0];
            price = int.Parse(priceText);
            return price;
        }
        private int areaToInt(string areaText)
        {
            int area;
            areaText = areaText.Substring(0, 2);
            area = int.Parse(areaText);
            return area;
        }
        private int roomsToInt(string roomsText)
        {
            int rooms;
            if (roomsText == "Kawalerka")
            {
                rooms = 1;
            }
            else
            {
                roomsText = roomsText.Substring(0, 1);
                rooms = int.Parse(roomsText);
            }
            return rooms;
        }
    }
}
