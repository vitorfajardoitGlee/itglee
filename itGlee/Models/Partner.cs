namespace itGlee.Models
{
    public class Partner
    {
        public string Href { get; set; }
        public string Title { get; set; }
        public string Src { get; set; }

        public static List<Partner> GetPartners()
        {
            return new List<Partner>
            {
                new Partner { Href= "https://www.outsystems.com",Title="Outsystems",Src="images/partners/Outsystems.png"},
                new Partner { Href= "https://www.indorama.com/",Title="Indorama",Src="images/partners/Indorama.jpg"},
                new Partner { Href= "https://www.criver.com/",Title="Charles River Laboratories International Inc.",Src="images/partners/CharlesRiverLabs.jpg"},
                new Partner { Href= "https://www.nttdata.com/global/en/",Title=@"NTT Data",Src="images/partners/NTTData.jpg"},
                new Partner { Href= "https://www.deloitte.com/global/en.html",Title="Deloitte",Src="images/partners/Deloitte.jpg"}
            };
        }
    }
}
