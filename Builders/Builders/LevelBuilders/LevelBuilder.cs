﻿using System;
using System.Collections.Generic;
using LevelModel;
using LevelModel.Models;
using LevelModel.Models.Components;
using LevelModel.Models.Components.Art;
using Builders.DataStructures.DTO;
using Builders.Builders.LevelBuilders.Types;
using Builders.Builders.LevelBuilders.Types.ImageBuilders;

using static Builders.DataStructures.DTO.BuildDTO;
using static Builders.Builders.LevelBuilders.Types.StartBlocksBuilder;
using static Builders.DataStructures.DTO.ImageDTO;

namespace Builders.Builders.LevelBuilders
{
    internal class LevelBuilder
    {

        public Level Result { get; set; }

        private BuildDTO _info;


        public LevelBuilder(BuildDTO info)
        {
            _info = info;

            Build();
        }


        private void Build()
        {
                Result = new Level();

                Result.Credits         = string.Empty;
                Result.Published       = false;
                Result.MaxTime         = 0;
                Result.Items           = GetItems();
                Result.Title           = _info.Title;
                Result.Gravity         = 1.0;
                Result.Note            = "The map was generated by code.";
                Result.RankLimit       = 0;
                Result.Song            = new Song(Song.DEFAULT);
                Result.HasPassword     = false;
                Result.GameMode        = new GameMode(GameMode.DEFAULT);
                Result.CowboyChance    = 100;
                Result.DataVersion     = Constants.DATA_VERSION;
                Result.BackgroundColor = GetBackgroundColor();
                Result.Blocks          = GetBlocks();
                Result.TextArt1        = new List<TextArt>();
                Result.TextArt2        = new List<TextArt>();
                Result.TextArt3        = new List<TextArt>();
                Result.DrawArt1        = GetDrawArt1(_info.ImageInfo);
                Result.DrawArt2        = new List<DrawArt>();
                Result.DrawArt3        = new List<DrawArt>();
                Result.BackgroundImage = Constants.NO_BACKGROUND_IMAGE;
                Result.TextArt0        = new List<TextArt>();
                Result.TextArt00       = new List<TextArt>();
                Result.DrawArt0        = GetDrawArt0(_info.ImageInfo);
                Result.DrawArt00       = new List<DrawArt>();
                Result.Hash            = string.Empty;
        }

        private string GetTextArt1() {
            // string position = "13546;9896";
            // string text = "t;Art1";
            // string color = "0";
            // int width = 180;
            // int height = 100;

            // return position + ";" + text + ";" + color + ";" + width + ";" + height;
            return string.Empty;
        }

        private List<DrawArt> GetDrawArt1(ImageDTO imageInfo) {
            if (imageInfo != null && imageInfo.Type == ImageType.Art1)
                return new SvgToArtBuilder(imageInfo).Result;
            else
                return new List<DrawArt>();
        }

        private List<DrawArt> GetDrawArt0(ImageDTO imageInfo)
        {
            if (imageInfo != null && imageInfo.Type == ImageType.Art0)
                return new ImageToArtBuilder(imageInfo).Result;
            else
                return new List<DrawArt>();
        }

        private List<Block> GetBlocks()
        {
            switch (_info.Type)
            {
                case BuildType.SmallLabyrinth: return new LabyrinthBuilder(50, 30).Blocks;
                case BuildType.LargeLabyrinth: return new LabyrinthBuilder(1500, 50).Blocks;
                case BuildType.Trapwork:       return new TrapworkBuilder(1500, 12).Blocks;
                case BuildType.ShortTraps:     return new ShortTrapsBuilder(70, 12, _info.Difficulty).Blocks;
                case BuildType.MaxBlockLimit:  return new MaxBlockLimitBuilder().Blocks;
                case BuildType.Image:          return new ImageToBlocksBuilder(_info.ImageInfo).Blocks;
                case BuildType.Simple:         return new StartBlocksBuilder(StartType.Simple).Blocks;

                default:                       return new StartBlocksBuilder(StartType.Simple).Blocks;
            }
        }

        private List<Item> GetItems()
        {
            var items = new List<Item>();

            switch (_info.Type)
            {
                case BuildType.Trapwork:
                    items.Add(new Item(Item.LASER_GUN));
                    break;
            }

            return items;
        }

        private string GetBackgroundColor()
        {
            string black = "0";
            // string darkRed  = "3b0505";
            // string darkBlue = "33";

            return black;
        }


    }
}
