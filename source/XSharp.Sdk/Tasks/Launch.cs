using System;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using XSharp.Launch.Hosts;
using XSharp.Launch.Hosts.Bochs;

namespace XSharp.Build.Tasks
{
    public class Launch : Task
    {
        enum LaunchTypeEnum
        {
            Bochs,
            VMware,
            HyperV
        }

        private LaunchTypeEnum mLaunchType;

        #region Task Properties

        [Required]
        public string LaunchType
        {
            get
            {
                return mLaunchType.ToString();
            }
            set
            {
#if NETCOREAPP2_0
                mLaunchType = Enum.Parse<LaunchTypeEnum>(value, true);
#else
                mLaunchType = (LaunchTypeEnum)Enum.Parse(typeof(LaunchTypeEnum), value, true);
#endif
            }
        }

        [Required]
        public string ISO { get; set; }

        [Required]
        public string ConfigurationFile { get; set; }

        #endregion

        public override bool Execute()
        {
            IHost xHost = null;

            switch (mLaunchType)
            {
                case LaunchTypeEnum.Bochs:
                    var xLaunchSettings = new BochsLaunchSettings()
                    {
                        ConfigurationFile = Path.GetFullPath(ConfigurationFile),
                        IsoFile = Path.GetFullPath(ISO)
                    };

                    xHost = new BochsHost(xLaunchSettings);
                    break;
                case LaunchTypeEnum.VMware:
                    //xHost = new VMwareHost();
                    break;
                case LaunchTypeEnum.HyperV:
                    //xHost = new HyperVHost(true, "", "");
                    break;
                default:
                    Log.LogError($"Unknown launch type! Launch type: '{mLaunchType}'");
                    return false;
            }

            Log.LogMessage(MessageImportance.High, "LAUNCHING");

            xHost.Start();

            return true;
        }
    }
}
