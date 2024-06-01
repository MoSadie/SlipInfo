﻿using SlipInfo;
using System;

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
	public bool isLocalPlayer;

	public CrewmateInfo(Crewmate crewmate)
	{
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
		isLocalPlayer = crewmate.IsLocalPlayer;
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
			Plugin.Log.LogError("An error occurred while checking if the crewmate is the captain.");
            return false;
        }
	}
}
