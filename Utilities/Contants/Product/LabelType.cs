
namespace Utilities.Contants
{
    public enum LabelType
    {
        amazon = 1,
        costco = 2,
        bestbuy = 3,
        nordstromrack = 4,
        hautelook = 5,
        sephora = 6,
        jomashop = 7,
        victoria_secret = 8

    }
    public struct LabelNameType
    {
        public const string amazon = "amazon";
        public const string costco = "costco";
        public const string bestbuy = "bestbuy";
        public const string nordstromrack = "nordstromrack";
        public const string jomashop = "jomashop";
        public const string hautelook = "hautelook";
        public const string sephora = "sephora";
        public const string victoria_secret = "victoria_secret";
        public static int GetLabelId(string label_name)
        {
            switch (label_name)
            {
                case LabelNameType.amazon:
                    return (int)LabelType.amazon;
                case LabelNameType.costco:
                    return (int)LabelType.costco;
                case LabelNameType.jomashop:
                    return (int)LabelType.jomashop;
            }

            return (int)LabelType.amazon;
        }

        public static string GetLabelName(int label_id)
        {
            switch (label_id)
            {
                case (int)LabelType.amazon:
                    return LabelType.amazon.ToString();
                case (int)LabelType.costco:
                    return LabelType.costco.ToString();
                case (int)LabelType.jomashop:
                    return LabelType.jomashop.ToString();
            }

            return LabelType.amazon.ToString();
        }
    }
}
