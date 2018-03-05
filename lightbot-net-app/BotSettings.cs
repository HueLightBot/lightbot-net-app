using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lightbot_net_app
{
    class BotSettings
    {
        public int CheerFloor { get; set; }
        public int HUEGroup { get; set; }
        public string Channel { get; set; }
        public int OffFloor { get; set; }
        public int OnFloor { get; set; }
        public AlertSettings AlertSettings { get; set; }

        public BotSettings()
        {

        }
    }

    struct AlertSettings
    {
        public enum SpecialLightType
        {
            None= 0,
            Blink,
            Loop,


            MAX_LIGHT_TYPES
        }

        public SpecialLightType Cheer { get; set; }
        public SpecialLightType Prime { get; set; }
        public SpecialLightType Sub_1000 { get; set; }
        public SpecialLightType Sub_2000 { get; set; }
        public SpecialLightType Sub_3000 { get; set; }


    }

}
