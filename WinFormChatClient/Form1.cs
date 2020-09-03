using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using System.Timers;
namespace WinFormChatClient
{
           
    
    public partial class Form1 : Form
    {
        //定义协议字符串的长度
        public int PROTOCOL_LEN = 1;
        //下面是一些协议字符串，服务器和客户端交换的信息
        //都应该在前、后添加这种特殊字符串。
        String USER_ROUND = "∏";
        String MSG_ROUND = "§";
        String TIME_ROUND = "γ";
        String LOGIN_SUCCESS = "★";
        String ISONLINE = "∑";
        String ALLMSG_ROUND = "☆";
        String CONNECT_COUNT = "▲";
        String QUIT_SUCCESS = "※";
         

        string strHomeIP = "192.168.1.23", 
               strOfficeIP = "192.168.0.101",
               strCenterIP = "192.168.0.108", 
               strInternetIP = "124.228.60.173",
               strHomeServer = "192.168.31.2";
        //IPAddress myip = IPAddress.Parse("59.51.124.44");
        IPAddress myip = IPAddress.Parse("192.168.31.2");
        IPEndPoint myserver;
        TcpClient tcp;
        NetworkStream ns;
        Thread thread;
        string userName;
        int size=0; //当前rtb行的字符个数
        int currentPostion = 0; //rtb当前位置

        bool hide = false;
        
        PopForm popForm;

        bool isClosed = false; //是否已关闭资源

        public Form1(string strLoginUser)
        {
            InitializeComponent();
            Form.CheckForIllegalCrossThreadCalls = false;
            popForm = new PopForm();    //提示窗口已创建，等待显示
            popForm.TopMost = true;
            
            HotKey.RegisterHotKey(this.Handle, 100, 0, Keys.F1);
            HotKey.RegisterHotKey(this.Handle, 200, 0, Keys.F2);
            HotKey.RegisterHotKey(this.Handle, 600, 0, Keys.F6);

            
            myserver = new IPEndPoint(myip, 5800);
            userName = strLoginUser;
            
        }

        private void login(string strUser)
        {
            tcp = new TcpClient();
            try
            {
                tcp.Connect(myserver);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            ns = tcp.GetStream();

            thread = new Thread(new ThreadStart(tcpWork));
            //后台线程
            //thread.IsBackground = true;

            thread.Start();
            
            send(USER_ROUND + strUser + USER_ROUND); //登录的同时发送用户名报头给服务器
        }

        private void send(string str)
        {
            byte[] sendBytes = new byte[1024];
            str += "\n";        //JAVA的服务器用的是ReadLine(),消息尾端要加换行符 "\n";
            sendBytes = System.Text.Encoding.Default.GetBytes(str);
            ns.Write(sendBytes, 0, sendBytes.Length);
        }


        /// <summary>
        /// 处理快捷键消息
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            //按快捷键   
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 100:   //F1
                            appClose(); 
                            break;
                    }
                    switch (m.WParam.ToInt32())
                    {
                        case 200:   //F2
                            this.Hide();
                            hide = true;    //主窗口已隐藏
                            break;
                    }

