

namespace Solution.Core.Domain.NpgSqlEntities.Common
{
    public class Dictionary:BaseEntity
    {
        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; } = null!;
        /// <summary>
        /// 值 
        /// </summary>
        public string Value { get; set; } = null!;
        /// <summary>
        /// 描述
        /// </summary>
        public string?Description { get; set; }
        /// <summary>
        /// 类型 0为创建类型 
        /// </summary>
        public int Type { get; set; }
        private Dictionary()
        {

        }
        public Dictionary(string key, string value, string? description, int type)
        {
            this.Key = key;
            this.Value = value;
            this.Description = description;
            this.Type = type;
        }
        public void ChangeValue(string value)
        {
            this.Value = value;
        }
        public void ChangeDescription(string description)
        {
            this.Description = description;
        }
        public void ChangeType(int type)
        {
            this.Type = type;
        }
    }
}
