using System;
using System.Collections.Generic;
using System.Text;

namespace SlipInfo.Data
{
    class ShipInfo
    {
        public float maxHealth;
        public float minHealth;
        public float currentHealth;
        public int maxFuel;
        public int currentFuel;
        public int currentSalvage;
        public int currentGems;
        public List<ShipTech> shipTech;

        public ShipInfo(MpShipController ship, MpShipTechController shipTech)
        {
            if (ship != null)
            {
                maxHealth = ship.CaptainShipHealth.Max;
                minHealth = ship.CaptainShipHealth.Min;
                currentHealth = ship.CaptainShipHealth.Current;

                maxFuel = ship.CaptainFuelTank.CurrentCapacity;
                currentFuel = ship.CaptainFuelTank.CurrentAmount;

                currentSalvage = ship.CaptainShipInventory.GetInventoryAmount(ShipItemType.Salvage);
                currentGems = ship.CaptainShipInventory.GetInventoryAmount(ShipItemType.Gems);
            }

            if (shipTech != null)
            {
                this.shipTech = ShipTech(shipTech);
            }
            else
            {
                this.shipTech = new List<ShipTech>();
            }
        }

        private static List<ShipTech> ShipTech(MpShipTechController shipTech)
        {
            List<ShipTech> techs = new List<ShipTech>();

            if (shipTech == null || shipTech.AllTechs == null)
            {
                return techs; // Empty List
            }

            foreach (var tech in shipTech.AllTechs)
            {
                if (tech == null || tech.DefVo == null)
                {
                    continue; // Skip null techs
                }

                techs.Add(new ShipTech(tech));
            }
            return techs;
        }
    }

    class ShipTech
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public int Level { get; set; }
        public int MaxLevel { get; set; }
        public bool IsActive { get; set; }
        public string Color { get; set; }

        public ShipTechUnitType UnitType { get; set; }
        public List<ShipTechLevel> Levels { get; set; }
        public ShipTech(AbstractShipTech tech)
        {
            if (tech == null || tech.DefVo == null)
            {
                Name = "Unknown";
                ShortDescription = "Unknown";
                LongDescription = "Unknown";
                Level = 0;
                MaxLevel = 0;
                IsActive = false;
                Levels = new List<ShipTechLevel>();
                Color = "#FFFFFFFF";
                UnitType = ShipTechUnitType.VALUE;
            }
            else
            {
                Name = tech.DefVo.Title;
                ShortDescription = tech.DefVo.ShortDescription;
                LongDescription = tech.DefVo.LongDescription;
                Level = tech.Level;
                MaxLevel = tech.MaxLevel;
                IsActive = tech.IsActive();
                Levels = GetLevels(tech);
                Color = tech.DefVo.Color;
                UnitType = tech.UnitType;
            }
        }

        private static List<ShipTechLevel> GetLevels(AbstractShipTech tech)
        {
            List<ShipTechLevel> levels = new List<ShipTechLevel>();
            foreach (var level in tech.DefVo.Levels)
            {
                levels.Add(new ShipTechLevel(level));
            }
            return levels;
        }
    }

    class ShipTechLevel
    {
        public int Level { get; set; }
        public float Value { get; set; }
        public int Cost { get; set; }

        public ShipTechLevel(ShipTechLevelDefVo level)
        {
            Level = level.Level;
            Value = level.Value;
            Cost = level.Cost;
        }
    }
}
