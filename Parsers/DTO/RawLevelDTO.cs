using LevelModel.DTO;

namespace Parsers.DTO
{
    public class RawLevelDTO
    {

        public string Data { get; set; }

        public string Title { get; set; }


        public Messages Messages { get; set; }


        public RawLevelDTO()
        {
            Messages = new Messages();
            Data     = string.Empty;
            Title    = string.Empty;
        } 
    }
}
