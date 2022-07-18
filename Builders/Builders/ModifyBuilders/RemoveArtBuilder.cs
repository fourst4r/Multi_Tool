using Builders.DataStructures.DTO;

namespace Builders.Builders.ModifyBuilders
{
    internal class RemoveArtBuilder
    {
        private RemoveArtDTO _info;

        public RemoveArtBuilder(RemoveArtDTO info)
        {
            _info = info;

            if (_info?.Level == null)
                return;

            RemoveArt();
        }

        private void RemoveArt()
        {
            switch (_info.ArtType)
            {
                case LevelModel.Models.Level.ArtType.TextArt00:
                    _info.Level.TextArt00.Clear();
                    break;
                case LevelModel.Models.Level.ArtType.TextArt0:
                    _info.Level.TextArt0.Clear();
                    break;
                case LevelModel.Models.Level.ArtType.TextArt1:
                    _info.Level.TextArt1.Clear();
                    break;
                case LevelModel.Models.Level.ArtType.TextArt2:
                    _info.Level.TextArt2.Clear();
                    break;
                case LevelModel.Models.Level.ArtType.TextArt3:
                    _info.Level.TextArt3.Clear();
                    break;
                case LevelModel.Models.Level.ArtType.DrawArt00:
                    _info.Level.DrawArt00.Clear();
                    break;
                case LevelModel.Models.Level.ArtType.DrawArt0:
                    _info.Level.DrawArt0.Clear();
                    break;
                case LevelModel.Models.Level.ArtType.DrawArt1:
                    _info.Level.DrawArt1.Clear();
                    break;
                case LevelModel.Models.Level.ArtType.DrawArt2:
                    _info.Level.DrawArt2.Clear();
                    break;
                case LevelModel.Models.Level.ArtType.DrawArt3:
                    _info.Level.DrawArt3.Clear();
                    break;
                case LevelModel.Models.Level.ArtType.All:
                    _info.Level.RemoveArt();
                    break;
            }
        }

    }
}
