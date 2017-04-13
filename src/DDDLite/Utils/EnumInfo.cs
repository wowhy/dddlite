namespace DDDLite.Utils
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    public class EnumInfo
    {
        private Type _enumType = null;
        private bool _flag = false;
        private Dictionary<int, string> _items = new Dictionary<int, string>();

        public EnumInfo(Type enumType)
        {
            this._enumType = enumType;
            if (_enumType.IsDefined(typeof(FlagsAttribute), false))
            {
                _flag = true;
            }
            // 遍历枚举值和名称
            foreach (var item in Enum.GetValues(_enumType))
            {
                FieldInfo fi = _enumType.GetField(item.ToString());
                int key = int.Parse(Convert.ChangeType(item, ((Enum)item).GetTypeCode()).ToString());
                string value = item.ToString();
                if (fi.IsDefined(typeof(DescriptionAttribute)))
                {
                    DescriptionAttribute da = fi.GetCustomAttribute<DescriptionAttribute>();
                    value = da.Description;
                }
                this._items.Add(key, value);
            }
        }

        /// <summary>
        /// 枚举类型。
        /// </summary>
        public Type EnumType { get { return _enumType; } }

        /// <summary>
        /// 是否是位值枚举
        /// </summary>
        public bool Flag { get { return _flag; } }

        /// <summary>
        /// 枚举值集合。
        /// 整数枚举值以及枚举的展示名称
        /// </summary>
        public Dictionary<int, string> Items { get { return _items; } }

        /// <summary>
        /// 获取枚举值名称。自动处理位值枚举的复选值。
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <returns>枚举值名称</returns>
        public string GetText(int value)
        {
            var text = string.Empty;
            if (this.Flag)
            {
                if (value == 0)
                {
                    return this._items.ContainsKey(value) ? this._items[value] : string.Empty;
                }

                if (value == 0) {
                    if (this._items.ContainsKey(value)) {
                        text = this._items[value];
                    }
                } 
                else {
                    var items = this._items.Where(k => k.Key != 0 && (k.Key & value) == k.Key).Select(k => k.Value);
                    text = string.Join(";", items);
                }
            }
            else
            {
                if (this._items.ContainsKey(value))
                {
                    text = this._items[value];
                }
            }

            return text;
        }
    }
}