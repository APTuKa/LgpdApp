namespace LgpdApp.Server.Models
{
    public class Child
    {
        public Guid Id { get; set; }             
        public Guid? LogopedId { get; set; }

        public User User { get; set; }            
        public User Logoped { get; set; }         
        public SpeechCard SpeechCard { get; set; }
    }
}
