using System.Collections.Generic;
using LevelModel.Models;
using LevelModel.Models.Components;
using Parsers.DTO;
using Parsers.Parsers;

namespace Parsers
{
    public class PR2Parser
    {

        public LevelDTO ParseLevel(string data) => new LevelParser(data).Result;

        public List<SearchResultLevel> ParseSearchResult(string result) => new SearchResultParser(result).Levels;

        public List<LoadResultLevel> ParseLoadResult(string result) => new LoadResultParser(result).Levels;

        public IList<Block> ParseBlocks(string blocks) => new BlockParser(blocks).Result;

    }
}
