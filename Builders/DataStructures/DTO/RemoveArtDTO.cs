using LevelModel.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static LevelModel.Models.Level;

namespace Builders.DataStructures.DTO
{
    public class RemoveArtDTO
    {

        public Level Level { get; set; }

        public ArtType ArtType { get; set; }

    }
}
