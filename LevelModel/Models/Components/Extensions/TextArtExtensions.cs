using System.Text;
using System.Collections.Generic;
using LevelModel.Models.Components.Art;
using LevelModel.Models;

using static LevelModel.Models.Components.Art.Art;

namespace LevelModel.Models.Components
{
    public static class TextArtExtensions
    {

        public static string ToPr2String(this List<TextArt> art) {
            StringBuilder sb = new StringBuilder();
            int pImageId     = 0;

            foreach (var info in art) {
                AddPosition(sb, info);
                AddContent(sb, info, ref pImageId);
                AddSize(sb, info);
                AddEndSymbol(sb);
            }

            return sb.TrimEnd();
        }


        private static void AddEndSymbol(StringBuilder sb) {
            if(sb.Length > 0)
                sb.Length--;

            sb.Append(",");
        }

        private static void AddPosition(StringBuilder sb, TextArt info) {
            sb.Append(info.X + ";");
            sb.Append(info.Y + ";");
        }

        private static void AddSize(StringBuilder sb, TextArt info) {
            if(info.Width != NOT_ASSIGNED) 
                sb.Append(info.Width + ";");

            if(info.Height != NOT_ASSIGNED) 
                sb.Append(info.Height + ";");
        }

        private static void AddContent(StringBuilder sb, TextArt info, ref int pImageId) {
            if (info.IsText) {
                sb.Append("t"        + ";");
                sb.Append(Level.EncodeText(info.Text)  + ";");
                sb.Append(info.Color + ";");
            }
            else {
                if(info.ImageId != pImageId) {
                    sb.Append(info.ImageId + ";");
                    pImageId = info.ImageId;
                }
            }
        }

    }
}
