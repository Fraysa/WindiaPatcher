namespace WindiaPatcher
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;

    public class PatcherForm : Form
    {
        private PatchStatus m_PatchStatus;
        private List<PatchEntry> m_aNeedsUpdate = new List<PatchEntry>();
        private List<PatchEntry> m_aPendingPatch = new List<PatchEntry>();
        private int m_nFinished;
        private IContainer components;
        private Label labelStatus;
        private Label labelStatusText;
        private Button buttonCheckForUpdates;
        private Button buttonDownload;
        private ProgressBar downloadProgressBar;

        public PatcherForm()
        {
            this.InitializeComponent();
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void ButtonCheckForUpdates_Click(object sender, EventArgs e)
        {
            if ((this.m_PatchStatus != PatchStatus.Checking) && (this.m_PatchStatus != PatchStatus.Updating))
            {
                this.downloadProgressBar.Value = 0;
                this.m_aNeedsUpdate.Clear();
                using (WebClient client = new WebClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    client.DownloadStringCompleted += delegate (object sdr, DownloadStringCompletedEventArgs ex) {
                        string[] textArray1 = new string[] { ex.Result.Contains(Environment.NewLine) ? Environment.NewLine : "\n" };
                        string[] separator = new string[] { ex.Result.Contains(Environment.NewLine) ? Environment.NewLine : "\n" };
                        string[] strArray = ex.Result.Split(separator, StringSplitOptions.None);
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            char[] chArray1 = new char[] { ' ' };
                            string[] strArray2 = strArray[i].Split(chArray1, 3);
                            this.m_aNeedsUpdate.Add(new PatchEntry(strArray2[0], (long) int.Parse(strArray2[1]), strArray2[2]));
                        }
                        this.CheckForUpdates();
                    };
                    try
                    {
                        client.DownloadStringAsync(new Uri("https://ac.windia.me/patch_info"));
                    }
                    catch (WebException)
                    {
                        MessageBox.Show("Error connecting to patch server.", "Patcher error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    this.labelStatusText.Text = "Checking...";
                    this.m_PatchStatus = PatchStatus.Checking;
                }
            }
        }

        private void ButtonDownload_Click(object sender, EventArgs e)
        {
            if (((this.m_PatchStatus != PatchStatus.Checking) && (this.m_PatchStatus != PatchStatus.Updating)) && ((this.m_aNeedsUpdate.Count < 20) || (System.IO.File.Exists("Windia.exe") || (MessageBox.Show("This seems to be your first time using the patcher!\n*Please* make sure that this patcher file is located in an *empty* folder. This patcher will download the game files to the same folder it is being ran from!\n\nIf you're having issues, try to disable your antivirus software.\nIf you can't get the patcher to work, feel free to ask for help on forums or on our Discord server.\n\nDo you want to proceed?", "Windia Patcher", MessageBoxButtons.YesNo) != DialogResult.No))))
            {
                Process[] processesByName = Process.GetProcessesByName("Windia.dll");
                if ((processesByName.Length != 0) && (MessageBox.Show("Seems like Windia is running on your computer. Patching the game while it is running might not work.\n\nSelect Yes to close all Windia processes, or No to attempt patching regardless.", "Windia Patcher", MessageBoxButtons.YesNo) == DialogResult.Yes))
                {
                    Process[] processArray2 = processesByName;
                    for (int i = 0; i < processArray2.Length; i++)
                    {
                        processArray2[i].Kill();
                        Thread.Sleep(0x3e8);
                    }
                }
                this.m_PatchStatus = PatchStatus.Updating;
                new Thread(new ThreadStart(this.DownloadThread)).Start();
            }
        }

        private void CheckForUpdates()
        {
            this.m_aPendingPatch.Clear();
            foreach (PatchEntry entry in this.m_aNeedsUpdate)
            {
                if (entry.SizeInBytes != this.GetFileSize(entry.FileName))
                {
                    this.m_aPendingPatch.Add(entry);
                }
            }
            if (this.m_aPendingPatch.Count == 0)
            {
                this.m_PatchStatus = PatchStatus.Updated;
                this.labelStatusText.Text = "Files are updated.";
            }
            else
            {
                this.m_PatchStatus = PatchStatus.Outdated;
                this.labelStatusText.Text = "Files are outdated.";
                this.buttonDownload.Enabled = true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        [AsyncStateMachine(typeof(<DownloadThread>d__11))]
        private void DownloadThread()
        {
            <DownloadThread>d__11 d__;
            d__.<>4__this = this;
            d__.<>t__builder = AsyncVoidMethodBuilder.Create();
            d__.<>1__state = -1;
            d__.<>t__builder.Start<<DownloadThread>d__11>(ref d__);
        }

        private long GetFileSize(string filename) => 
            (System.IO.File.Exists(filename) ? new FileInfo(filename).Length : 0L);

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(PatcherForm));
            this.labelStatus = new Label();
            this.labelStatusText = new Label();
            this.buttonCheckForUpdates = new Button();
            this.buttonDownload = new Button();
            this.downloadProgressBar = new ProgressBar();
            base.SuspendLayout();
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new Point(9, 0x12);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new Size(40, 13);
            this.labelStatus.TabIndex = 0;
            this.labelStatus.Text = "Status:";
            this.labelStatusText.AutoSize = true;
            this.labelStatusText.Location = new Point(0x37, 0x12);
            this.labelStatusText.Name = "labelStatusText";
            this.labelStatusText.Size = new Size(0x38, 13);
            this.labelStatusText.TabIndex = 0;
            this.labelStatusText.Text = "Unknown.";
            this.buttonCheckForUpdates.Location = new Point(11, 0x2e);
            this.buttonCheckForUpdates.Name = "buttonCheckForUpdates";
            this.buttonCheckForUpdates.Size = new Size(0x87, 0x17);
            this.buttonCheckForUpdates.TabIndex = 1;
            this.buttonCheckForUpdates.Text = "Check for updates";
            this.buttonCheckForUpdates.UseVisualStyleBackColor = true;
            this.buttonCheckForUpdates.Click += new EventHandler(this.ButtonCheckForUpdates_Click);
            this.buttonDownload.Enabled = false;
            this.buttonDownload.Location = new Point(0x99, 0x2e);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new Size(0x88, 0x17);
            this.buttonDownload.TabIndex = 2;
            this.buttonDownload.Text = "Download files";
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Click += new EventHandler(this.ButtonDownload_Click);
            this.downloadProgressBar.Location = new Point(11, 0x54);
            this.downloadProgressBar.Name = "downloadProgressBar";
            this.downloadProgressBar.Size = new Size(0x116, 0x17);
            this.downloadProgressBar.Step = 1;
            this.downloadProgressBar.TabIndex = 3;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x12d, 0x77);
            base.Controls.Add(this.downloadProgressBar);
            base.Controls.Add(this.buttonDownload);
            base.Controls.Add(this.buttonCheckForUpdates);
            base.Controls.Add(this.labelStatusText);
            base.Controls.Add(this.labelStatus);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.Name = "PatcherForm";
            this.Text = "Windia Patcher";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void OnDownloadFileAsyncCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.m_nFinished++;
            this.PerformStepSafe(this.downloadProgressBar);
            this.SetLabelTextSafe(this.labelStatusText, $"Downloading... ({this.m_nFinished}/{this.m_aPendingPatch.Count})");
            if (this.m_nFinished >= this.m_aPendingPatch.Count)
            {
                this.m_PatchStatus = PatchStatus.Done;
                this.SetLabelTextSafe(this.labelStatusText, "Done patching!");
            }
        }

        private void PerformStepSafe(ProgressBar bar)
        {
            if (!base.Disposing && !base.IsDisposed)
            {
                if (!bar.InvokeRequired)
                {
                    bar.PerformStep();
                }
                else
                {
                    PerformStepCallback method = new PerformStepCallback(this.PerformStepSafe);
                    object[] args = new object[] { bar };
                    base.Invoke(method, args);
                }
            }
        }

        private void SetBarValuesSafe(ProgressBar bar, int value, int maximum)
        {
            if (!base.Disposing && !base.IsDisposed)
            {
                if (!bar.InvokeRequired)
                {
                    bar.Value = value;
                    bar.Maximum = maximum;
                }
                else
                {
                    SetBarValuesCallback method = new SetBarValuesCallback(this.SetBarValuesSafe);
                    object[] args = new object[] { bar, value, maximum };
                    base.Invoke(method, args);
                }
            }
        }

        private void SetButtonEnabledSafe(Button button, bool enabled)
        {
            if (!base.Disposing && !base.IsDisposed)
            {
                if (!button.InvokeRequired)
                {
                    button.Enabled = enabled;
                }
                else
                {
                    SetButtonEnabledCallback method = new SetButtonEnabledCallback(this.SetButtonEnabledSafe);
                    object[] args = new object[] { button, enabled };
                    base.Invoke(method, args);
                }
            }
        }

        private void SetLabelTextSafe(Label label, string text)
        {
            if (!base.Disposing && !base.IsDisposed)
            {
                if (!label.InvokeRequired)
                {
                    label.Text = text;
                }
                else
                {
                    SetLabelTextCallback method = new SetLabelTextCallback(this.SetLabelTextSafe);
                    object[] args = new object[] { label, text };
                    base.Invoke(method, args);
                }
            }
        }

        [CompilerGenerated]
        private struct <DownloadThread>d__11 : IAsyncStateMachine
        {
            public int <>1__state;
            public AsyncVoidMethodBuilder <>t__builder;
            public PatcherForm <>4__this;
            private List<PatchEntry>.Enumerator <>7__wrap1;
            private WebClient <pWebClient>5__3;
            private TaskAwaiter <>u__1;

            private void MoveNext()
            {
                long nBytes;
                DateTime pLastUpdate;
                int num = this.<>1__state;
                PatcherForm form = this.<>4__this;
                try
                {
                    if (num != 0)
                    {
                        form.SetBarValuesSafe(form.downloadProgressBar, 0, form.m_aPendingPatch.Count);
                        form.m_nFinished = 0;
                        form.SetLabelTextSafe(form.labelStatusText, $"Downloading... (0/{form.m_aPendingPatch.Count})");
                        form.SetButtonEnabledSafe(form.buttonDownload, false);
                        this.<>7__wrap1 = form.m_aPendingPatch.GetEnumerator();
                    }
                    try
                    {
                        if (num == 0)
                        {
                            goto TR_0015;
                        }
                        goto TR_0018;
                    TR_000A:
                        this.<pWebClient>5__3 = null;
                        goto TR_0018;
                    TR_0015:
                        try
                        {
                            if (num != 0)
                            {
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                            }
                            try
                            {
                                TaskAwaiter awaiter;
                                if (num == 0)
                                {
                                    awaiter = this.<>u__1;
                                    this.<>u__1 = new TaskAwaiter();
                                    this.<>1__state = num = -1;
                                    goto TR_000D;
                                }
                                else
                                {
                                    PatcherForm.<>c__DisplayClass11_0 class_;
                                    pLastUpdate = DateTime.Now;
                                    nBytes = 0L;
                                    this.<pWebClient>5__3.DownloadFileCompleted += new AsyncCompletedEventHandler(form.OnDownloadFileAsyncCompleted);
                                    this.<pWebClient>5__3.DownloadProgressChanged += new DownloadProgressChangedEventHandler(class_.<DownloadThread>b__0);
                                    awaiter = this.<pWebClient>5__3.DownloadFileTaskAsync(new Uri(pEntry.URL), pEntry.FileName).GetAwaiter();
                                    if (awaiter.IsCompleted)
                                    {
                                        goto TR_000D;
                                    }
                                    else
                                    {
                                        this.<>1__state = num = 0;
                                        this.<>u__1 = awaiter;
                                        this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, PatcherForm.<DownloadThread>d__11>(ref awaiter, ref this);
                                    }
                                }
                                return;
                            TR_000D:
                                awaiter.GetResult();
                                Thread.Sleep(500);
                            }
                            catch (WebException exception)
                            {
                                MessageBox.Show("Error: " + exception.Message, "Patcher error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            }
                        }
                        finally
                        {
                            if ((num < 0) && (this.<pWebClient>5__3 != null))
                            {
                                this.<pWebClient>5__3.Dispose();
                            }
                        }
                        goto TR_000A;
                    TR_0018:
                        while (true)
                        {
                            if (this.<>7__wrap1.MoveNext())
                            {
                                PatcherForm <>4__this = form;
                                PatchEntry pEntry = this.<>7__wrap1.Current;
                                this.<pWebClient>5__3 = new WebClient();
                            }
                            else
                            {
                                goto TR_0006;
                            }
                            break;
                        }
                        goto TR_0015;
                    }
                    finally
                    {
                        if (num < 0)
                        {
                            this.<>7__wrap1.Dispose();
                        }
                    }
                    return;
                TR_0006:
                    this.<>7__wrap1 = new List<PatchEntry>.Enumerator();
                    this.<>1__state = -2;
                    this.<>t__builder.SetResult();
                }
                catch (Exception exception2)
                {
                    this.<>1__state = -2;
                    this.<>t__builder.SetException(exception2);
                }
            }

            [DebuggerHidden]
            private void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.<>t__builder.SetStateMachine(stateMachine);
            }
        }

        private enum PatchStatus
        {
            Unknown,
            Checking,
            Updated,
            Outdated,
            Updating,
            Done
        }

        private delegate void PerformStepCallback(ProgressBar bar);

        private delegate void SetBarValuesCallback(ProgressBar bar, int value, int maximum);

        private delegate void SetButtonEnabledCallback(Button button, bool enabled);

        private delegate void SetLabelTextCallback(Label label, string text);
    }
}

