using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication018_Road_Ver1._0
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            tbR.Text = "0"; tbG.Text = "0"; tbB.Text = "0"; tbMin.Text = "0";
            pbColor.BackColor = Color.FromArgb(0, 0, 0);
        }
        int[] a = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string resultFile;
            OpenFileDialog dlgopen = new OpenFileDialog();
            dlgopen.InitialDirectory = "f:\\";
            dlgopen.Filter = "所有文件(*.*)|*.*|jpg格式(*.jpg)|*.jpg|bmp格式(*.bmp)|*.bmp|tif格式(*.tif)|*.tif";
            dlgopen.FilterIndex = 2;
            dlgopen.RestoreDirectory = true;
            if (dlgopen.ShowDialog() == DialogResult.OK)
            {
                resultFile = dlgopen.FileName;
                //tbRoute.Text = resultFile;
                string sPicPaht = dlgopen.FileName.ToString();
                Bitmap bmPic = new Bitmap(sPicPaht);
                Point ptLoction = new Point(bmPic.Size);
                if (ptLoction.X > pbOrigin.Size.Width || ptLoction.Y > pbOrigin.Size.Height)
                {
                    //图像框的停靠方式  
                    //pcbPic.Dock = DockStyle.Fill;  
                    //图像充满图像框，并且图像維持比例  
                    pbOrigin.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    //图像在图像框置中  
                    pbOrigin.SizeMode = PictureBoxSizeMode.CenterImage;
                }

                //LoadAsync：非同步载入图像  
                pbOrigin.LoadAsync(sPicPaht);
                Image pic = Image.FromFile(resultFile);//strFilePath是该图片的绝对路径
                int intWidth = pic.Width;//长度像素值
                int intHeight = pic.Height;//高度像素值
                lblSize.Text = intWidth + "×" + intHeight;

            }
        }

        private void btAnalysis_Click(object sender, EventArgs e)
        {
            try
            {
                TimeCounter.Start();
                //Image pic = Image.FromFile(resultFile);//strFilePath是该图片的绝对路径
                Image pic = pbOrigin.Image;
                int intWidth = pic.Width;//长度像素值
                int intHeight = pic.Height;//高度像素值

                Image Image_temp = BlackandWhiteEffect(pbOrigin);

                Bitmap bmPic = new Bitmap(Image_temp);
                Point ptLoction = new Point(Image_temp.Size);
                if (ptLoction.X > pbNew.Size.Width || ptLoction.Y > pbNew.Size.Height)
                {
                    //图像框的停靠方式  
                    //pcbPic.Dock = DockStyle.Fill;  
                    //图像充满图像框，并且图像維持比例  
                    pbNew.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    //图像在图像框置中  
                    pbNew.SizeMode = PictureBoxSizeMode.CenterImage;
                }
                //LoadAsync：非同步载入图像
                pbNew.Image = Image_temp;
                TimeCounter.Stop();
                lblTime.Text = string.Format("{0}秒", TimeCounter.Seconds.ToString());
                Process.Value = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("请选择图片！");
                //throw;
            }
        }
        public Image BlackandWhiteEffect(PictureBox Pict)
        {
            int Var_H = Pict.Image.Height;          //获取图象的高度  
            int Var_W = Pict.Image.Width;          //获取图象的宽度  
            Bitmap Var_bmp = new Bitmap(Var_W, Var_H);    //根据图象的大小实例化Bitmap类  
            Bitmap Var_SaveBmp = (Bitmap)Pict.Image;        //根据图象实例化Bitmap类  
                                                            //遍历图象的象素  
            for (int i = 0; i < Var_W; i++)
                for (int j = 0; j < Var_H; j++)
                {
                    Color tem_color = Var_SaveBmp.GetPixel(i, j);     //获取当前象素的颜色值
                    int tem_r, tem_g, tem_b, tem_Value = 0;       //定义变量 
                    tem_r = tem_color.R;          //获取R色值 
                    tem_g = tem_color.G;          //获取G色值 
                    tem_b = tem_color.B;          //获取B色值 
                    tem_Value = ((tem_r + tem_g + tem_b) / 3);      //用平均值法产生黑白图像
                    if (tem_Value > 220)
                    {
                        Var_bmp.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                    }
                    else
                    {
                        Var_bmp.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                    }
                    Process.Maximum = Var_W;
                    Process.Value = i;                    

                }
            return Var_bmp;
        }
        public class TimeCounter
        {
            private static long startCount = 0;
            private static long elapsedCount = 0;
            #region Properties  
            public static float Seconds
            {
                get
                {
                    long freq = 0;
                    float retValue = 0.0f;
                    QueryPerformanceFrequency(ref freq);
                    if (freq != 0)
                    {
                        retValue = (float)elapsedCount / (float)freq;
                    }
                    return retValue;
                }
            }
            #endregion
            #region Methods  
            public static void Start()
            {
                startCount = 0;
                QueryPerformanceCounter(ref startCount);
            }
            public static void Stop()
            {
                long stopCount = 0;
                QueryPerformanceCounter(ref stopCount);
                elapsedCount = (stopCount - startCount);
            }
            #endregion
            #region Import API  
            [DllImport("kernel32.dll")]
            private static extern bool QueryPerformanceCounter(
                ref long lpPerformanceCount);
            [DllImport("kernel32.dll")]
            private static extern bool QueryPerformanceFrequency(
                ref long lpFrequency);

            #endregion
        }

        private void btConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                int color_R, color_G, color_B, Temp_Min;
                color_R = int.Parse(tbR.Text);
                color_G = int.Parse(tbG.Text);
                color_B = int.Parse(tbB.Text);
                Temp_Min = int.Parse(tbMin.Text);
                pbColor.BackColor = Color.FromArgb(color_R, color_G, color_B);
            }
            catch
            {
                MessageBox.Show("请输入 0~255 之间的数字！");
                //throw;
            }
        }

        private void btGet_Click(object sender, EventArgs e)
        {
            try
            {
                TimeCounter.Start();
                int Var_H = pbOrigin.Image.Height;          //获取图象的高度  
                int Var_W = pbOrigin.Image.Width;          //获取图象的宽度  
                Bitmap Var_bmp = new Bitmap(Var_W, Var_H);    //根据图象的大小实例化Bitmap类  
                Bitmap Var_SaveBmp = (Bitmap)pbOrigin.Image;        //根据图象实例化Bitmap类  
                                                                    //遍历图象的象素  
                for (int i = 0; i < Var_W; i++)
                    for (int j = 0; j < Var_H; j++)
                    {
                        Color tem_color = Var_SaveBmp.GetPixel(i, j);     //获取当前象素的颜色值
                        int tem_r, tem_g, tem_b, tem_Value = 0;       //定义变量 
                        int tem_R, tem_G, tem_B;       //定义变量
                        tem_r = tem_color.R;          //获取R色值 
                        tem_g = tem_color.G;          //获取G色值 
                        tem_b = tem_color.B;          //获取B色值 
                        tem_Value = ((tem_r + tem_g + tem_b) / 3);      //用平均值法产生黑白图像
                        tem_R = int.Parse(tbR.Text);
                        tem_G = int.Parse(tbG.Text);
                        tem_B = int.Parse(tbB.Text);
                        int tem_Rr = tem_R - tem_r;
                        int tem_Gg = tem_G - tem_g;
                        int tem_Bb = tem_B - tem_b;
                        int tem_Value2 = ((tem_Rr + tem_Gg + tem_Bb) / 3);
                        int ErMin = int.Parse(tbMin.Text);
                        if (tem_Rr < ErMin && tem_Gg < ErMin && tem_Bb < ErMin)
                        {
                            Var_bmp.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                        }
                        else
                        {
                            Var_bmp.SetPixel(i, j, Color.FromArgb(tem_r, tem_g, tem_b));
                        }
                        Process.Maximum = Var_W;
                        Process.Value = i;
                        if (Process.Value >= Var_W)
                        {
                            Process.Value = 0;
                        }
                    }
                pbNew.Image = Var_bmp;
                TimeCounter.Stop();
                Process.Value = 0;
                lblTime.Text = string.Format("{0}秒", TimeCounter.Seconds.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("请选择图片！");
                //throw;
            }
        }

        private void btError_Click(object sender, EventArgs e)
        {
            try
            {
                TimeCounter.Start();
                int Var_H = pbNew.Image.Height;
                int Var_W = pbNew.Image.Width;
                Image Image_temp = Error_Size(Var_W, Var_H);
                pbNew.Image = Image_temp;
                Process.Value = 0;
                TimeCounter.Stop();
                lblTime.Text = string.Format("{0}秒", TimeCounter.Seconds.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("请先处理图片！");
                //throw;
            }
            
        }
        public Image Error_Size(int Var_W, int Var_H)
        {
            Bitmap Var_bmp = new Bitmap(Var_W, Var_H);
            Bitmap Var_SaveBmp = (Bitmap)pbNew.Image;
            
            for (int i = 3; i < Var_W - 3; i++) 
            {
                for (int j = 3; j < Var_H - 3; j++) 
                {
                    Color tem_color = Var_SaveBmp.GetPixel(i, j);     //获取当前象素的颜色值
                    int tem_r, tem_g, tem_b, tem_Value = 0;       //定义变量 
                    tem_r = tem_color.R;          //获取R色值 
                    tem_g = tem_color.G;          //获取G色值 
                    tem_b = tem_color.B;          //获取B色值 
                    tem_Value = (tem_r + tem_g + tem_b) / 3;

                    tem_color = Var_SaveBmp.GetPixel(i, j - 1);     //获取当前象素的颜色值
                    int tem_Value1 = 0;       //定义变量 
                    tem_r = tem_color.R;          //获取R色值 
                    tem_g = tem_color.G;          //获取G色值 
                    tem_b = tem_color.B;          //获取B色值 
                    tem_Value1 = (tem_r + tem_g + tem_b) / 3;
                    tem_color = Var_SaveBmp.GetPixel(i, j + 1);     //获取当前象素的颜色值
                    int tem_Value2 = 0;       //定义变量 
                    tem_r = tem_color.R;          //获取R色值 
                    tem_g = tem_color.G;          //获取G色值 
                    tem_b = tem_color.B;          //获取B色值 
                    tem_Value2 = (tem_r + tem_g + tem_b) / 3;
                    tem_color = Var_SaveBmp.GetPixel(i - 1, j);     //获取当前象素的颜色值
                    int tem_Value3 = 0;       //定义变量 
                    tem_r = tem_color.R;          //获取R色值 
                    tem_g = tem_color.G;          //获取G色值 
                    tem_b = tem_color.B;          //获取B色值 
                    tem_Value3 = (tem_r + tem_g + tem_b) / 3;
                    tem_color = Var_SaveBmp.GetPixel(i+1, j);     //获取当前象素的颜色值
                    int tem_Value4 = 0;       //定义变量 
                    tem_r = tem_color.R;          //获取R色值 
                    tem_g = tem_color.G;          //获取G色值 
                    tem_b = tem_color.B;          //获取B色值 
                    tem_Value4 = (tem_r + tem_g + tem_b) / 3;
                    int tem_Value5 = tem_Value1 + tem_Value2 + tem_Value3 + tem_Value4;
                    if (tem_Value5 / 4 < 10) 
                    {
                        Var_bmp.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                    }
                    else
                    {
                        Var_bmp.SetPixel(i, j, Color.FromArgb(tem_Value5 / 4, tem_Value5 / 4, tem_Value5 / 4));
                        //Var_bmp.SetPixel(i, j, Color.FromArgb(255,254,254,254));
                    }
                    tem_Value5 = 0;
                }
                Process.Maximum = Var_W;
                Process.Value = i;
            }
            return Var_bmp;
        }

        private void GetLegth_Click(object sender, EventArgs e)
        {
            try
            {
                //this.Cursor = System.Windows.Forms.Cursors.Cross;  
                Image pic = pbNew.Image;
                int intWidth = pic.Width;//长度像素值
                int intHeight = pic.Height;//高度像素值         
                int legth = get_legth();
                if (legth != 0)
                {
                    //lblValue.Text = legth.ToString();
                }
                else
                {
                    //lblValue.Text = "不存在！";
                }   
            }
            catch (Exception)
            {
                MessageBox.Show("图片什么的不存在的啦~~~","Warnning",(MessageBoxButtons)MessageBoxDefaultButton.Button1,MessageBoxIcon.Information);
                //throw;
            }
                
        }
        public int get_legth()
        {
            int distance = 0; int legthi = 0, legthj =0;
            int Var_H = pbNew.Image.Height;          //获取图象的高度  
            int Var_W = pbNew.Image.Width;          //获取图象的宽度  
            Bitmap Var_bmp = new Bitmap(Var_W, Var_H);    //根据图象的大小实例化Bitmap类  
            Bitmap Var_SaveBmp = (Bitmap)pbOrigin.Image;
            for (int i = 0; i < Var_W; i++)
            {
                for (int j = 0; j < Var_H; j++)
                {
                    Color tem_color = Var_SaveBmp.GetPixel(i, j);     //获取当前象素的颜色值
                    int tem_r, tem_g, tem_b, tem_Value = 0;       //定义变量 
                    tem_r = tem_color.R;          //获取R色值 
                    tem_g = tem_color.G;          //获取G色值 
                    tem_b = tem_color.B;          //获取B色值 
                    tem_Value = (tem_r + tem_g + tem_b) / 3;
                    if (tem_Value <= 120)
                    {
                        legthi = i;
                        legthj = j;
                        i = Var_W; j = Var_H;
                    }         
                }
            }
            for (int m = legthi; m < Var_W; m++)
            {
                Color tem_color = Var_SaveBmp.GetPixel(m, legthj);     //获取当前象素的颜色值
                int tem_r, tem_g, tem_b, tem_Value = 0;       //定义变量 
                tem_r = tem_color.R;          //获取R色值 
                tem_g = tem_color.G;          //获取G色值 
                tem_b = tem_color.B;          //获取B色值 
                tem_Value = (tem_r + tem_g + tem_b) / 3;
                if (tem_Value >= 230)
                {                    
                    int legth = m - legthi;
                    lblValue.Text = legth.ToString();
                    m = Var_W;
                }
            }
            return distance;
        }

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) { this.Cursor = System.Windows.Forms.Cursors.Arrow; }

        }

        private void 软件信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("作者：奶茶");
            //MessageBoxIcon.Information.ToString();
            //MessageBox.Show("版权所有");
            
        }

        private void SaveFile_Click(object sender, EventArgs e)
        {
            string resultFile;
            SaveFileDialog dlgsave = new SaveFileDialog();
            dlgsave.InitialDirectory = "f:\\";
            dlgsave.Filter = "所有文件(*.*)|*.*|jpg格式(*.jpg)|*.jpg";
            dlgsave.FilterIndex = 2;
            dlgsave.RestoreDirectory = true;
            if (dlgsave .ShowDialog()==DialogResult.OK )
            {
                resultFile = dlgsave.FileName ;
                pbNew.Image.Save(resultFile);
            }
        }
    }    
}

