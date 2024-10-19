using Capstone.Models;

namespace Capstone.ViewModels
{
    public class StudentScrumModel
    {
        private List<Scrum> scrums;
        private List<Response> responses;

        public StudentScrumModel() { }
        
        public StudentScrumModel(List<Scrum> scrums, List<Response> responses)
        {
            this.scrums = scrums;
            this.responses = responses;
        }
        public List<Scrum> Scrums
        {
            get { return scrums; }
            set { scrums = value; }
        }

        public List<Response> Responses
        {
            get { return responses; }
            set { responses = value; }
        }
    }
}
