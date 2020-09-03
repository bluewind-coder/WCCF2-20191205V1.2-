using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinFormChatClient
{
    
        
    class strWork
    {

        //定义协议字符串的长度
        static int PROTOCOL_LEN = 1;
        //下面是一些协议字符串，服务器和客户端交换的信息
        //都应该在前、后添加这种特殊字符串。
        static string USER_ROUND = "∏";
        static string MSG_ROUND = "§";
        static string TIME_ROUND = "γ";
        static string LOGIN_SUCCESS = "★";
        static string ALLMSG_ROUND = "☆";
        static string ISCONNECTED = "▲";
        static string ISONLINE = "∑";
        static string QUIT_SUCCESS = "※";

       //---------------------------function---------------

        public static string getSubStr(string str, string strBegin, string strEnd)
        {
            int beginIndex = str.IndexOf(strBegin);
            int endIndex = str.LastIndexOf(strEnd);
            int subLength = endIndex - beginIndex;
            return str.Substring(beginIndex + 1, subLength - 1);
        }

        public static string getRealMsg(string strReceive)
        {
            string str;
            str = getSubStr(strReceive, TIME_ROUND, TIME_ROUND);
            return str;
        }

        

        //判断是否是心跳包
        public static Boolean isOnline(String str)
        {
            if (str.Equals(ISONLINE))
                return true;
            else
                return false;
        }

        // 判断是否是聊天信息
        public static Boolean isChat(String str)
        {
            if (str.StartsWith(ALLMSG_ROUND) && str.EndsWith(ALLMSG_ROUND))
                return true;
            else
                return false;

        }

        // 判断是否是登录信息
        public static Boolean isLogin(String str)
        {
            if (str.StartsWith(LOGIN_SUCCESS) && str.EndsWith(LOGIN_SUCCESS))
                return true;
            else
                return false;

        }

        // 判断是否是退出信息
        public static Boolean isQuit(String str)
        {
            if (str.StartsWith(QUIT_SUCCESS) && str.EndsWith(QUIT_SUCCESS))
                return true;
            else
                return false;

        }
       
                        
    }
}
