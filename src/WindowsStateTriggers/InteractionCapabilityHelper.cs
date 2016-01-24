using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;

namespace WindowsStateTriggers
{
    /// <summary>
    /// 
    /// </summary>
    public class InteractionCapabilityHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static InteractionCapability GetForCurrentView()
        {
            InteractionCapability retVal;

            UserInteractionMode interactionMode = UIViewSettings.GetForCurrentView().UserInteractionMode;

            if (DeviceFamily == DeviceFamily.Team)
                retVal = InteractionCapability.MultiUserMultitouch;
            else if (DeviceFamily == DeviceFamily.Desktop && interactionMode == UserInteractionMode.Mouse)
                retVal = InteractionCapability.MouseAndKeyboard;
            else
                retVal = InteractionCapability.SingleUserMultitouch;

            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        public static DeviceFamily DeviceFamily
        {
            get
            {
                DeviceFamily retVal = DeviceFamily.Unknown;

                var deviceFamily = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily;

                switch (deviceFamily)
                {
                    case "Windows.Mobile":
                        retVal = DeviceFamily.Mobile;
                        break;
                    case "Windows.Desktop":
                        retVal = DeviceFamily.Desktop;
                        break;
                    case "Windows.Team":
                        retVal = DeviceFamily.Team;
                        break;
                    case "Windows.IoT":
                        retVal = DeviceFamily.IoT;
                        break;
                    case "Windows.Xbox":
                        retVal = DeviceFamily.Xbox;
                        break;
                    default:
                        retVal = DeviceFamily.Unknown;
                        break;
                }

                return retVal;
            }
        }
    }
}
