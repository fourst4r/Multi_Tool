using Builders.DataStructures.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserInterface.DataStructures.Info
{
    class RemoveBlocksInfo
    {

        public RemoveBlocksDTO DTO { get; set; }

        public int LevelID { get; set; }


        public string Title { get; set; }

        public RemoveBlocksInfo()
        {
            DTO = new RemoveBlocksDTO();
        }
    }
}
