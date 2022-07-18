using Builders.DataStructures.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserInterface.DataStructures.Info
{
    internal class RemoveArtInfo
    {

        public RemoveArtDTO DTO { get; set; }

        public int LevelID { get; set; }

        public string Title { get; set; }

        public RemoveArtInfo()
        {
            DTO = new RemoveArtDTO();
        }
    }
}
