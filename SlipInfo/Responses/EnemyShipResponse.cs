using SlipInfo.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlipInfo.Responses
{
    class EnemyShipResponse
    {
        public EnemyShipInfo enemyShip;

        public EnemyShipResponse(MpShipController ship, MpScenarioController scenario)
        {
            if (ship == null || scenario == null || scenario.CurrentScenario.Battle == null)
            {
                enemyShip = null;
                return;
            }
            
            enemyShip = new EnemyShipInfo(ship, scenario);
        }
    }
}
