using System;
using System.Collections.Generic;
using Parsers;
using Parsers.DTO;
using LevelModel.DTO;
using LevelModel.Models;
using LevelModel.Models.Components;

namespace UserInterface.Handlers
{
    internal class ParseHandler : IOHandler
    {


        private PR2Parser _parser;


        public ParseHandler() {
            _parser = new PR2Parser();
        }


        internal Level Parse(string levelData) {
            try {
                var result = _parser.ParseLevel(levelData);
                ShowMessages(result.Messages);
                return result.Level;
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }

            return null;
        }

        internal List<SearchResultLevel> ParseSearchResult(string result) {
            try {
                var levels = _parser.ParseSearchResult(result);

                if (levels.Count == 0)
                    WriteLine(Environment.NewLine + "\tNo levels found!" + Environment.NewLine, ErrorColor);

                return levels;
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }

            return null;
        }

        internal List<LoadResultLevel> ParseLoadResult(string result) {
            try {
                var levels = _parser.ParseLoadResult(result);

                if (levels.Count == 0)
                    WriteLine(Environment.NewLine + "\tNo levels found!" + Environment.NewLine, ErrorColor);

                return levels;
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }

            return null;
        }

        internal IList<Block> ParseBlocks(string blocks) {
            try {
                return _parser.ParseBlocks(blocks);
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }

            return null;
        }

        private void ShowMessages(IEnumerable<Message> warnings) {
            foreach (var msg in warnings) {
                if (msg.Type == Message.MessageType.Warning)
                    WriteLine(Environment.NewLine + "\tWarning: " + msg.Content, WarningColor);
                else
                    WriteLine(Environment.NewLine + "\t" + msg.Content);
            }
        }


    }
}
