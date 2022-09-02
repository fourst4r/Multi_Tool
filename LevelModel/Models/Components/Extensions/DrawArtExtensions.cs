using System;
using System.Text;
using System.Collections.Generic;
using LevelModel.Models.Components.Art;

using static LevelModel.Models.Components.Art.DrawArt;

namespace LevelModel.Models.Components
{

    public static class DrawArtExtensions
    {

        public static string ToPr2String(this List<DrawArt> art) {
            StringBuilder sb = new StringBuilder();
            string pColor    = "0";
            int pSize        = 4;
            bool pErase      = false;

            foreach (var da in art) {
                sb.Append(DrawArtToString(da, pColor, pSize, pErase) + ",");
                pColor = da.Color;
                pSize  = da.Size;
                pErase = da.IsErase;
            }

            return sb.TrimEnd();
        }

        public static void AddDot(this List<DrawArt> art, string color, int size, int x, int y) {
            var da = new DrawArt();

            da.Color = color;
            da.Size = size;
            da.X = x;
            da.Y = y;
            da.Movement.Add(0);
            da.Movement.Add(0);

            art.Add(da);
        }


        private static string DrawArtToString(DrawArt da, string pColor, int pSize, bool pErase) {
            StringBuilder sb = new StringBuilder();

            if (!pColor.Equals(da.Color, StringComparison.InvariantCultureIgnoreCase))
                sb.Append("c" + da.Color + ",");

            if (pErase != da.IsErase)
                sb.Append((da.IsErase ? ERASE : DRAW) + ",");

            if (pSize != da.Size)
                sb.Append("t" + da.Size + ",");

            sb.Append("d" + da.X + ";" + da.Y + ";");

            foreach (var value in da.Movement)
                sb.Append(value + ";");

            return sb.TrimEnd();
        }

    }
}
