using System;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public struct Resources
    {
        [field: SerializeField] public int Food { get; private set; }
        [field: SerializeField] public int Wood { get; private set; }
        [field: SerializeField] public int Stone { get; private set; }

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

        public static bool operator ==(Resources left, Resources right)
        {
            return (left.Food == right.Food) & (left.Wood == right.Wood) & (left.Stone == right.Stone);
        }

        public static bool operator !=(Resources left, Resources right)
        {
            return (left.Food != right.Food) | (left.Wood != right.Wood) | (left.Stone != right.Stone);
        }

        public static bool operator >(Resources left, Resources right)
        {
            return (left.Food > right.Food) & (left.Wood > right.Wood) & (left.Stone > right.Stone);
        }

        public static bool operator <(Resources left, Resources right)
        {
            return (left.Food < right.Food) & (left.Wood < right.Wood) & (left.Stone < right.Stone);
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

        public static Resources Empty
        {
            get
            {
                return new Resources(0, 0, 0);
            }
        }

        public static bool ConstainsGlobal(Resources a, Resources b)
        {
            return a.Food > b.Food | a.Wood > b.Wood | a.Stone > b.Stone;
        }

        public int TotalCount()
        {
            return Food + Wood + Stone;
        }

        public bool Contains(Resources other)
        {
            return (Food > 0 & other.Food > 0) | (Wood > 0 & other.Wood > 0) | (Stone > 0 & other.Stone > 0);
        }

        public bool ContainsInversed(Resources other)
        {
            return  other.Food > Food | other.Wood > Wood | other.Stone > Stone;
        }

        public Resources GetClampedResources(Resources needed, int maxCount)
        {
            var totalCount = 0;
            var result = new Resources();
            
            var food = Mathf.Clamp(Food, 0, Mathf.Min(needed.Food, maxCount - totalCount));
            totalCount += food;
            result = new Resources(food, result.Wood, result.Stone);

            var wood = Mathf.Clamp(Wood, 0, Mathf.Min(needed.Wood, maxCount - totalCount));
            totalCount += wood;
            result = new Resources(result.Food, wood, result.Stone);

            var stone = Mathf.Clamp(Stone, 0, Mathf.Min(needed.Wood, maxCount - totalCount));
            totalCount += stone;
            result = new Resources(result.Food, result.Wood, stone);

            this -= result;

            return result;
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