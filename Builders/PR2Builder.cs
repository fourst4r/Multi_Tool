using LevelModel.Models;
using Builders.DataStructures.DTO;
using Builders.Builders.LevelBuilders;
using Builders.Builders.ModifyBuilders;

namespace Builders
{
    public static class PR2Builder
    {

        public static Level BuildLevel(BuildDTO info) => new LevelBuilder(info).Result;

        public static Level Merge(MergeDTO info) => new MergeBuilder(info).Result;

        public static Level Transform(TransformBlockDTO info) => new TransformBuilder(info).Result;

        public static Level Extend(ExtendDTO info) => new ExtendBuilder(info).Result;

        public static void AddArtBlocks(AddArtBlocksDTO info) => new AddArtBlocksBuilder(info);

        public static void MoveArt(MoveArtDTO info) => new MoveArtBuilder(info);

        public static void RemoveBlocks(RemoveBlocksDTO info) => new RemoveBlocksBuilder(info);

        public static void RemoveArt(RemoveArtDTO info) => new RemoveArtBuilder(info);

    }
}
