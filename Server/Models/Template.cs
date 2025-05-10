namespace LgpdApp.Server.Models
{
    public class Template
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Game> Games { get; set; }
    }
}
