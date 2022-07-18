using Builders.DataStructures.DTO;

namespace UserInterface.DataStructures.Info
{
    public class ExtendInfo : ExtendDTO
    {

        public int LevelID { get; set; }

        public BuildDTO ArtInfo { get; set; }

        public string InputBlocks { get; set; }


        public ExtendInfo()
        {
            ArtInfo = new BuildDTO();
        }


        public void SetPaddingX(int x) {
            PaddingX = x;
            ArtInfo.ImageInfo.PaddingX = x + ImageDTO.CENTER_X;
        }

        public void SetPaddingY(int y)
        {
            PaddingY = y;
            ArtInfo.ImageInfo.PaddingY = y + ImageDTO.CENTER_Y - 200;
        }

    }
}
