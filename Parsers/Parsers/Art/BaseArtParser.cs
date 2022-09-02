using LevelModel.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parsers.Parsers.Art
{
    internal abstract class BaseArtParser<T>  : BaseParser
    {

        private Messages _messages;
        private readonly string _artType;

        public List<T> Result { get; set; }


        public BaseArtParser(Messages messages, string type) {
            Result = new List<T>();

            _messages = messages;
            _artType = type;
        }


        protected void SafeParse(Action parseAction) {
            try {
                parseAction?.Invoke();
            }
            catch(Exception ex) {
                _messages.Add("Failed to parse " + _artType + Environment.NewLine + "\tReason: " + ex.Message + Environment.NewLine);
                Result.Clear();
            }
        }


    }
}
