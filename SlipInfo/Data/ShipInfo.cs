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

        public ShipInfo(MpShipController ship)
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
        }
    }
}
