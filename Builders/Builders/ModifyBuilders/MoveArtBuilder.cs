using Builders.DataStructures.DTO;
using LevelModel.Models.Components.Art;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Builders.Builders.ModifyBuilders
{
    class MoveArtBuilder
    {

        private MoveArtDTO _info;

        public MoveArtBuilder(MoveArtDTO info)
        {
            _info = info;

            if(info == null || info.Level == null)
                return;

            Build();
        }

        private void Build()
        {
            switch (_info.ArtType)
            {
                case LevelModel.Models.Level.ArtType.TextArt00:
                    MoveTextArt(_info.Level.TextArt00);
                    break;

                case LevelModel.Models.Level.ArtType.TextArt0:
                    MoveTextArt(_info.Level.TextArt0);
                    break;

                case LevelModel.Models.Level.ArtType.TextArt1:
                    MoveTextArt(_info.Level.TextArt1);
                    break;

                case LevelModel.Models.Level.ArtType.TextArt2:
                    MoveTextArt(_info.Level.TextArt2);
                    break;

                case LevelModel.Models.Level.ArtType.TextArt3:
                    MoveTextArt(_info.Level.TextArt3);
                    break;

                case LevelModel.Models.Level.ArtType.DrawArt00:
                    MoveDrawArt(_info.Level.DrawArt00);
                    break;

                case LevelModel.Models.Level.ArtType.DrawArt0:
                    MoveDrawArt(_info.Level.DrawArt0);
                    break;

                case LevelModel.Models.Level.ArtType.DrawArt1:
                    MoveDrawArt(_info.Level.DrawArt1);
                    break;

                case LevelModel.Models.Level.ArtType.DrawArt2:
                    MoveDrawArt(_info.Level.DrawArt2);
                    break;

                case LevelModel.Models.Level.ArtType.DrawArt3:
                    MoveDrawArt(_info.Level.DrawArt3);
                    break;

                case LevelModel.Models.Level.ArtType.All:
                    MoveTextArt(_info.Level.TextArt00);
                    MoveTextArt(_info.Level.TextArt0);
                    MoveTextArt(_info.Level.TextArt1);
                    MoveTextArt(_info.Level.TextArt2);
                    MoveTextArt(_info.Level.TextArt3);
                    MoveDrawArt(_info.Level.DrawArt00);
                    MoveDrawArt(_info.Level.DrawArt0);
                    MoveDrawArt(_info.Level.DrawArt1);
                    MoveDrawArt(_info.Level.DrawArt2);
                    MoveDrawArt(_info.Level.DrawArt3);
                    break;

                default: throw new Exception("Invalid Art Type.");
            }
        }

        private void MoveTextArt(IEnumerable<Art> art)
        {
            if(art == null)
                return;

            if(art.Count() > 0)
            {
                art.First().X += _info.X;
                art.First().Y += _info.Y;
            }
        }

        private void MoveDrawArt(IEnumerable<Art> art)
        {
            if (art == null)
                return;

            foreach (Art a in art)
            {
                a.X += _info.X;
                a.Y += _info.Y;
            }
        } 

    }
}
