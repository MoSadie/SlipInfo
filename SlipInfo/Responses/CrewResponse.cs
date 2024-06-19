using SlipInfo.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlipInfo.Responses
{
    class CrewResponse
    {
        public CrewmateInfo crewmate;

        public CrewResponse(string username)
        {
            MpSvc mpSvc = Svc.Get<MpSvc>();

            if (mpSvc == null)
            {
                crewmate = null;
                return;
            }

            MpCrewController mpCrewController = mpSvc.Crew;

            if (mpCrewController == null)
            {
                crewmate = null;
                return;
            }

            foreach (Crewmate potentialCrewmate in mpCrewController.AllCrew())
            {
                try
                {
                    if (potentialCrewmate.Client.Player.DisplayName.Equals(username, StringComparison.OrdinalIgnoreCase))
                    {
                        crewmate = new CrewmateInfo(potentialCrewmate);
                        return;
                    }
                } catch (Exception ex) {
                    // Just skip that crewmate, likely offline so no player attached.
                    Plugin.Log.LogDebug($"An error occurred while checking crewmate display names in CrewResponse: {ex.Message}");
                    continue;
                }
            }
        }

        public CrewResponse(Crewmate crewmateIn)
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
