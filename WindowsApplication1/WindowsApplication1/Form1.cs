using System;
using System.Windows.Forms;
using System.Globalization;
using System.Media;
using System.IO;

namespace WindowsApplication1
{
    public partial class Form1 : Form
    {
        int n;
        int counter;
        bool status;
        int c_pulse;
        public Form1()
        {
            InitializeComponent();
            timer1 = new System.Windows.Forms.Timer();
            timer1.Interval = 1000;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Enabled = false;
            timer4 = new System.Windows.Forms.Timer();
            timer4.Interval = 1000;
            timer4.Tick += new EventHandler(timer4_Tick);
            timer2.Tick += new EventHandler(timer2_Tick);
            n = 0;
        }

        void timer2_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            DateTimeFormatInfo dtfi = DateTimeFormatInfo.InvariantInfo;
            textBox2.Text = dt.ToString("F", dtfi);
            //throw new Exception("The method or operation is not implemented.");
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            //build table
            int ccm=0;
            int cch=8;
            
            try
            {   //if(!File.Exists(textBox1.Text))
                // {MessageBox.Show("ملف الصوت غير موجود"); return; }

                if (int.Parse(comboBox4.Text) > 45 || int.Parse(comboBox3.Text) > 10 || int.Parse(comboBox2.Text) > 30 || int.Parse(comboBox5.Text) >20)
                { MessageBox.Show("قيم كبيرة"); return; }
                if (int.Parse(comboBox1.Text) > int.Parse(comboBox3.Text))
                { MessageBox.Show("خطأ في موعد الفرصة"); return; }
                if (int.Parse(comboBox4.Text) <= 0 || int.Parse(comboBox3.Text) <= 0 || int.Parse(comboBox2.Text) <= 0 || int.Parse(comboBox1.Text) <= 0 || int.Parse(comboBox5.Text) <= 0)
            {MessageBox.Show("لا يجوز استخدام قيم سالبة أو صفر"); return; }
            n = int.Parse(comboBox3.Text);
            timer4.Interval = 100;
            c_pulse = int.Parse(comboBox5.Text);
                for (int i = 0; i < int.Parse(comboBox3.Text) + 1; i++)
                {
                    dataGridView1.RowCount = i + 1;
                    //
                    string s1,s2;
                    if (cch < 10) s1 = "0" + cch; else s1 = ""+cch;
                    if (ccm < 10) s2 = "0" + ccm; else s2 = ""+ccm;
                    dataGridView1[1, i].Value =s1+":"+s2;
                    //
                    if (i == int.Parse(comboBox1.Text))
                    {
                        cch = addh(int.Parse(comboBox2.Text), ccm, cch);
                        ccm = addm(int.Parse(comboBox2.Text), ccm, cch);
                        dataGridView1[0, i].Value = "الفرصة";
                        //
                        if (cch < 10) s1 = "0" + cch; else s1 = "" + cch;
                        if (ccm < 10) s2 = "0" + ccm; else s2 = "" + ccm;
                        dataGridView1[2, i].Value = s1 + ":" + s2;
                        //
                    }
                    else
                    {
                        cch = addh(int.Parse(comboBox4.Text), ccm, cch);
                        ccm = addm(int.Parse(comboBox4.Text), ccm, cch);
                        if (i > int.Parse(comboBox1.Text))
                            dataGridView1[0, i].Value = nam(i - 1);
                        else
                            dataGridView1[0, i].Value = nam(i);
                        //
                        if (cch < 10) s1 = "0" + cch; else s1 = "" + cch;
                        if (ccm < 10) s2 = "0" + ccm; else s2 = "" + ccm;
                        dataGridView1[2, i].Value = s1 + ":" + s2;
                        //
                        if (checkBox1.Checked)
                        {
                            cch = addh(5, ccm, cch);
                            ccm = addm(5, ccm, cch);
                        }
                    }

                }
                //write
                FileStream s = new FileStream("config.txt", FileMode.Open);
                StreamWriter w = new StreamWriter(s);
                w.WriteLine(comboBox3.Text);
                w.WriteLine(comboBox4.Text);
                w.WriteLine(comboBox1.Text);
                w.WriteLine(comboBox2.Text);
                if (checkBox1.Checked)w.WriteLine("1");
                else w.WriteLine("0");
                w.WriteLine(comboBox5.Text);
                w.Close();
                //
                //run thread
                dataGridView1[0, 0].Selected = false;
                timer1.Enabled = true;

            }
            catch (Exception ex)
            { MessageBox.Show("Error!"); }
        }

        private string nam(int i)
        {
            if (i == 0) return "الأولى";
            else if (i == 1) return "الثانية";
            else if (i == 2) return "الثالثة";
            else if (i == 3) return "الرابعة";
            else if (i == 4) return "الخامسة";
            else if (i == 5) return "السادسة";
            else if (i == 6) return "السابعة";
            else if (i == 7) return "الثامنة";
            else if (i == 8) return "التاسعة";
            else if (i == 9) return "العاشرة";
            else return "";
            
        }

