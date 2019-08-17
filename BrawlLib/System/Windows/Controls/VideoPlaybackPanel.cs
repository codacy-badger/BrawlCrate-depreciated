﻿using BrawlLib.Imaging;
using System.Audio;
using System.ComponentModel;

namespace System.Windows.Forms
{
    public class VideoPlaybackPanel : UserControl
    {
        #region Designer

        private CustomTrackBar trackBar1;
        private Button btnPlay;
        private Button btnRewind;
        private Label lblProgress;
        private readonly IContainer components;
        private PreviewPanel previewPanel1;
        public CheckBox chkLoop;

        private void InitializeComponent()
        {
            btnPlay = new Button();
            btnRewind = new Button();
            chkLoop = new CheckBox();
            lblProgress = new Label();
            previewPanel1 = new PreviewPanel();
            trackBar1 = new CustomTrackBar();
            ((ISupportInitialize) trackBar1).BeginInit();
            SuspendLayout();
            // 
            // btnPlay
            // 
            btnPlay.Anchor = AnchorStyles.Bottom;
            btnPlay.Location = new System.Drawing.Point(152, 263);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new System.Drawing.Size(75, 20);
            btnPlay.TabIndex = 1;
            btnPlay.Text = "Play";
            btnPlay.UseVisualStyleBackColor = true;
            btnPlay.Click += new EventHandler(btnPlay_Click);
            // 
            // btnRewind
            // 
            btnRewind.Anchor = AnchorStyles.Bottom;
            btnRewind.Location = new System.Drawing.Point(122, 263);
            btnRewind.Name = "btnRewind";
            btnRewind.Size = new System.Drawing.Size(24, 20);
            btnRewind.TabIndex = 2;
            btnRewind.Text = "|<";
            btnRewind.UseVisualStyleBackColor = true;
            btnRewind.Click += new EventHandler(btnRewind_Click);
            // 
            // chkLoop
            // 
            chkLoop.Anchor = AnchorStyles.Bottom;
            chkLoop.Location = new System.Drawing.Point(27, 263);
            chkLoop.Name = "chkLoop";
            chkLoop.Size = new System.Drawing.Size(100, 20);
            chkLoop.TabIndex = 3;
            chkLoop.Text = "Loop Preview";
            chkLoop.UseVisualStyleBackColor = true;
            chkLoop.CheckedChanged += new EventHandler(chkLoop_CheckedChanged);
            // 
            // lblProgress
            // 
            lblProgress.Anchor = AnchorStyles.Bottom;
            lblProgress.Location = new System.Drawing.Point(-79, 239);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new System.Drawing.Size(536, 23);
            lblProgress.TabIndex = 4;
            lblProgress.Text = "0/0";
            lblProgress.TextAlign = Drawing.ContentAlignment.MiddleCenter;
            // 
            // previewPanel1
            // 
            previewPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                                                    | AnchorStyles.Left
                                                    | AnchorStyles.Right;
            previewPanel1.CurrentIndex = 0;
            previewPanel1.DisposeImage = true;
            previewPanel1.Location = new System.Drawing.Point(3, 3);
            previewPanel1.Name = "previewPanel1";
            previewPanel1.RenderingTarget = null;
            previewPanel1.Size = new System.Drawing.Size(372, 203);
            previewPanel1.TabIndex = 5;
            // 
            // trackBar1
            // 
            trackBar1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left
                                                   | AnchorStyles.Right;
            trackBar1.Location = new System.Drawing.Point(0, 212);
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new System.Drawing.Size(378, 45);
            trackBar1.TabIndex = 0;
            trackBar1.TickFrequency = 2;
            trackBar1.UserSeek += new EventHandler(trackBar1_UserSeek);
            trackBar1.ValueChanged += new EventHandler(trackBar1_ValueChanged);
            // 
            // VideoPlaybackPanel
            // 
            Controls.Add(previewPanel1);
            Controls.Add(lblProgress);
            Controls.Add(btnRewind);
            Controls.Add(btnPlay);
            Controls.Add(trackBar1);
            Controls.Add(chkLoop);
            Name = "VideoPlaybackPanel";
            Size = new System.Drawing.Size(378, 289);
            ((ISupportInitialize) trackBar1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private bool _loop = false;

        private bool _isPlaying = false;
        //private bool _isScrolling = false;

        private DateTime _frameTime;
        private readonly CoolTimer _timer;

        private IVideo _targetSource;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IVideo TargetSource
        {
            get => _targetSource;
            set => TargetChanged(value);
        }

        private AudioProvider _provider;
        private AudioBuffer _buffer;

        public VideoPlaybackPanel()
        {
            InitializeComponent();

            _timer = new CoolTimer();
            _timer.RenderFrame += new EventHandler<FrameEventArgs>(RenderUpdate);

            previewPanel1.LeftClicked += previewPanel1_LeftClicked;
            previewPanel1.RightClicked += previewPanel1_RightClicked;
        }

        private void previewPanel1_RightClicked(object sender, EventArgs e)
        {
            Seek(_frame + 1);
        }

        private void previewPanel1_LeftClicked(object sender, EventArgs e)
        {
            Seek(_frame - 1);
        }

        public void RenderUpdate(object sender, FrameEventArgs e)
        {
            if (_isPlaying)
            {
                //TODO: Sync video to audio
                if (_buffer != null)
                {
                    _buffer.Fill();
                }

                trackBar1.Value = ++_frame;

                if (_frame >= _targetSource.NumFrames)
                {
                    if (!_loop)
                    {
                        Stop();
                    }
                    else
                    {
                        _frame = 0;
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            Close();
            if (_provider != null)
            {
                _provider.Dispose();
                _provider = null;
            }

            base.Dispose(disposing);
        }

        private void Close()
        {
            //Stop playback
            Stop();

            _targetSource = null;

            //Reset fields
            chkLoop.Checked = false;
        }

        private void TargetChanged(IVideo newTarget)
        {
            if (_targetSource == newTarget)
            {
                return;
            }

            Close();

            if ((_targetSource = newTarget) == null)
            {
                return;
            }

            previewPanel1.RenderingTarget = _targetSource;

            IAudioStream s = _targetSource.Audio;

            //Create provider
            if (_provider == null && s != null)
            {
                _provider = AudioProvider.Create(null);
                _provider.Attach(this);
            }

            chkLoop.Checked = false;

            //Create buffer for stream
            if (s != null)
            {
                _buffer = _provider.CreateBuffer(s);
            }

            if (_targetSource.FrameRate > 0)
            {
                _frameTime = new DateTime((long) (_targetSource.NumFrames * 10000000.0f / _targetSource.FrameRate));
            }

            trackBar1.TickStyle = TickStyle.None;
            trackBar1.Maximum = (int) _targetSource.NumFrames;
            trackBar1.Minimum = 1;
            trackBar1.Value = 1;

            if (_targetSource.FrameRate > 0)
            {
                UpdateTimeDisplay();
            }

            Enabled = _targetSource.NumFrames > 0;
        }

        private void UpdateTimeDisplay()
        {
            if (_targetSource == null)
            {
                return;
            }

            _frame = trackBar1.Value - 1;
            DateTime t = new DateTime((long) ((trackBar1.Value - 1) * 10000000.0f / _targetSource.FrameRate));
            lblProgress.Text = string.Format("{0:mm:ss.ff} / {1:mm:ss.ff} - Frame {2} of {3}", t, _frameTime,
                _frame + 1, TargetSource.NumFrames);

            previewPanel1.CurrentIndex = _targetSource.GetImageIndexAtFrame(_frame);
        }

        private void Seek(int frame)
        {
            bool temp = false;
            if (_isPlaying)
            {
                temp = true;
                Stop();
            }

            _frame = frame.Clamp(0, (int) _targetSource.NumFrames - 1);
            trackBar1.Value = _frame + 1;

            if (_buffer != null)
            {
                _buffer.Seek((int) Math.Round(frame / _targetSource.FrameRate * _targetSource.Frequency, 0));
            }

            if (temp)
            {
                Play();
            }
        }

        private void Play()
        {
            if (_targetSource == null)
            {
                return;
            }

            if (_isPlaying)
            {
                return;
            }

            _isPlaying = true;

            //Start from beginning if at end
            if (trackBar1.Value == _targetSource.NumFrames)
            {
                trackBar1.Value = 1;
            }

            btnPlay.Text = "Stop";
            previewPanel1.btnLeft.Visible = previewPanel1.btnRight.Visible = false;

            if (_buffer != null)
            {
                //Seek buffer to current sample
                _buffer.Seek((int) Math.Round((trackBar1.Value - 1) / _targetSource.FrameRate * _targetSource.Frequency,
                    0));

                //Fill initial buffer
                _buffer.Fill();

                //Begin playback
                _buffer.Play();
            }

            _timer.Run(0, _targetSource.FrameRate);
        }

        private void Stop()
        {
            if (!_isPlaying)
            {
                return;
            }

            _isPlaying = false;

            //Stop timer
            _timer.Stop();

            //Stop device
            if (_buffer != null)
            {
                _buffer.Stop();
            }

            btnPlay.Text = "Play";
            previewPanel1.btnLeft.Visible = previewPanel1.btnRight.Visible = true;
        }

        private int _frame = 0;

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (_isPlaying)
            {
                Stop();
            }
            else
            {
                Play();
            }
        }

        private void chkLoop_CheckedChanged(object sender, EventArgs e)
        {
            _loop = chkLoop.Checked;
            if (_buffer != null)
            {
                _buffer.Loop = _loop;
            }
        }

        private void btnRewind_Click(object sender, EventArgs e)
        {
            Seek(0);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            UpdateTimeDisplay();
        }

        private void trackBar1_UserSeek(object sender, EventArgs e)
        {
            Seek(trackBar1.Value - 1);
        }
    }
}