using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Converters.DataStructures.DTO;
using LevelModel.Models;
using LevelModel.Models.Components;

namespace Converters.Converters
{
    class LevelToPr2
    {

        internal const string HashSalt = "84ge5tnr";

        private ToPr2DTO _info;
        public string Result { get; }

        public LevelToPr2(ToPr2DTO info)
        {
            _info  = info;
            Result = Convert(info.Level, info.Username, info.Token); 
        }


        private string Convert(Level level, string username, string token)
        {
            string items        = GetItems(level);
            string badHats      = GetBadHats(level);
            string song         = GetSong(level);
            string gameMode     = GetGameMode(level);
            string hasPass      = GetHasPass(level);
            string data         = level.GetData();
            string note         = GetNote(level.Note);
            string published    = GetPublished(level);
            string gravity      = level.Gravity.ToString(CultureInfo.InvariantCulture);
            string maxTime      = level.MaxTime.ToString(CultureInfo.InvariantCulture);
            string time         = level.Time.ToString(CultureInfo.InvariantCulture);
            string minLevel     = level.RankLimit.ToString(CultureInfo.InvariantCulture);
            string cowboyChance = level.CowboyChance.ToString(CultureInfo.InvariantCulture);
            string hash         = GenerateHash(level.Title + username.ToLower(CultureInfo.InvariantCulture) + data + HashSalt);
            string overwrite    = _info.OverWrite ? "1" : "0";

            return EncodeAttribute("credits")            + "=" + EncodeAttribute(level.Credits)    + "&" +
                   EncodeAttribute("live")               + "=" + EncodeAttribute(published)        + "&" + 
                   EncodeAttribute("max_time")           + "=" + EncodeAttribute(maxTime)          + "&" +
                   EncodeAttribute("time")               + "=" + EncodeAttribute(time)             + "&" +
                   EncodeAttribute("items")              + "=" + EncodeAttribute(items)            + "&" + 
                   EncodeAttribute("title")              + "=" + EncodeAttribute(level.Title)      + "&" + 
                   EncodeAttribute("gravity")            + "=" + EncodeAttribute(gravity)          + "&" + 
                   EncodeAttribute("hash")               + "=" + EncodeAttribute(hash)             + "&" + 
                   EncodeAttribute("data")               + "=" + EncodeAttribute(data)             + "&" + 
                   EncodeAttribute("note")               + "=" + EncodeAttribute(note)             + "&" + 
                   EncodeAttribute("min_level")          + "=" + EncodeAttribute(minLevel)         + "&" + 
                   EncodeAttribute("song")               + "=" + EncodeAttribute(song)             + "&" + 
                   EncodeAttribute("hasPass")            + "=" + EncodeAttribute(hasPass)          + "&" + 
                   EncodeAttribute("passHash")           + "=" + string.Empty                      + "&" + 
                   EncodeAttribute("badHats")            + "=" + EncodeAttribute(badHats)          + "&" + 
                   EncodeAttribute("gameMode")           + "=" + EncodeAttribute(gameMode)         + "&" + 
                   EncodeAttribute("cowboyChance")       + "=" + EncodeAttribute(cowboyChance)     + "&" + 
                   EncodeAttribute("token")              + "=" + EncodeAttribute(token)            + "&" +
                   EncodeAttribute("overwrite_existing") + "=" + EncodeAttribute(overwrite);
        }

        private string EncodeAttribute(string text)
        {
            if(_info.EnableEncoding == false)
                return text;

            return System.Web.HttpUtility.UrlEncode(text);
        }

        private string GenerateHash(string stringToHash)
        {
            byte[] bytesToHash = Encoding.UTF8.GetBytes(stringToHash);
            byte[] bytesHashed = (new MD5CryptoServiceProvider()).ComputeHash(bytesToHash);
            return BitConverter.ToString(bytesHashed).Replace("-", "").ToLower(CultureInfo.InvariantCulture);
        }

        private string GetPublished(Level level)
        {
            return level.Published ? "1" : "0";
        }

        private string GetItems(Level level)
        {
            string items = string.Empty;

            if (level.Items == null)
                return items;

            foreach (var item in level.Items)
            {
                if(item.ID != Item.NONE)
                    items += item.ID + "`";
            }
                
            if (items.Length > 0)
                items = items.Substring(0, items.Length - 1);

            return items;
        }

        private string GetBadHats(Level level)
        {
            string hats = string.Empty;

            if (level.BadHats == null)
                return hats;

            foreach (var hat in level.BadHats)
            {
                hats += hat + ",";
            }
                
            if (hats.Length > 0)
                hats = hats.Substring(0, hats.Length - 1);

            return hats;
        }


        private string GetSong(Level level)
        {
            if (level.Song == null)
                return string.Empty;

            if (level.Song.ID != Song.RANDOM)
                return level.Song.ID.ToString(CultureInfo.InvariantCulture);

            return "random";  // Random has no ID
        }

        private string GetGameMode(Level level)
        {
            if (level.GameMode != null)
                return level.GameMode.PR2Name;

            return GameMode.DEFAULT;  
        }

        private string GetHasPass(Level level)
        {
            if (level.HasPassword)
                return "1";

            return "0";
        }

        private string GetNote(string note) {
            if(note == null)
                return string.Empty;

            if(note.Length >= 256)
                return note.Substring(0, 255);

            return note;
        }

    }
}
