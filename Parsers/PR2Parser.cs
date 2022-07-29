using System.Collections.Generic;
using LevelModel.Models;
using LevelModel.Models.Components;
using Parsers.DTO;
using Parsers.Parsers;

namespace Parsers
{
    public static class PR2Parser
    {

        public static LevelDTO Level(string data) => new LevelParser(data).Result;

        public static List<SearchResultLevel> SearchResult(string result) => new SearchResultParser(result).Levels;

        public static List<LoadResultLevel> LoadResult(string result) => new LoadResultParser(result).Levels;

        public static List<Block> Blocks(string blocks) => new BlockParser(blocks).Result;

    }
}
