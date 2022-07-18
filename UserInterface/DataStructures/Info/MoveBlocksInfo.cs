using System;
using System.Text;
using System.Collections.Generic;
using LevelModel.Models;
using Builders.DataStructures.DTO;

namespace UserInterface.DataStructures.Info
{
    class MoveBlocksInfo
    {

        public int X { get; set; }

        public int Y { get; set; }

        public int LevelID { get; set; }

        public Level Level { get; set; }

        public string Title { get; set; }

        public bool MoveArt { get; set; }


        public MoveBlocksInfo(bool moveArt)
        {
            MoveArt = moveArt;
        }

        public MoveArtDTO ArtDTO => new MoveArtDTO {
            Level   = Level,
            ArtType = Level.ArtType.All,
            X       = X * 30,
            Y       = Y * 30,
        };

    }
}
