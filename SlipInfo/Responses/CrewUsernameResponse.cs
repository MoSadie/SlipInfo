using System;
using System.Collections.Generic;
using System.Text;

namespace SlipInfo.Responses
{
    class CrewUsernameResponse
    {
        public CrewmateInfo crewmate;

        public CrewUsernameResponse(string username)
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
                    continue;
                }
            }
        }
    }
}
