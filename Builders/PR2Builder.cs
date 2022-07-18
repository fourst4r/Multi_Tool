using LevelModel.Models;
using Builders.DataStructures.DTO;
using Builders.Builders.LevelBuilders;
using Builders.Builders.ModifyBuilders;

namespace Builders
{
    public class PR2Builder
    {

        public Level BuildLevel(BuildDTO info) => new LevelBuilder(info).Result;      

        public Level Merge(MergeDTO info) => new MergeBuilder(info).Result;

        public Level Transform(TransformBlockDTO info) => new TransformBuilder(info).Result;

        public Level Extend(ExtendDTO info) => new ExtendBuilder(info).Result;

        public void AddArtBlocks(AddArtBlocksDTO info) => new AddArtBlocksBuilder(info);

        public void MoveArt(MoveArtDTO info) => new MoveArtBuilder(info);

        public void RemoveBlocks(RemoveBlocksDTO info) => new RemoveBlocksBuilder(info);

        public void RemoveArt(RemoveArtDTO info) => new RemoveArtBuilder(info);


    }
}
