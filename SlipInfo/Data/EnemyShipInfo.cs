using System;
using System.Collections.Generic;
using System.Text;

namespace SlipInfo.Data
{
    class EnemyShipInfo
    {
        public float maxHealth;
        public float minHealth;
        public float currentHealth;

        public string name;
        public string invaders;
        public string intel;

        public uint threatLevel;
        public uint cargoLevel;
        public uint speedLevel;

        public EnemyShipInfo(MpShipController ship, MpScenarioController scenario)
        {
            if (ship != null && scenario != null && scenario.CurrentScenario.Battle != null)
            {
                Plugin.Log.LogInfo("Part1");
                maxHealth = ship.EnemyShipHealth.Max;
                minHealth = ship.EnemyShipHealth.Min;
                currentHealth = ship.EnemyShipHealth.Current;
                Plugin.Log.LogInfo($"Part2 {maxHealth} {minHealth} {currentHealth}");

                BattleScenarioVo battle = scenario.CurrentScenario.Battle;
                name = battle.Metadata.EnemyName;
                invaders = battle.Metadata.InvaderDescription;
                intel = battle.Metadata.IntelDescription;
                Plugin.Log.LogInfo($"Part3 {name} : {invaders} : {intel}");

                threatLevel = battle.Metadata.ThreatLevel;
                cargoLevel = battle.Metadata.CargoLevel;
                speedLevel = battle.Metadata.SpeedLevel;
                Plugin.Log.LogInfo($"Part4 {threatLevel} {cargoLevel} {speedLevel}");
            } else
            {
                maxHealth = 0;
                minHealth = 0;
                currentHealth = 0;

                name = "";
                invaders = "";
                intel = "";

                threatLevel = 0;
                cargoLevel = 0;
                speedLevel = 0;
            }
        }
    }
}
