using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;

namespace Suntek.Utils
{
    public class SendMTBrandname
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //get brandname
        //get temp

        public static int SentMTMessage(string message, string phone, string commandCode, string cpNumber, string requestId)
        {
            string messageSent = EncodeTo64(message);
            string phoneSent = phone.StartsWith("+") ? phone.Replace("+", "") : phone;
            if (phoneSent.StartsWith("0"))
            {
                string test = phoneSent.Substring(1);
                phoneSent = string.Concat("84", test);
            }
            if (!phoneSent.StartsWith("84"))
            {
                phoneSent = string.Concat("84", phoneSent);
            }
            int sendMessageResult = -1;
            try
            {
                SendMsgReceiver smsSend = new SendMsgReceiver();
                smsSend.UserName = "tekasms";
                smsSend.Password = "tekasms123456";
                smsSend.PreAuthenticate = true;
                sendMessageResult = smsSend.sendMT(phoneSent, messageSent, cpNumber, commandCode, "1", requestId, "1", "1", "0", "0");

                return sendMessageResult;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return sendMessageResult;
            }
        }
        public static int SentMTMessage(string message, string phone)
        {
            string messageSent = EncodeTo64(message);
            string phoneSent = phone.StartsWith("+") ? phone.Replace("+", "") : phone;
            if (phoneSent.StartsWith("0"))
            {
                string test = phoneSent.Substring(1);
                phoneSent = string.Concat("84", test);
            }
            if (!phoneSent.StartsWith("84"))
            {
                phoneSent = string.Concat("84", phoneSent);
            }
            int sendMessageResult = -1;
            try
            {
                SendMsgReceiver smsSend = new SendMsgReceiver();
                smsSend.UserName = "vinhbluesea";
                smsSend.Password = "vinhbluesea#####&*(#";
                smsSend.PreAuthenticate = true;
                sendMessageResult = smsSend.sendMT(phoneSent, messageSent, "Format VN", "Format VN", "0", "0", "0", "0", "0", "0");
                logger.Info("test sendMTBrandname: " + phone);
                return sendMessageResult;
            }
            catch (Exception ex)
            {
                logger.Info("ex sendMTBrandname: " + ex);
                return sendMessageResult;
            }
        }
        private static string EncodeTo64(string toEncode)
        {

            byte[] toEncodeAsBytes

                  = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);

            string returnValue

                  = System.Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;

        }
    }
}