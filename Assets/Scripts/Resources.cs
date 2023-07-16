using System;

namespace Assets.Scripts
{
    [Serializable]
    public struct Resources
    {
        public int Wood;
        public int Stone;

        public Resources(int wood, int stone)
        {
            Wood = wood;
            Stone = stone;
        }

        public static Resources operator -(Resources left, Resources right)
        {
            return new Resources(left.Wood - right.Wood, left.Stone - right.Stone);
        }

        public static Resources operator +(Resources left, Resources right)
        {
            return new Resources(left.Wood + right.Wood, left.Stone + right.Stone);
        }

        public static bool operator >=(Resources left, Resources right)
        {
            return (left.Wood >= right.Wood) & (left.Stone >= right.Stone);
        }

        public static bool operator <=(Resources left, Resources right)
        {
            return (left.Wood <= right.Wood) & (left.Stone <= right.Stone);
        }

        public override string ToString()
        {
            return $"Wood: {Wood}\n" +
                $"Stone: {Stone}";
        }
    }
}