using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using XSharp.Launch;

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
#elif NET462
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
            IHost xHost;

            switch (mLaunchType)
            {
                case LaunchTypeEnum.Bochs:
                    xHost = new Bochs(ConfigurationFile, ISO, null, false, false);
                    break;
                case LaunchTypeEnum.VMware:
                    //xHost = new VMware();
                    break;
                case LaunchTypeEnum.HyperV:
                    //xHost = new HyperV(true, "", "");
                    break;
                default:
                    Log.LogError($"Unknown launch type! Launch type: '{mLaunchType}'");
                    return false;
            }

            Log.LogMessage(MessageImportance.High, "LAUNCHING");

            //xHost.Start();

            return true;
        }
    }
}
