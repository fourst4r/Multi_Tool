using System.Linq;
using System.Collections.Generic;
using Builders.DataStructures.DTO;
using LevelModel.Models;
using LevelModel.Models.Components;
using LevelModel.Models.Components.Art;

using static LevelModel.Models.Components.Block;

namespace Builders.Builders.ModifyBuilders
{
    internal class MergeBuilder
    {


        private MergeDTO     _info;

        private Point _startPos1    = Point.Empty;
        private Point _startPos2    = Point.Empty;
        private Point _newStartPos2 = Point.Empty;

        public Level Result { get; set; }


        internal MergeBuilder(MergeDTO info)
        {
            _info            = info;

            if (_info.Level1 != null && _info.Level2 != null)
                Build();
        }


        private void Build()
        {
            if (_info.Settings == MergeDTO.KeepSettings.Second) 
                Merge(_info.Level2, _info.Level1);
            else 
                Merge(_info.Level1, _info.Level2);
        }

        private void Merge(Level level1, Level level2) {
            GetStartPositions(level1, level2);
            RemoveStartBlocks(level2);
            MergeArt(level1, level2);
            MergeBlocks(level1, level2);

            Result = level1;
            Result.Title = _info.Title;
            Result.Published = false;
        }

        private void GetStartPositions(Level level1, Level level2) {
            if (level2.Blocks.Count() > 0)
                _startPos1 = level1.Blocks.GetPosition(0);

             if (level2.Blocks.Count() > 0)
                _startPos2 = level2.Blocks.GetPosition(0);

             RemoveStartBlocks(level2);

             if (level2.Blocks.Count() > 0)
                _newStartPos2 = level2.Blocks.GetPosition(0);
        }

        private void RemoveStartBlocks(Level level) {
            level.Blocks = level.Blocks.Remove(b => Block.IsStartBlock(b.Id));
        }

        private void MergeBlocks(Level level1, Level level2) {
            level2.Blocks.SetStartPosition(_info.PaddingX, _info.PaddingY);
            level1.Blocks = level1.Blocks.Merge(level2.Blocks);
        }

        private void MergeArt(Level level1, Level level2) {
            var padding = ArtPadding(level1, level2) * PIXELS_PER_BLOCK;

            level1.DrawArt00 = MergeDrawnArt(level1.DrawArt00, level2.DrawArt00, padding.X, padding.Y);
            level1.DrawArt0  = MergeDrawnArt(level1.DrawArt0,  level2.DrawArt0,  padding.X, padding.Y);
            level1.DrawArt1  = MergeDrawnArt(level1.DrawArt1,  level2.DrawArt1,  padding.X, padding.Y);
            level1.DrawArt2  = MergeDrawnArt(level1.DrawArt2,  level2.DrawArt2,  padding.X, padding.Y);
            level1.DrawArt3  = MergeDrawnArt(level1.DrawArt3,  level2.DrawArt3,  padding.X, padding.Y);
                               
            level1.TextArt00 = MergeTextArt(level1.TextArt00, level2.TextArt00, padding.X, padding.Y);
            level1.TextArt0  = MergeTextArt(level1.TextArt0,  level2.TextArt0,  padding.X, padding.Y);
            level1.TextArt1  = MergeTextArt(level1.TextArt1,  level2.TextArt1,  padding.X, padding.Y);
            level1.TextArt2  = MergeTextArt(level1.TextArt2,  level2.TextArt2,  padding.X, padding.Y);
            level1.TextArt3  = MergeTextArt(level1.TextArt3,  level2.TextArt3,  padding.X, padding.Y);
        }

        private List<DrawArt> MergeDrawnArt(List<DrawArt> art1, List<DrawArt> art2, int xDiff, int yDiff) {
            art2.ForEach(da  => {
                    da.X += xDiff;
                    da.Y += yDiff;
                });

            return art1.Merge(art2);
        }

         private List<TextArt> MergeTextArt(List<TextArt> art1, List<TextArt> art2, int xDiff, int yDiff) {
            var text = art2?.FirstOrDefault();

            if(text == null)
                return art1;

            SetTextStart(art1, text, xDiff, yDiff);
            return art1.Merge(art2);
        }

        private void SetTextStart(List<TextArt> art1, TextArt text, int xDiff, int yDiff) {
            if(art1 == null || art1.Count() == 0) {
                text.X = (text.X - (Block.START_POSITION.X * PIXELS_PER_BLOCK)) + ((_startPos1.X * PIXELS_PER_BLOCK) + xDiff); 
                text.Y = (text.Y - (Block.START_POSITION.Y * PIXELS_PER_BLOCK)) + ((_startPos1.Y * PIXELS_PER_BLOCK) + yDiff);
          
            }
            else {
                text.X += xDiff - art1.Sum(t => t.X);
                text.Y += yDiff - art1.Sum(t => t.Y);
            }
        }

        private Point ArtPadding(Level level1, Level level2) {
            var levelSize1 = Point.Empty;
            level1.Blocks.ForEach(b => levelSize1 += new Point(b.X, b.Y)); 

            return new Point(
                 (_startPos1.X - _startPos2.X) + (levelSize1.X - _startPos1.X) - (_newStartPos2.X - _startPos2.X) + _info.PaddingX,
                 (_startPos1.Y - _startPos2.Y) + (levelSize1.Y - _startPos1.Y) - (_newStartPos2.Y - _startPos2.Y) + _info.PaddingY
            );
        }


    }
}
