using LevelModel.Models;
using System.Collections.Generic;

namespace Builders.DataStructures.DTO
{
    public class TransformBlockDTO
    {

        public List<int> BlockToTransform { get; set; }

        public List<int> BlockToTransformTo { get; set; }

        public Level Level { get; set; }

        public string Title { get; set; }

        public TransformType Type { get; set; }

        public enum TransformType { Blocks = 1, ReverseTraps }


        public TransformBlockDTO()
        {
            BlockToTransform   = new List<int>();
            BlockToTransformTo = new List<int>();
        }


    }
}
