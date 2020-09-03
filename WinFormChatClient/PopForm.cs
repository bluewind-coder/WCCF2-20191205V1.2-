using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WinFormChatClient
{
    
    public partial class PopForm : Form
    {
        public static bool isLoad = false; //默认标记窗口未加载
        private int currentX;//横坐标        
        private int currentY;//纵坐标
        private int screenHeight;//屏幕高度
        private int screenWidth;//屏幕宽度
        int AW_ACTIVE = 0x20000; //激活窗口，在使用了AW_HIDE标志后不要使用这个标志
        int AW_HIDE = 0x10000;//隐藏窗口
        int AW_BLEND = 0x80000;// 使用淡入淡出效果
        int AW_SLIDE = 0x40000;//使用滑动类型动画效果，默认为滚动动画类型，当使用AW_CENTER标志时，这个标志就被忽略
        int AW_CENTER = 0x0010;//若使用了AW_HIDE标志，则使窗口向内重叠；否则向外扩展
        int AW_HOR_POSITIVE = 0x0001;//自左向右显示窗口，该标志可以在滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志
        int AW_HOR_NEGATIVE = 0x0002;//自右向左显示窗口，该标志可以在滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志
        int AW_VER_POSITIVE = 0x0004;//自顶向下显示窗口，该标志可以在滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志     
        int AW_VER_NEGATIVE = 0x0008;//自下向上显示窗口，该标志可以在滚动动画和滑动动画中使用。使用AW_CENTER标志时忽略该标志
        [DllImport("user32.dll")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dateTime, int dwFlags);//hwnd窗口句柄.dateTime:动画时长.dwFlags:动画类型组合  


        public PopForm()
        {
            InitializeComponent();
        }

        private void PopForm_Load(object sender, EventArgs e)
        {
            
            Rectangle rect = Screen.PrimaryScreen.WorkingArea;
            screenHeight = rect.Height;
            screenWidth = rect.Width;
            currentX = screenWidth - this.Width;
            currentY = screenHeight - this.Height;
            this.Location = new System.Drawing.Point(currentX, currentY);
            AnimateWindow(this.Handle, 1000, AW_ACTIVE | AW_SLIDE | AW_VER_NEGATIVE);//前端显示|滑动|至下向上
            isLoad = true; //窗口已加载
        }

        private void PopForm_Click(object sender, EventArgs e)
        {
            //this.Close();
            this.Hide();
        }

        private void PopForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            isLoad = false;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("iexplore.exe", "http://www.qidian.com/Default.aspx");
            //this.Close();
            this.Hide();
        }
    }
}
