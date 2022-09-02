using LevelModel.Models;
using LevelModel.Models.Components;
using LevelModel.Models.Components.Art;
using System.Collections.Generic;

namespace Builders.DataStructures.DTO
{
    public class ExtendDTO
    {

        public string Title { get; set; }

        public Level Level { get; set; }

        public List<DrawArt> ArtToAdd { get; set; }

        public List<Block> BlocksToAdd { get; set; }

        public int PaddingX { get; set; }

        public int PaddingY { get; set; }

        public int Multiplier { get; set; }

        public ExtendType Type { get; set; }

        public enum ExtendType { Art0 = 1, Art1, Blocks }

        public bool OffsetArtPositionToLastBlock { get; set; }

        public int OffetX {get; set; }
        public int OffetY { get; set; }

    }
}
