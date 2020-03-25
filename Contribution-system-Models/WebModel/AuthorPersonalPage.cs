namespace Contribution_system_Models.WebModel
{
    public class AuthorManuscriptNum
    {
        public int DarftManuscript { get; set; }
        public int ReviewsManusript { get; set; }
        public int CompleteManuscript { get; set; }
    }

    public class CompleteManuscript
    {
        public string avtor { get; set; }

        public string Titile { get; set; }

        public string KeyWord { get; set; }

        public string Author { get; set; }

        public string Time { get; set; }
    }
}