using SlipInfo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SlipInfo.Data
{
	public class CrewmateInfo
	{
		public string name;
		public string archetype;
		public string skin;
		public int level;
		public int xp;
		public float currentHealth;
		public float maxHealth;
		public float currentShields;
		public float maxShields;
		public bool isCaptain;
        public bool isOfficer;
		public List<Role> roles = new List<Role>();
        public bool isLocalPlayer;
		public string playerId;

		public CrewmateInfo(Crewmate crewmate)
		{
			if (crewmate == null)
			{
                SlipInfo.Log.LogError("CrewmateInfo constructor received a null crewmate.");
                return;
            }

            // Get the list of roles for this crewmate, if any
            roles = [];

			if (crewmate.Client != null && crewmate.Client.Roles != null)
			{
				foreach (Role role in RoleHelpers.AllRolesInstances)
                {
                    if (crewmate.Client.Roles.Has(role))
                    {
                        roles.Add(role);
                    }
                }
            }

            // Set the fields of this object from the values of the crewmate object

            name = crewmate.Client != null ? crewmate.Client.Player.DisplayName : "Crew";
			archetype = crewmate.ArchetypeId;
			skin = crewmate.SkinId;
			level = crewmate.Progression != null ? crewmate.Progression.Level : -1;
			xp = crewmate.Progression != null ? crewmate.Progression.TotalXp : -1;
			currentHealth = crewmate.Health;
			maxHealth = crewmate.Stats != null ? crewmate.Stats.MaxHealth : -1;
			currentShields = crewmate.Shields;
			maxShields = crewmate.Stats != null ? crewmate.Stats.MaxShields : -1;
			isCaptain = getIsCaptain(crewmate);
			isOfficer = getIsOfficer(roles);
			isLocalPlayer = crewmate.IsLocalPlayer;
			playerId = (crewmate.Client != null && crewmate.Client.Player != null) ? crewmate.Client.Player.SlipUserDbId : "";
		}

		private bool getIsCaptain(Crewmate crewmate)
		{
			try
			{
				MpCaptainController captains = Svc.Get<MpSvc>().Captains;

				if (captains == null || captains.CaptainClient == null)
				{
					return false;
				}
				else
				{
					return crewmate.Client.Equals(captains.CaptainClient);
				}
			}
			catch (Exception e)
			{
				SlipInfo.Log.LogError($"An error occurred while checking if the crewmate is the captain: {e.Message}");
				return false;
			}
		}

        private readonly Role[] officerRoles =
        [
            Roles.FirstMate,
			Roles.ChiefEngineer,
			Roles.SupplyRunner,
			Roles.Intern
		];

		private bool getIsOfficer(List<Role> roles)
		{
			try
			{
				if (roles == null)
				{
					SlipInfo.DebugLogWarn("Roles is null, returning false for isOfficer.");
					return false;
				} else if (roles.Count == 0)
                {
                    // No roles assigned, return false
                    return false;
                }

                return roles.Intersect(officerRoles).Any();
            }
			catch (Exception e)
			{
				SlipInfo.Log.LogError($"An error occurred while checking if the crewmate is an officer: {e.Message}");
				return false;
			}
		}
	}
}