                    switch (m.WParam.ToInt32())
                    {
                        case 600:   //F6
                            this.Show();
                            popForm.Hide();
                            hide = false;   //主窗口已显示
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// 线程函数:接收服务器的消息
        /// </summary>
        public void tcpWork()
        {   string strReceive;
           
            StreamReader sr = new StreamReader(ns);
            try
            {
                while ((strReceive = sr.ReadLine()) != null)
                {
                    currentPostion = rtb.TextLength;//当前RTB控件的字符位置
                    
                    //如果是可用信息
                    if (strWork.isLogin(strReceive))//登录信息
                    {
                        string strUser = strWork.getSubStr(strReceive, USER_ROUND, USER_ROUND);
                        string strTime = strWork.getSubStr(strReceive, TIME_ROUND, TIME_ROUND);
                        string strCount = strWork.getSubStr(strReceive, CONNECT_COUNT, CONNECT_COUNT);
                        string strShow = strUser + "进入社保查询平台! " + "现有在线人数：" + strCount + " 时间:" + strTime;
                        rtb.AppendText(strShow + '\n');
                    }

                    if (strWork.isChat(strReceive))//聊天信息
                    {
                        string strUser = strWork.getSubStr(strReceive, USER_ROUND, USER_ROUND);
                        string strMsg = strWork.getSubStr(strReceive, MSG_ROUND, MSG_ROUND);
                        string strTime = strWork.getSubStr(strReceive, TIME_ROUND, TIME_ROUND);
                       

                        string userLine = strUser + "    " + strTime;
                        rtb.AppendText(userLine + '\n');
                        rtb.Select(currentPostion, userLine.Length);
                        if (strUser.Equals("blue"))
                        {
                            rtb.SelectionColor = Color.Blue;
                        }
                        else
                        {
                            rtb.SelectionColor = Color.Magenta;
                        }
                        currentPostion = rtb.TextLength;
                        rtb.AppendText(strMsg + '\n');
                        currentPostion = rtb.TextLength;

                    }

                    if (strWork.isQuit(strReceive))//退出信息
                    {
                        string strUser = strWork.getSubStr(strReceive, USER_ROUND, USER_ROUND);
                        string strTime = strWork.getSubStr(strReceive, TIME_ROUND, TIME_ROUND);
                        rtb.AppendText(strUser + "退出社保查询平台!" + "时间:" + strTime + '\n');

                    }
                    
                    //如果是心跳包
                    if (strWork.isOnline(strReceive))
                    {
                        continue;//啥都不做,继续下个循环
                        
                    }
                    
                    if(hide == true )
                    //当前聊天窗口已隐藏时弹出提示窗口
                    {
                        GetActiveWindowMehtod();
                    }

                    
                }//while
                
            }//try
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message+"tctWork()");
                //Abort关闭线程会出现异常
            }

        }
        

        private void btnSend_Click(object sender, EventArgs e)
        {
            send(MSG_ROUND + txtBoxSend.Text + MSG_ROUND); //加入信息报头
            txtBoxSend.Clear();  //清空发送文本框
           
        }

        //退出之前关闭所有资源
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            appClose();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            rtb.Select(rtb.Text.Length, 0);
            rtb.ScrollToCaret();
        }

        private void txtBoxSend_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyValue == 13)
            {
                e.SuppressKeyPress = true;//回车事件已经处理完不再响应了
                send(MSG_ROUND + txtBoxSend.Text + MSG_ROUND); //加入信息报头
                txtBoxSend.Clear();  //清空发送文本框
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            login(userName);
            //txtBoxSend.Focus(); //获得输入焦点
        }

        public void Disply(ref RichTextBox rtb, string strInput, Color fontColor)
        {
            int p1 = rtb.TextLength;  //取出未添加时的字符串长度。 
            rtb.AppendText(strInput + "\n");  //保留每行的所有颜色。 //  rtb.Text += strInput + "\n";  //添加时，仅当前行有颜色。 
            int p2 = strInput.Length;  //取出要添加的文本的长度 
            rtb.Select(p1, p2);        //选中要添加的文本 
            rtb.SelectionColor = fontColor;  //设置要添加的文本的字体色 
            //rtb.Refresh(); 
        }

        void GetActiveWindowMehtod()
        {
            //在拥有主窗体的线程上创建子窗体(弹出窗口）
            this.Invoke(new GetActiveWindowHandle(GetActiveWindow));
        }

        delegate void GetActiveWindowHandle();
        void GetActiveWindow()
        {
            //在这里写加载新窗体的代码
            if(PopForm.isLoad==false)
            {
                popForm=new PopForm();
                popForm.TopMost = true;
            }
            popForm.Show();//加载窗口
           
        }

        private void Form1_Activated(object sender, EventArgs e)
        {//如果聊天窗口已被激活，弹出窗口隐藏

            if (popForm != null)
            { 
                popForm.Hide();
            }
            txtBoxSend.Focus(); //获得输入焦点
        }

        private void appClose()
        {
            if (isClosed == false) //如果没有关闭资源
            {
                try
                {
                    send(QUIT_SUCCESS);
                    
                    ns.Close();
                    tcp.Close();
                    thread.Abort();
                                     
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "appClose()");
                }
                isClosed = true;
            }
            Application.Exit();

        }

       

    }
}
