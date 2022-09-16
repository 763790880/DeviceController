using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constants
{
    public class IotDeviceAPI
    {
        #region 消息 Send And Receive Topic
        public const string MessagesSend = "halo/devices/{0}/sys/messages/up";
        public const string MessagesDown = "halo/devices/{0}/sys/messages/down";
        #endregion

        #region
        public const string IotMessagesSend = "halo/devices/sys/messages/up";
        public const string IotMessagesDown = "halo/devices/sys/messages/down";
        public const string IotEventUp = "halo/devices/sys/events/up";
        public const string IotEventDown = "halo/devices/sys/events/down";
        #endregion

        #region Event Send and Receive Topic
        public const string EventUp = "halo/devices/{0}/sys/events/up";
        public const string EventDown = "halo/devices/{0}/sys/events/down";
        public const string EventServiceId = "file_manager";

        //File Upload or Download Event Type
        public const string FileUploadET = "get_upload_url";
        public const string FileUploadResponseET = "get_upload_url_response";
        public const string FileDownloadET = "get_download_url";
        public const string FileDownResponseET = "get_download_url_response";
        public const string FileUpDownResultReportET = "upload_result_report";
        #endregion

        #region Commands Send and Receive Topic
        public const string CommandsUp = "halo/devices/{0}/sys/commands/response/request_id=";
        public const string CommandsDown = "halo/devices/{0}/sys/commands/#";
        #endregion
    }
}
