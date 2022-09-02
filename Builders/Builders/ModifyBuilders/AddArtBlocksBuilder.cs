using System;
using System.Collections.Generic;
using System.Linq;
using Builders.DataStructures.DTO;
using LevelModel.Models;
using LevelModel.Models.Components;
using LevelModel.Models.Components.Art;
using static Builders.DataStructures.DTO.AddArtBlocksDTO;
using static LevelModel.Models.Level;

namespace Builders.Builders.ModifyBuilders
{
    class AddArtBlocksBuilder
    {

        private AddArtBlocksDTO _info;
        private int _currentX;
        private int _currentY;

        public AddArtBlocksBuilder(AddArtBlocksDTO info)
        {
            _info     = info;
            _currentX = -GetArtList().Sum(b => b.X);
            _currentY = -GetArtList().Sum(b => b.Y);

            Build();
        }

        private void Build()
        {
            foreach(var b in  _info.Level.Blocks)
            {
                _currentX += b.X * Block.PIXELS_PER_BLOCK;
                _currentY += b.Y * Block.PIXELS_PER_BLOCK;

                if(b.Id == _info.BlockId)
                {
                    AddFakeBlock();
                    _currentX = 0;
                    _currentY = 0;
                }
            }
        }

        private List<TextArt> GetArtList() {

            switch (_info.ArtType)
            {
                case ArtType.TextArt00:
                    return _info.Level.TextArt00;

                case ArtType.TextArt0:
                    return _info.Level.TextArt0;

                case ArtType.TextArt1:
                    return _info.Level.TextArt1;

                case ArtType.TextArt2:
                    return _info.Level.TextArt2;

                case ArtType.TextArt3:
                    return _info.Level.TextArt3;

                default: throw new Exception("Invalid Art Type.");
            }
        }

        private void AddFakeBlock()
        {
            var textArt = new TextArt(false)
            { 
                X       = _currentX,
                Y       = _currentY,
                ImageId = _info.ArtBlockId,
                Width   = _info.Width,
                Height  = _info.Height
            };

            GetArtList().Add(textArt);
        }
    }
}