        private int addm(int p, int ccm, int cch)
        {
            int r = p + ccm;
            if (r < 60) return r;
            else return r - 60;
        }

        private int addh(int p, int ccm, int cch)
        {
            int r = p + ccm;
            if (r < 60) return cch;
            else return cch+1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Title = "Select a Wave Sound File";
            of.Filter = "Wav Files(*.wav)|*.wav";
             of.ShowDialog();
             textBox1.Text = of.FileName;
             
             
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //parPort.Output(888, 0);
            label7.Text = "0";
            DateTime dt = DateTime.Now;
            DateTimeFormatInfo dtfi = DateTimeFormatInfo.InvariantInfo;
            textBox2.Text = dt.ToString("F",dtfi);
            //read
            FileStream s = new FileStream("config.txt", FileMode.Open);
            StreamReader r = new StreamReader(s);
            comboBox3.Text = r.ReadLine();
            comboBox4.Text = r.ReadLine();
            comboBox1.Text = r.ReadLine();
            comboBox2.Text = r.ReadLine();
            if (r.ReadLine() == "1") checkBox1.Checked = true;
            else checkBox1.Checked = false;
            comboBox5.Text = r.ReadLine();
            r.Close();
            //
            button1_Click(sender, e);
            textBox1.Text = Directory.GetCurrentDirectory() + "\\schoolbell.wav";
            //textBox1.Text="C:\\Documents and Settings\\lddl\\سطح المكتب\\DingDong.wav";
            
            this.Visible = false;
        }


        void timer1_Tick(object sender, EventArgs e)
        {
            
                DateTime dt = DateTime.Now;
                DateTimeFormatInfo dtfi = DateTimeFormatInfo.InvariantInfo;
                for (int i = 0; i < n + 1; i++)
                {
                    if (string.Equals(dt.ToString("t", dtfi), dataGridView1[1, i].Value) && dt.ToString("T", dtfi).EndsWith("00"))
                    {
                        if (File.Exists(textBox1.Text))
                        {
                            SoundPlayer sp = new SoundPlayer(textBox1.Text);
                            sp.Play();
                        }
                        //out
                        //parPort.Output(888, 1);
                        //label4.Text = "on";
                        status = true;
                        counter = 0;
                        timer4.Enabled = true;
                        //
                        for (int j = 0; j < n + 1; j++)
                        dataGridView1[0, j].Selected = false;
                        dataGridView1[0, i].Selected = true;
                        return;
                    }
                }
                for (int i = 0; i < n + 1; i++)
                {
                    if (string.Equals(dt.ToString("t", dtfi), dataGridView1[2, i].Value) && dt.ToString("T", dtfi).EndsWith("00"))
                    {
                        if (File.Exists(textBox1.Text))
                        {
                            SoundPlayer sp = new SoundPlayer(textBox1.Text);
                            sp.Play();
                        }
                        //out
                        //parPort.Output(888, 1);
                        //label4.Text = "on";
                        timer4.Enabled = true;
                        //
                        //dataGridView1[0, i].Selected = true;
                        return;
                    }
                }
                for (int i = 0; i < n + 1; i++)
                {   int c=int.Parse(dt.ToString("t", dtfi).Substring(0,2))*60+int.Parse(dt.ToString("t", dtfi).Substring(3,2));
                    int s=int.Parse(dataGridView1[1, i].Value.ToString().Substring(0,2))*60+int.Parse(dataGridView1[1, i].Value.ToString().Substring(3,2));
                    int e1=int.Parse(dataGridView1[2, i].Value.ToString().Substring(0,2))*60+int.Parse(dataGridView1[2, i].Value.ToString().Substring(3,2));
                    if (c>=s&&c<e1)
                    {
                        for (int j = 0; j < n + 1; j++)
                            dataGridView1[0, j].Selected = false;
                        dataGridView1[0, i].Selected = true;
                        return;
                    }
                }
            
        }


        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit ?","", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                //parPort.Output(888, 0);
                label7.Text = "0";
                Application.Exit();
                notifyIcon1.Visible = false;
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            this.Visible = false;
            timer3.Enabled = false;
        }

        void timer4_Tick(object sender, EventArgs e)
        {
            if (status == true)
            {
                //parPort.Output(888, 1);
                label7.Text = "1";
                status = !status;
                timer4.Interval = 1000;
                counter++;
            }
            else
            {
               // parPort.Output(888, 0);
                label7.Text = "0";
                status = !status;
                timer4.Interval = 300;
            }
            if (counter >= int.Parse(comboBox5.Text))
            {
                
                //parPort.Output(888, 0); 
                label7.Text = "0";
                counter = 0;
                timer4.Enabled = false;
            }

            //label4.Text = "finish";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //parPort.Output(888, 0);
            label7.Text = "0";
            timer4.Enabled = false;

        }

                

        }
    }
