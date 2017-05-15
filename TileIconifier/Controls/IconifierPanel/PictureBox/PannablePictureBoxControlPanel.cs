﻿#region LICENCE

// /*
//         The MIT License (MIT)
// 
//         Copyright (c) 2016 Johnathon M
// 
//         Permission is hereby granted, free of charge, to any person obtaining a copy
//         of this software and associated documentation files (the "Software"), to deal
//         in the Software without restriction, including without limitation the rights
//         to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//         copies of the Software, and to permit persons to whom the Software is
//         furnished to do so, subject to the following conditions:
// 
//         The above copyright notice and this permission notice shall be included in
//         all copies or substantial portions of the Software.
// 
//         THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//         IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//         FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//         AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//         LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//         OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//         THE SOFTWARE.
// 
// */

#endregion

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TileIconifier.Properties;

namespace TileIconifier.Controls.IconifierPanel.PictureBox
{
    public partial class PannablePictureBoxControlPanel : UserControl
    {
        public PannablePictureBoxControlPanel()
        {
            InitializeComponent();
        }

        [Browsable(false)]
        public PannablePictureBox PannablePictureBox => panPct;

        [Localizable(true)]
        public string HeaderText
        {
            get { return lblHeader.Text; }
            set { lblHeader.Text = value; }
        }
        
        public Size PannablePictureBoxSize
        {
            get { return panPct.Size; }
            set { panPct.Size = value; }
        }        

        public event EventHandler ChangeImageClick;

        public void UpdateTrackBarAndZoom()
        {
            trkZoom.Value = (int) Math.Round(PannablePictureBox.GetZoomPercentage(), 1);
            UpdateZoomPercentage();
        }

        public void UpdateControls()
        {
            if (PannablePictureBox?.PannablePictureBoxImage.Image == null)
            {
                DisableControls();
                return;
            }
            EnableControls();
            UpdateTrackBarAndZoom();
        }        

        private void btnEnlarge_MouseDown(object sender, MouseEventArgs e)
        {
            tmrEnlarge_Tick(this, null);
            tmrEnlarge.Start();
        }

        private void btnEnlarge_MouseUp(object sender, MouseEventArgs e)
        {
            tmrEnlarge.Stop();
        }

        private void tmrEnlarge_Tick(object sender, EventArgs e)
        {
            PannablePictureBox.EnlargeImage();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            PannablePictureBox.ResetImage();
        }

        private void tmrShrink_Tick(object sender, EventArgs e)
        {
            PannablePictureBox.ShrinkImage();
        }

        private void btnShrink_MouseDown(object sender, MouseEventArgs e)
        {
            tmrShrink_Tick(this, null);
            tmrShrink.Start();
        }

        private void btnShrink_MouseUp(object sender, MouseEventArgs e)
        {
            tmrShrink.Stop();
        }

        private void btnOpenImage_Click(object sender, EventArgs e)
        {
            ChangeImageClick?.Invoke(this, null);
        }

        private void btnAlign_Click(object sender, EventArgs e)
        {
            var ctrl = (Control)sender;
            alignImageDropDown.Show(MousePosition);
        }        

        private void trkZoom_Scroll(object sender, EventArgs e)
        {
            PannablePictureBox.SetZoom(trkZoom.Value);
            UpdateZoomPercentage();
        }

        private void EnableControls()
        {
            trkZoom.Enabled = true;
            btnAlign.Enabled = true;
            btnEnlarge.Enabled = true;
            btnReset.Enabled = true;
            btnShrink.Enabled = true;
        }

        private void DisableControls()
        {
            trkZoom.Value = 1;
            trkZoom.Enabled = false;
            lblPercent.Text = @"---%";
            btnAlign.Enabled = false;
            btnEnlarge.Enabled = false;
            btnReset.Enabled = false;
            btnShrink.Enabled = false;
        }

        private void UpdateZoomPercentage()
        {
            lblPercent.Text = $@"{PannablePictureBox.GetZoomPercentage().ToString("F")}%";
        }
    }
}