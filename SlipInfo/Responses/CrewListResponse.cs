using System;
using System.Collections.Generic;

public class CrewListResponse
{
	public List<CrewmateInfo> crewList;

	public CrewListResponse(List<Crewmate> crewList)
	{
		// For each crewmate in the list, add them to the response as a new CrewResponse object
		this.crewList = new List<CrewmateInfo>();

		if (crewList == null)
		{
            return;
        }

		foreach (Crewmate crewmate in crewList)
		{
            this.crewList.Add(new CrewmateInfo(crewmate));
        }
	}
}
