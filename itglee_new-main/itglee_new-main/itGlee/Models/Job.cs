namespace itGlee.Models
{
    public class Job
    {
        public string id;
        public string position;
        public string jobpositiondescription;
        public JobDetails job;
        public string imgurl;
    }

    public class JobDetails
    {
        public List<Section> sections;
        public List<string> contractterms;
        public List<string> tags;
    }

    public class Section
    {
        public string header;
        public string text;
        public List<string> bullets;
    }
}
