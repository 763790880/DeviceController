namespace Opcua
{
    public class OpcUaStatusEventArgs
    {


        /// <summary>
        /// 是否异常
        /// </summary>
        public bool Error { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 转化为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Error ? "[异常]" : "[正常]" + Time.ToString("  yyyy-MM-dd HH:mm:ss  ") + Text;
        }


    }
}
