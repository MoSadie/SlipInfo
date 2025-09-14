using SlipInfo.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlipInfo.Responses
{
    class ShipResponse
    {
        ShipInfo ship;

        public ShipResponse(MpShipController ship, MpShipTechController tech)
        {
            if (ship == null)
            {
                this.ship = null;
            }
            else
            {
                this.ship = new ShipInfo(ship, tech);
            }
        }
    }
}
