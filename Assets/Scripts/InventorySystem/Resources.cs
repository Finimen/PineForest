using System;

namespace Assets.Scripts
{
    [Serializable]
    public struct Resources
    {
        public int Food;
        public int Wood;
        public int Stone;

        public Resources(int food, int wood, int stone)
        {
            Food = food;
            Wood = wood;
            Stone = stone;
        }

        public static Resources operator -(Resources left, Resources right)
        {
            return new Resources(left.Food - right.Food, left.Wood - right.Wood, left.Stone - right.Stone);
        }

        public static Resources operator +(Resources left, Resources right)
        {
            return new Resources(left.Food + right.Food, left.Wood + right.Wood, left.Stone + right.Stone);
        }

        public static bool operator >=(Resources left, Resources right)
        {
            return (left.Food >= right.Food) & (left.Wood >= right.Wood) & (left.Stone >= right.Stone);
        }

        public static bool operator <=(Resources left, Resources right)
        {
            return (left.Food <= right.Food) & (left.Wood <= right.Wood) & (left.Stone <= right.Stone);
        }

        public static Resources operator *(Resources left, int right)
        {
            return new Resources(left.Food * right, left.Wood * right, left.Stone * right);
        }

        public static Resources operator -(Resources resources)
        {
            return new Resources( -resources.Food, - resources.Wood, -resources.Stone);
        }

        public override string ToString()
        {
            return
                $"Food: {Food}\n" +
                $"Wood: {Wood}\n" +
                $"Stone: {Stone}";
        }
    }
}