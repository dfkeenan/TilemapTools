using System;

namespace TilemapTools.Tiled
{
    public class Property
    {
        public string Name { get; set; }

        public PropertyType Type { get; set; }

        public string RawValue { get; set; }

        public object Value
        {
            get
            {
                switch (Type)
                {
                    case PropertyType.String:
                        return RawValue;
                    case PropertyType.Int:
                        return Convert.ToInt32(RawValue);
                    case PropertyType.Float:
                        return Convert.ToSingle(RawValue);
                    case PropertyType.Bool:
                        return Convert.ToBoolean(RawValue);
                    default:
                        return RawValue;
                }
            }
        }
    }
}