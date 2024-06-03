using SlipInfo.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlipInfo.Responses
{
    class CrewCrewmateResponse
    {
        public CrewmateInfo crewmate;

        public CrewCrewmateResponse(Crewmate crewmateIn)
        {
            if (crewmateIn == null)
            {
                crewmate = null;
                return;
            }

            crewmate = new CrewmateInfo(crewmateIn);        
        }
    }
}
