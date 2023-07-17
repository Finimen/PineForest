using System;

namespace Assets.Scripts
{
    [Serializable]
    public struct Resources
    {
        public int Villagers;
        public int Wood;
        public int Stone;

        public Resources(int villagers, int wood, int stone)
        {
            Villagers = villagers;
            Wood = wood;
            Stone = stone;
        }

        public static Resources operator -(Resources left, Resources right)
        {
            return new Resources(left.Villagers - right.Villagers, left.Wood - right.Wood, left.Stone - right.Stone);
        }

        public static Resources operator +(Resources left, Resources right)
        {
            return new Resources(left.Villagers + right.Villagers, left.Wood + right.Wood, left.Stone + right.Stone);
        }

        public static bool operator >=(Resources left, Resources right)
        {
            return (left.Villagers >= right.Villagers) & (left.Wood >= right.Wood) & (left.Stone >= right.Stone);
        }

        public static bool operator <=(Resources left, Resources right)
        {
            return (left.Villagers <= right.Villagers) & (left.Wood <= right.Wood) & (left.Stone <= right.Stone);
        }

        public static Resources operator *(Resources left, int right)
        {
            return new Resources(left.Villagers * right, left.Wood * right, left.Stone * right);
        }

        public static Resources operator -(Resources resources)
        {
            return new Resources(-resources.Villagers, -resources.Wood, -resources.Stone);
        }

        public override string ToString()
        {
            return $"Villagers: {Villagers}\n" +
                $"Wood: {Wood}\n" +
                $"Stone: {Stone}";
        }
    }
}