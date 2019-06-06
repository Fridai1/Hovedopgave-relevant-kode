using UnityEngine;
namespace Assets.Scripts.AI
{
    public static class BlackBoard
    {
        public static bool IsCarRdy { get; set; }

        public static Vector3 PlayerLastCords { get; set; }

        public static GameObject Car { get; set; }


    }
}