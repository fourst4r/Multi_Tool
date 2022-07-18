namespace LevelModel.DTO
{
    public class Message
    {
        public string Content { get; set; }

        public enum MessageType { Warning, Normal }; 

        public MessageType Type { get; set; }

        public Message(string content, MessageType t) {
            Content = content;
            Type = t;
        }
    }
}
