using System;
using System.Globalization;
using System.Diagnostics;

namespace LevelModel.Models.Components
{

    public partial class Item
    {


        public const int NONE = 0;
        public const int LASER_GUN = 1;
        public const int MINE = 2;
        public const int LIGHTNING = 3;
        public const int TELEPORT = 4;
        public const int SUPER_JUMP = 5;
        public const int JET_PACK = 6;
        public const int SPEED_BURST = 7;
        public const int SWORD = 8;
        public const int ICE_WAVE = 9;

        public int ID { get; set; }

        public string Name { get; set; }


        public Item(int id)
        {
            ID = id;
            Name = GetName(id);
        }

        public Item(string name)
        {
            ID = GetID(name);
            Name = (ID == NONE) ? name : GetName(ID);
        }


        private string GetName(int itemID)
        {
            switch (itemID)
            {
                case LASER_GUN:   return "Laser Gun";
                case MINE:        return "Mine";
                case LIGHTNING:   return "Lightning";
                case TELEPORT:    return "Teleport";
                case SUPER_JUMP:  return "Super Jump";
                case JET_PACK:    return "Jet Pack";
                case SPEED_BURST: return "Speed Burst";
                case SWORD:       return "Sword";
                case ICE_WAVE:    return "Ice Wave";

                default: return "Unknown";
            }
        }

        //Old maps can have some extra garbage data after last item, so only check start if matching.
        private int GetID(string itemName)
        {
            if (itemName == null || itemName.Length == 0)
                return NONE;  //No item in map
            if (itemName.StartsWith("laser gun", StringComparison.InvariantCultureIgnoreCase))
                return LASER_GUN;
            if (itemName.StartsWith("mine", StringComparison.InvariantCultureIgnoreCase))
                return MINE;
            if (itemName.StartsWith("lightning", StringComparison.InvariantCultureIgnoreCase))
                return LIGHTNING;
            if (itemName.StartsWith("teleport", StringComparison.InvariantCultureIgnoreCase))
                return TELEPORT;
            if (itemName.StartsWith("super jump", StringComparison.InvariantCultureIgnoreCase))
                return SUPER_JUMP;
            if (itemName.StartsWith("jet pack", StringComparison.InvariantCultureIgnoreCase))
                return JET_PACK;
            if (itemName.StartsWith("speed burst", StringComparison.InvariantCultureIgnoreCase))
                return SPEED_BURST;
            if (itemName.StartsWith("sword", StringComparison.InvariantCultureIgnoreCase))
                return SWORD;
            if (itemName.StartsWith("ice wave", StringComparison.InvariantCultureIgnoreCase))
                return ICE_WAVE;

            return NONE;
        }


    }
}
