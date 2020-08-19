using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAsync
{
    public partial class Form1 : Form
    {
        private readonly FakeWork worker = new FakeWork();

        public Form1()
        {
            InitializeComponent();
            EventBinding();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            worker.ProgressEvent -= Worker_ProgressEvent;
            base.Dispose(disposing);
        }

        public void EventBinding()
        {
            buttonStart.Click += ButtonStart_ClickAsync;
            worker.ProgressEvent += Worker_ProgressEvent;

            button1.Click += OnUpdateLabel1;
            button2.Click += OnUpdateLabel2;
            button3.Click += OnUpdateLabel3;
            button4.Click += OnUpdateLabel4;
        }

        private async void ButtonStart_ClickAsync(object sender, EventArgs e)
        {
            Console.WriteLine($"ButtonStart_ClickAsync Thread ID = {Thread.CurrentThread.ManagedThreadId}");
            await worker.LongRunAsync();
        }

        private void Worker_ProgressEvent(object sender, EventArgs e)
        {
            Console.WriteLine($"Worker_ProgressEvent Thread ID = {Thread.CurrentThread.ManagedThreadId}");
            int i = (int)sender;

            //Update1(i);
            Update2(i);
        }

        private void Update1(int i)
        {
            labelStart.Text = i.ToString();
        }

        private void Update2(int i)
        {
            Console.WriteLine($"enter Update2 Thread ID = {Thread.CurrentThread.ManagedThreadId}");
            if (InvokeRequired)
            {
                Invoke(new Action<int>(Update2), i);
                return;
            }

            Console.WriteLine($"Update2 Thread ID = {Thread.CurrentThread.ManagedThreadId}");
            labelStart.Text = i.ToString();
        }

        private async void OnUpdateLabel1(object sender, EventArgs e)
        {
            Console.WriteLine($"before OnUpdateLabel1 Thread ID = {Thread.CurrentThread.ManagedThreadId}");
            button1.Enabled = false;

            int i = int.Parse(label1.Text);
            Task<int> t = Task.Run(() => worker.Calc(i));
            int j = await t; // non-blocking
            label1.Text = j.ToString();

            Console.WriteLine($"after OnUpdateLabel1 Thread ID = {Thread.CurrentThread.ManagedThreadId}");
            button1.Enabled = true;
        }

        private void OnUpdateLabel2(object sender, EventArgs e)
        {
            Console.WriteLine($"before OnUpdateLabel2 Thread ID = {Thread.CurrentThread.ManagedThreadId}");
            button2.Enabled = false;

            int i = int.Parse(label2.Text);
            Task<int> t = Task.Run(() => worker.Calc(i));
            int j = t.Result; // blocking
            label2.Text = j.ToString();

            Console.WriteLine($"after OnUpdateLabel2 Thread ID = {Thread.CurrentThread.ManagedThreadId}");          
            button2.Enabled = true;
        }

        private async void OnUpdateLabel3(object sender, EventArgs e)
        {
            Console.WriteLine($"before OnUpdateLabel3 Thread ID = {Thread.CurrentThread.ManagedThreadId}");
            button3.Enabled = false;

            int i = int.Parse(label3.Text);
            Task<int> t = worker.CalcAsync(i);
            int j = await t; // non-blocking
            label3.Text = j.ToString();

            Console.WriteLine($"after OnUpdateLabel3 Thread ID = {Thread.CurrentThread.ManagedThreadId}");
            button3.Enabled = true;
        }

        //https://stackoverflow.com/questions/28601678/calling-async-method-on-button-click
        //https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html
        // await 之后的线程一定是 UI 线程
        private void OnUpdateLabel4(object sender, EventArgs e)
        {
            Console.WriteLine($"before OnUpdateLabel4 Thread ID = {Thread.CurrentThread.ManagedThreadId}");
            button4.Enabled = false;

            int i = int.Parse(label4.Text);
            Task<int> t = worker.CalcAsync(i);
            int j = t.Result; // blocking，主线程被占用，形成死锁
            label4.Text = j.ToString();

            Console.WriteLine($"after OnUpdateLabel4 Thread ID = {Thread.CurrentThread.ManagedThreadId}");
            button4.Enabled = true;
        }

    }
}
