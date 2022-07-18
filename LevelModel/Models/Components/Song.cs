using System;
using System.Diagnostics;

namespace LevelModel.Models.Components
{
    public class Song
    {


        // Random has no ID in pr2, so i set something here, must be less than -1
        public const int RANDOM = -2;
        public const int NONE = 0;
        public const int DEFAULT = 1;

        public int ID { get; set; }

        public string Name { get; set; }


        public Song(int id)
        {
            ID = id;
            Name = GetName(id);
        }


        private string GetName(int songID)
        {
            switch (songID)
            {
                case 0 : return "None";
                case 1:  return "Miniature Fantasy by Dreamscaper";
                case 2:  return "Under Fire by Andrew-Parker";
                case 3:  return "Paradise on E by API";
                case 4:  return "Crying Soul by Bounc3";
                case 5:  return "My Vision by  MrMaestro";
                case 6:  return "Switchblade by SKAzini";
                case 7:  return "The Wires by Cheez-R-Us";
                case 8:  return "Before Mydnite by F-777";
                //dont exist
                case 10: return "Broked It by SWiTCH";
                case 11: return "Hello? by TMM43";
                case 12: return "Pyrokinesis by Sean Tucker";
                case 13: return "Flowerz 'n' Herbz by Brunzolaitis";
                case 14: return "Instrumental #4 by Reasoner";
                case 15: return "Prismatic by Lunanova";
                case 17: return "PR2 Menu Music by Jiggmin";

                case RANDOM: return "Random";


                default: return string.Empty;
            }
        }


    }
}
