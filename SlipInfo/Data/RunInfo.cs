using System;
using System.Collections.Generic;
using System.Text;

namespace SlipInfo.Data
{
    class RunInfo
    {
        public string region;
        public string regionDescription;
        public string sector;
        public int runId;

        public RunInfo(MpCampaignController mpCampaignController)
        {
            if (mpCampaignController == null)
            {
                region = null;
                regionDescription = null;
                sector = null;
                runId = -1;
            } else if (!mpCampaignController.IsCampaignInProgress)
            {
                region = "Space";
                regionDescription = "The vast expanse of space. Perfect place to plan the next adventure!";
                sector = "The Void";
                runId = -1;
            }
            else
            {
                if (mpCampaignController.CurrentCampaign == null)
                {
                    Plugin.debugLogInfo("Current campaign is null");
                    return;
                } else if (mpCampaignController.CurrentCampaign.CaptainCampaign != null) { // Only Captains get this info
                    region = mpCampaignController.CurrentCampaign.CaptainCampaign.RegionVo.Metadata.Name;
                    regionDescription = mpCampaignController.CurrentCampaign.CaptainCampaign.RegionVo.Metadata.Description;
                    sector = mpCampaignController.CurrentCampaign.CaptainCampaign.CurrentSectorVo.Definition.Name;
                }

                runId = mpCampaignController.CurrentCampaign.CampaignId;
            }
        }
    }
}
