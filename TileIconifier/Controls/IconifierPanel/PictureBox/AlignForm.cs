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
using System.Windows.Forms;
using TileIconifier.Properties;

namespace TileIconifier.Controls.IconifierPanel.PictureBox
{
    public partial class AlignForm : Form
    {
        private PannableImageAdjustement _timerClick;

        public AlignForm()
        {
            InitializeComponent();
            Deactivate += (sender, args) => Close();
        }

        public event AlignFormEvent AlignFormClick;

        protected virtual void OnAlignFormClick(PannableImageAdjustement alignbuttonclick)
        {
            AlignFormClick?.Invoke(this, new AlignFormEventArgs(alignbuttonclick));
        }

        private void AlignForm_Load(object sender, EventArgs e)
        {
            Width = 92; //Seems to reset to some minimum width otherwise
            BuildTooltips();
            AddEventHandlers();
        }

        private void AddEventHandlers()
        {
            btnLeft.Click += (sender, args) => OnAlignFormClick(PannableImageAdjustement.LeftAlign);
            btnRight.Click += (sender, args) => OnAlignFormClick(PannableImageAdjustement.RightAlign);
            btnTop.Click += (sender, args) => OnAlignFormClick(PannableImageAdjustement.TopAlign);
            btnBottom.Click += (sender, args) => OnAlignFormClick(PannableImageAdjustement.BottomAlign);
            btnXMiddle.Click += (sender, args) => OnAlignFormClick(PannableImageAdjustement.XAlign);
            btnYMiddle.Click += (sender, args) => OnAlignFormClick(PannableImageAdjustement.YAlign);
            btnAlignCenter.Click += (sender, args) => OnAlignFormClick(PannableImageAdjustement.Center);

            btnNudgeLeft.MouseDown += (sender, args) => TimerDown(PannableImageAdjustement.NudgeLeft);
            btnNudgeDown.MouseDown += (sender, args) => TimerDown(PannableImageAdjustement.NudgeDown);
            btnNudgeUp.MouseDown += (sender, args) => TimerDown(PannableImageAdjustement.NudgeUp);
            btnNudgeRight.MouseDown += (sender, args) => TimerDown(PannableImageAdjustement.NudgeRight);
            btnNudgeLeft.MouseUp += (sender, args) => TimerUp();
            btnNudgeDown.MouseUp += (sender, args) => TimerUp();
            btnNudgeUp.MouseUp += (sender, args) => TimerUp();
            btnNudgeRight.MouseUp += (sender, args) => TimerUp();
        }

        private void BuildTooltips()
        {
            var toolTip = new ToolTip();
            toolTip.SetToolTip(btnTop, Strings.AlignTop);
            toolTip.SetToolTip(btnBottom, Strings.AlignBottom);
            toolTip.SetToolTip(btnRight, Strings.AlignRight);
            toolTip.SetToolTip(btnLeft, Strings.AlignLeft);
            toolTip.SetToolTip(btnXMiddle, Strings.AlignXMiddle);
            toolTip.SetToolTip(btnYMiddle, Strings.AlignYMiddle);
            toolTip.SetToolTip(btnAlignCenter, Strings.AlignCentre);
            toolTip.SetToolTip(btnNudgeLeft, Strings.NudgeLeft);
            toolTip.SetToolTip(btnNudgeRight, Strings.NudgeRight);
            toolTip.SetToolTip(btnNudgeUp, Strings.NudgeUp);
            toolTip.SetToolTip(btnNudgeDown, Strings.NudgeDown);
        }

        private void tmrNudge_Tick(object sender, EventArgs e)
        {
            if (_timerClick == PannableImageAdjustement.Unknown)
                return;
            OnAlignFormClick(_timerClick);
        }


        private void TimerDown(PannableImageAdjustement clickType)
        {
            _timerClick = clickType;
            tmrNudge_Tick(this, null);
            tmrNudge.Enabled = true;
        }

        private void TimerUp()
        {
            tmrNudge.Enabled = false;
            _timerClick = PannableImageAdjustement.Unknown;
        }
    }

    public enum PannableImageAdjustement
    {
        Unknown = 0,
        LeftAlign = 1,
        RightAlign = 2,
        TopAlign = 3,
        BottomAlign = 4,
        XAlign = 5,
        YAlign = 6,
        NudgeUp = 7,
        NudgeDown = 8,
        NudgeLeft = 9,
        NudgeRight = 10,
        Center = 11
    }

    public class AlignFormEventArgs : EventArgs
    {
        public PannableImageAdjustement AlignButtonClicked { get; set; }

        public AlignFormEventArgs(PannableImageAdjustement alignButtonClick)
        {
            AlignButtonClicked = alignButtonClick;
        }
    }

    public delegate void AlignFormEvent(object sender, AlignFormEventArgs e);
}