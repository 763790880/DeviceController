using Opc.Ua;

namespace Opcua
{
    public class OpcNodeAttribute
    {
        /// <summary>
        /// 属性的名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 属性的类型描述
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 操作结果状态描述
        /// </summary>
        public StatusCode StatusCode { get; set; }
        /// <summary>
        /// 属性的值，如果读取错误，返回文本描述
        /// </summary>
        public object Value { get; set; }

    }
}
