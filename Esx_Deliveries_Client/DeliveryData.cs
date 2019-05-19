using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace Esx_Deliveries_Client
{
    class DeliveryData
    {
        // If you are setting decor codees on your server change the following value to whatever your code is.
        public static int DecorCode                                     = 0;
        public static int DELIVERIES_MIN                                = 0; 
        public static int DELIVERIES_MAX                                = 0;
        public static int DELIVERIES_REWARD_SCOOTER                     = 0;
        public static int DELIVERIES_REWARD_VAN                         = 0;
        public static int DELIVERIES_REWARD_TRUCK                       = 0;

        public static readonly string MODEL_SCOOTER                     = "faggio3";
        public static readonly string MODEL_VAN                         = "pony";
        public static readonly string MODEL_TRUCK                       = "mule";

        public static readonly float COLLECT_GOODS_SCALE_VECTOR_SCOOTER = 0.6f;
        public static readonly float COLLECT_GOODS_SCALE_VECTOR_VAN     = 3.0f;
        public static readonly float COLLECT_GOODS_SCALE_VECTOR_TRUCK   = 4.5f;

        public static int SAFE_DEPOSIT_SCOOTER                          = 0;
        public static int SAFE_DEPOSIT_VAN                              = 0;
        public static int SAFE_DEPOSIT_TRUCK                            = 0;

        public static Dictionary<string, string> _U = new Dictionary<string, string>
        {
            ["DELIVERY_NOT_AVAILABLE"]      = "This mode is ~r~not available~w~ yet. Please use the ~g~bike~w~ or ~g~van~w~.",
            ["START_DELIVERY"]              = "Press ~INPUT_CONTEXT~ to start a delivery route. You will need to place a safe deposit of ~g~$",
            ["SAFE_DEPOSIT_RECEIVED"]       = "Vehicle's safe deposit was removed from your bank account.",
            ["SAFE_DEPOSIT_RETURNED"]       = "Your vehicle's safe deposit was returned to your bank account.",
            ["SAFE_DEPOSIT_WITHHELD"]       = "Delivery ~r~Failed~w~. Your vehicle's safe deposit has been ~r~withheld~w~ by the bank.",
            ["DELIVERY_POINT_REWARD"]       = "Delivery complete. Your received ~g~$",
            ["GET_BACK_IN_VEHICLE"]         = "Get into your vehicle!",
            ["REMOVE_GOODS"]                = "Press ~INPUT_CONTEXT~ to remove ~y~delivery goods~w~.",
            ["DELIVER_INSIDE_SHOP"]         = "Get to the shop and deliver the ~y~goods~w~.",
            ["GET_BACK_TO_DELIVERYHUB"]     = "Get back to delivery hub to return the vehicle.",
            ["DELIVERY_VEHICLE_RETURNED"]   = "Your delivery vehicle has been returned.",
        };

        public static List<Vector4> ParkingSpawns = new List<Vector4>
        {
            new Vector4(-310.5f, -1011.08f, 30.39f, 252.0f),
            new Vector4(-309.35f, -1008.24f, 30.39f, 251.15f),
            new Vector4(-311.53f, -1013.72f, 30.39f, 252.0f),
            new Vector4(-305.17f, -1013.04f, 30.39f, 70.78f),
            new Vector4(-307.33f, -1002.65f, 30.39f, 248.33f),
            new Vector4(-322.99f, -1000.16f, 30.39f, 73.11f),
            new Vector4(-329.65f, -1004.1f, 30.39f, 253.11f),
            new Vector4(-324.65f, -1006.08f, 30.39f, 69.82f),
            new Vector4(-326.87f, -1011.45f, 30.39f, 73.49f),
        };

        public static List<Tuple<Vector3, Vector3>> DeliveryLocationsScooter = new List<Tuple<Vector3, Vector3>>()
        {
            Tuple.Create(   new Vector3(-153.19f, -838.31f, 30.12f),    new Vector3(-143.85f, -846.3f, 30.6f)),
            Tuple.Create(   new Vector3(37.72f, -795.71f, 30.93f),      new Vector3(44.94f, -803.24f, 31.52f)),
            Tuple.Create(   new Vector3(111.7f, -809.56f, 30.71f),      new Vector3(102.19f, -818.22f, 31.35f)),
            Tuple.Create(   new Vector3(132.61f, -889.41f, 29.71f),     new Vector3(121.25f, -879.82f, 31.12f)),
            Tuple.Create(   new Vector3(54.41f, -994.86f, 28.7f),       new Vector3(43.89f, -997.98f, 29.34f)),
            Tuple.Create(   new Vector3(54.41f, -994.86f, 28.7f),       new Vector3(57.65f, -1003.72f, 29.36f)),
            Tuple.Create(   new Vector3(142.87f, -1026.78f, 28.67f),    new Vector3(135.44f, -1031.19f, 29.35f)),
            Tuple.Create(   new Vector3(248.03f, -1005.49f, 28.61f),    new Vector3(254.83f, -1013.25f, 29.27f)),
            Tuple.Create(   new Vector3(275.68f, -929.64f, 28.47f),     new Vector3(285.55f, -937.26f, 29.39f)),
            Tuple.Create(   new Vector3(294.29f, -877.33f, 28.61f),     new Vector3(301.12f, -883.47f, 29.28f)),
            Tuple.Create(   new Vector3(247.68f, -832.03f, 29.16f),     new Vector3(258.66f, -830.44f, 29.58f)),
            Tuple.Create(   new Vector3(227.21f, -705.26f, 35.07f),     new Vector3(232.2f, -714.55f, 35.78f)),
            Tuple.Create(   new Vector3(241.06f, -667.74f, 37.44f),     new Vector3(245.5f, -677.7f, 37.75f)),
            Tuple.Create(   new Vector3(257.05f, -628.21f, 40.59f),     new Vector3(268.54f, -640.44f, 42.02f)),
            Tuple.Create(   new Vector3(211.33f, -605.63f, 41.42f),     new Vector3(222.32f, -596.71f, 43.87f)),
            Tuple.Create(   new Vector3(126.27f, -555.46f, 42.66f),     new Vector3(168.11f, -567.17f, 43.87f)),
            Tuple.Create(   new Vector3(254.2f, -377.17f, 43.96f),      new Vector3(239.06f, -409.27f, 47.92f)),
            Tuple.Create(   new Vector3(244.49f, 349.05f, 105.46f),     new Vector3(252.86f, 357.13f, 105.53f)),
            Tuple.Create(   new Vector3(130.77f, -307.27f, 44.58f),     new Vector3(138.67f, -285.45f, 50.45f)),
            Tuple.Create(   new Vector3(54.44f, -280.4f, 46.9f),        new Vector3(61.86f, -260.86f, 52.35f)),
            Tuple.Create(   new Vector3(55.15f, -225.54f, 50.44f),      new Vector3(76.29f, -233.15f, 51.4f)),
            Tuple.Create(   new Vector3(44.6f, -138.99f, 54.66f),       new Vector3(50.78f, -136.23f, 55.2f)),
            Tuple.Create(   new Vector3(32.51f, -162.61f, 54.86f),      new Vector3(26.84f, -168.84f, 55.54f)),
            Tuple.Create(   new Vector3(-29.6f, -110.84f, 56.51f),      new Vector3(-23.5f, -106.74f, 57.04f)),
            Tuple.Create(   new Vector3(-95.29f, -87.53f, 57f),         new Vector3(-87.82f, -83.55f, 57.82f)),
            Tuple.Create(   new Vector3(-146.26f, -71.46f, 53.9f),      new Vector3(-132.92f, -69.02f, 55.42f)),
            Tuple.Create(   new Vector3(-238.41f, 91.97f, 68.11f),      new Vector3(-263.61f, 98.88f, 69.3f)),
            Tuple.Create(   new Vector3(-251.45f, 20.43f, 53.9f),       new Vector3(-273.35f, 28.21f, 54.75f)),
            Tuple.Create(   new Vector3(-322.4f, -10.06f, 47.42f),      new Vector3(-315.48f, -3.76f, 48.2f)),
            Tuple.Create(   new Vector3(-431.22f, 14.6f, 45.5f),        new Vector3(-424.83f, 21.74f, 46.27f)),
            Tuple.Create(   new Vector3(-497.33f, -8.38f, 44.33f),      new Vector3(-500.95f, -18.65f, 45.13f)),
            Tuple.Create(   new Vector3(-406.69f, -44.87f, 45.13f),     new Vector3(-429.07f, -24.12f, 46.23f)),
            Tuple.Create(   new Vector3(-433.94f, -76.33f, 40.93f),     new Vector3(-437.89f, -66.91f, 43f)),
            Tuple.Create(   new Vector3(-583.22f, -154.84f, 37.51f),    new Vector3(-582.8f, -146.8f, 38.23f)),
            Tuple.Create(   new Vector3(-613.68f, -213.46f, 36.51f),    new Vector3(-622.23f, -210.97f, 37.33f)),
            Tuple.Create(   new Vector3(-582.44f, -322.69f, 34.33f),    new Vector3(-583.02f, -330.38f, 34.97f)),
            Tuple.Create(   new Vector3(-658.25f, -329f, 34.2f),        new Vector3(-666.69f, -329.06f, 35.2f)),
            Tuple.Create(   new Vector3(-645.84f, -419.67f, 34.1f),     new Vector3(-654.84f, -414.21f, 35.45f)),
            Tuple.Create(   new Vector3(-712.7f, -668.08f, 29.81f),     new Vector3(-714.58f, -675.37f, 30.63f)),
            Tuple.Create(   new Vector3(-648.24f, -681.53f, 30.61f),    new Vector3(-656.77f, -678.12f, 31.46f)),
            Tuple.Create(   new Vector3(-648.87f, -904.3f, 23.8f),      new Vector3(-660.88f, -900.72f, 24.61f)),
            Tuple.Create(   new Vector3(-529.01f, -848.03f, 29.26f),    new Vector3(-531.0f, -854.04f, 29.79f)),
        };

        public static List<Tuple<Vector3, Vector3>> DeliveryLocationsVan = new List<Tuple<Vector3, Vector3>>()
        {
            Tuple.Create(   new Vector3(-51.95f, -1761.67f, 28.89f),    new Vector3(-41.15f, -1751.66f, 29.42f)),
            Tuple.Create(   new Vector3(369.38f, 317.44f, 103.21f),     new Vector3(375.08f, 333.65f, 103.57f)),
            Tuple.Create(   new Vector3(-702.38f, -920.38f, 18.8f),     new Vector3(-705.7f, -905.46f, 19.22f)),
            Tuple.Create(   new Vector3(-1225.49f, -893.3f, 12.13f),    new Vector3(-1223.77f, -912.26f, 12.33f)),
            Tuple.Create(   new Vector3(-1506.82f, -383.06f, 40.64f),   new Vector3(-1482.29f, -378.95f, 40.16f)),
            Tuple.Create(   new Vector3(1149.13f, -985.08f, 45.64f),    new Vector3(1131.86f, -979.32f, 46.42f)),
            Tuple.Create(   new Vector3(1157.19f, -331.77f, 68.64f),    new Vector3(1163.79f, -314.6f, 69.21f)),
            Tuple.Create(   new Vector3(-1145.42f, -1590.97f, 4.06f),   new Vector3(-1150.31f, -1601.7f, 4.39f)),
            Tuple.Create(   new Vector3(48.18f, -992.62f, 29.03f),      new Vector3(38.41f, -1005.3f, 29.48f)),
            Tuple.Create(   new Vector3(370.05f, -1036.4f, 28.99f),     new Vector3(376.7f, -1028.82f, 29.34f)),
            Tuple.Create(   new Vector3(785.95f, -761.67f, 26.33f),     new Vector3(797.0f, -757.31f, 26.89f)),
            Tuple.Create(   new Vector3(41.53f, -138.21f, 55.08f),      new Vector3(50.96f, -135.49f, 55.2f)),
            Tuple.Create(   new Vector3(106.8f, 304.21f, 109.81f),      new Vector3(90.86f, 298.51f, 110.21f)),
        };

        public static List<Tuple<Vector3, Vector3>> DeliveryLocationsTruck = new List<Tuple<Vector3, Vector3>>()
        {
            
            Tuple.Create(   new Vector3(-1395.82f, -653.76f, 28.91f),   new Vector3(-1413.43f, -635.47f, 28.67f)),
            Tuple.Create(   new Vector3(164.18f, -1280.21f, 29.38f),    new Vector3(136.5f, -1278.69f, 29.36f)),
            Tuple.Create(   new Vector3(75.71f, 164.41f, 104.93f),      new Vector3(78.55f, 180.44f, 104.63f)),
            Tuple.Create(   new Vector3(-226.62f, 308.87f, 92.4f),      new Vector3(-229.54f, 293.59f, 92.19f)),
            Tuple.Create(   new Vector3(-365.87f, 297.27f, 85.04f),     new Vector3(-370.5f, 277.98f, 86.42f)),
            Tuple.Create(   new Vector3(-403.95f, 196.11f, 82.67f),     new Vector3(-395.17f, 208.6f, 83.59f)),
            Tuple.Create(   new Vector3(-412.26f, 297.95f, 83.46f),     new Vector3(-427.08f, 294.19f, 83.23f)),
            Tuple.Create(   new Vector3(-606.23f, -901.52f, 25.39f),    new Vector3(-592.48f, -892.76f, 25.93f)),
            Tuple.Create(   new Vector3(-837.03f, -1142.46f, 7.44f),    new Vector3(-841.89f, -1127.91f, 6.97f)),
            Tuple.Create(   new Vector3(-1061.56f, -1382.19f, 5.44f),   new Vector3(-1039.38f, -1396.88f, 5.55f)),
            Tuple.Create(   new Vector3(156.41f, -1651.21f, 29.53f),    new Vector3(169.11f, -1633.38f, 29.29f)),
            Tuple.Create(   new Vector3(168.13f, -1470.07f, 29.37f),    new Vector3(175.78f, -1461.16f, 29.24f)),
            Tuple.Create(   new Vector3(118.99f, -1486.21f, 29.38f),    new Vector3(143.54f, -1481.18f, 29.36f)),
            Tuple.Create(   new Vector3(-548.34f, 308.19f, 83.34f),     new Vector3(-546.6f, 291.46f, 83.02f)),
        };

        public static Dictionary<int, int> OutfitScooter_drawables = new Dictionary<int, int>()
        {
            [1]     = 0,
            [3]     = 66,
            [4]     = 97,
            [5]     = 0,
            [6]     = 32,
            [7]     = 0,
            [8]     = 15,
            [9]     = 0,
            [11]    = 184,
            [12]    = 18,
            [13]    = 1280,
        };

        public static Dictionary<int, int> OutfitScooter_drawableTextures = new Dictionary<int, int>()
        {
            [1]     = 0,
            [3]     = 0,
            [4]     = 3,
            [5]     = 0,
            [6]     = 14,
            [7]     = 0,
            [8]     = 0,
            [9]     = 0,
            [11]    = 0,
            [12]    = 5,
            [13]    = 2,

        };

        public static Dictionary<int, int> OutfitVan_drawables = new Dictionary<int, int>()
        {
            [1]     = 0,
            [3]     = 66,
            [4]     = 97,
            [5]     = 0,
            [6]     = 32,
            [7]     = 0,
            [8]     = 141,
            [9]     = 0,
            [11]    = 230,
            [12]    = 45,
            [13]    = 1280,
        };

        public static Dictionary<int, int> OutfitVan_drawableTextures = new Dictionary<int, int>()
        {
            [1]     = 0,
            [3]     = 0,
            [4]     = 3,
            [5]     = 0,
            [6]     = 14,
            [7]     = 0,
            [8]     = 0,
            [9]     = 0,
            [11]    = 3,
            [12]    = 7,
            [13]    = 2,

        };

        public static List<string> VAN_GOODS_PROP_NAMES = new List<string>()
        {
            "prop_crate_11e",
            "prop_cardbordbox_02a"
        };

    }
    
}
