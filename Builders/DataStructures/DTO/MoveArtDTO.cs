using LevelModel.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static LevelModel.Models.Level;

namespace Builders.DataStructures.DTO
{
    public class MoveArtDTO
    {

        public ArtType ArtType { get; set; }

        public Level Level { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

    }
}
