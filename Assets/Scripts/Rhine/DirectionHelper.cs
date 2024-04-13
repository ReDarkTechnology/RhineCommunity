using UnityEngine;

namespace Rhine
{
    public static class DirectionHelper
    {
        public static Vector3[] directions = {
            new Vector3(-1, 0),
            new Vector3(0, -1),
            new Vector3(0, 1),
            new Vector3(1, 0),
            new Vector3(-0.70710678118654752f, 0.70710678118654752f),
            new Vector3(0.70710678118654752f, 0.70710678118654752f),
            new Vector3(0.70710678118654752f, -0.70710678118654752f),
            new Vector3(-0.70710678118654752f, -0.70710678118654752f),
            new Vector3(0, 1)
        };

        public static Vector3[] rotations = {
            new Vector3(0, 0, 90),
            new Vector3(0, 0, 180),
            new Vector3(0, 0, 0),
            new Vector3(0, 0, -90),
            new Vector3(0, 0, 45),
            new Vector3(0, 0, -45),
            new Vector3(0, 0, -135),
            new Vector3(0, 0, 135),
            new Vector3(0, 0, 0)
        };

        public static KeyCode ToKey(this string str)
        {
            return (KeyCode)System.Enum.Parse(typeof(KeyCode), str);
        }

        public static Vector3 ToRotation(this Direction dir)
        {
            return rotations[(int)dir];
        }

        public static bool IsDiagonal(this Direction dir)
        {
            int value = (int)dir;
            return value > 3 && value < 8;
        }
        public static bool IsNotDiagonal(this Direction dir) => ((int)dir) < 4;

        public static Vector3 ToDirection(this Direction dir)
        {
            return directions[(int)dir];
        }
    }
}