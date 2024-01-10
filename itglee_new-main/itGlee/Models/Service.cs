namespace itGlee.Models
{
    public class Service
    {
        public string title { get; set; }
        public string description { get; set; }

        public Service(string title, string description)
        {
            this.title = title;
            this.description = description;
        }

        public static List<Service> GetServices()
        {
            return new List<Service>() {
                new Service("Consulting","Our technology expertise can provide you with the right tools to change or even transform your business.Focusing on delivery and acceptance, our consultants are used to dealing with challenging projects.We design lean solutions that meet and exceed your needs, enabling you to improve your business and stand out from your competitors."),
                new Service("Custom Developing","Our passion is to help organisations work smarter and grow faster by adopting the most effective tools to boost productivity, create more value, and reduce waste. Our tailored and innovative solutions aim to achieve high rates of technology adoption, reducing costs and improving operations surveillance."),
                new Service("Outsourcing","We believe we are playing a role inside a bigger act. We are not alone and our expert resources help our clients to be more effective, integrating existing solutions, designing new approaches and delivering higher value for them and their partners."),
                new Service("On-Site analysis & Tech Assurance","Confidence is key in building a strong relationship with our clients. In addition to our remote development services we provide on-site analysis, design, and leadership through some of our most experienced consultants to ensure that we optimize engagement between our clients and our developers."),
            };
        }
    }
}
