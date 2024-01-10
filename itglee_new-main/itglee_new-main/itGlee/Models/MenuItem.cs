namespace itGlee.Models
{
    public class MenuItem
    {
        public string Href;
        public string Description;

        public static List<MenuItem> GetMenuItems()
        {
            return new List<MenuItem>()
            {
                new MenuItem{Href="/#services",Description= "Services"},
                new MenuItem{Href="/#partners",Description="Partners"},
                new MenuItem{Href="/#aboutus",Description="About us"},
                new MenuItem{Href="/#contacts",Description="Contacts"},
                new MenuItem{Href="/Careers",Description="Careers"}
            };
        }
    }
}
