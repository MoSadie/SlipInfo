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
            foreach (var tech in shipTech.AllTechs)
            {
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
        public List<ShipTechLevel> Levels { get; set; }
        public ShipTech(AbstractShipTech tech)
        {
            Name = tech.DefVo.Title;
            ShortDescription = tech.DefVo.ShortDescription;
            LongDescription = tech.DefVo.LongDescription;
            Level = tech.Level;
            MaxLevel = tech.MaxLevel;
            IsActive = tech.IsActive();
            Levels = GetLevels(tech);
            Color = tech.DefVo.Color;
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
