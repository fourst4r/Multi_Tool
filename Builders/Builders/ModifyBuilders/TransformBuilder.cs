using System.Linq;
using Builders.DataStructures.DTO;
using LevelModel.Models;
using LevelModel.Models.Components;

using static Builders.DataStructures.DTO.TransformBlockDTO;

namespace Builders.Builders.ModifyBuilders
{
    internal class TransformBuilder
    {


        private TransformBlockDTO _info;
        private bool _isFirstBlock;
        private int _startPositionTrapwork;

        private const int NOT_FOUND = -1;
        private const int LEFT_TRAP_START  = 1400;
        private readonly int RIGHT_TRAP_START = Block.START_POSITION.X;

        public Level Result { get; set; }


        public TransformBuilder(TransformBlockDTO info)
        {
            _info = info;
            _isFirstBlock = true;
            var blocks = info.Level.Blocks;

            if (_info.Type == TransformType.ReverseTraps)
                _startPositionTrapwork = StartPositionTrapwork();

            Build();
        }


        private int StartPositionTrapwork() {
            var blockIDs = _info.Level.Blocks.Select(b => b.Id);

            int rightArrowCount = blockIDs.Count(x => x.Equals(Block.ARROW_RIGHT));
            int leftArrowCount  = blockIDs.Count(x => x.Equals(Block.ARROW_LEFT));

            // return the opposite as the map is being reversed
            return (rightArrowCount > leftArrowCount) ? LEFT_TRAP_START : RIGHT_TRAP_START;
        }

        private int TransformID(int id) {
            int index = _info.BlockToTransform.FindIndex(0, x => x == id);

            return (index == NOT_FOUND) ? id : _info.BlockToTransformTo[index];
        }

        private void TransformBlocks(Block block) {
            block.Id = TransformID(block.Id);

            if (_info.Type == TransformType.ReverseTraps) {
                block.X = (_isFirstBlock) ? _startPositionTrapwork : -block.X;
                _isFirstBlock = false;
            }
        }

        private void Build()
        {
            Result        = _info.Level;
            Result.Title  = _info.Title;
            Result.Blocks.ForEach(TransformBlocks);

            if (_info.Type == TransformType.ReverseTraps)
                Result.RemoveArt();
        }


    }
}
