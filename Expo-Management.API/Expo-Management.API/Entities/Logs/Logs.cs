namespace Expo_Management.API.Entities.Logs
{
    public class Logs
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Message_Template { get; set; }
        public string Level { get; set; }
        public DateTime TimeStamp{ get; set; }
        public string Exception { get; set; }
        public string Properties { get; set; }
    }
}
