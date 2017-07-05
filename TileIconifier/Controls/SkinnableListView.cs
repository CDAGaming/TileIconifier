﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TileIconifier.Skinning.Skins;
using TileIconifier.Skinning.Utilities;

namespace TileIconifier.Controls
{
    class SkinnableListView : ListView, ISkinnableControl
    {
        private const string USE_FLATSTYLE_INSTEAD_ERROR = 
            "Use the FlatStyle property instead.";

        public SkinnableListView()
        {
            //Set the base class property to bypass the deprecated warning
            base.OwnerDraw = true;

            DoubleBuffered = true;
        }

        #region "Properties"
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete(USE_FLATSTYLE_INSTEAD_ERROR)]
        [DefaultValue(true)]
        public new bool OwnerDraw
        {
            get { return base.OwnerDraw; }
            set { throw new NotSupportedException(USE_FLATSTYLE_INSTEAD_ERROR); }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Obsolete(USE_FLATSTYLE_INSTEAD_ERROR)]
        public new BorderStyle BorderStyle
        {
            get { return base.BorderStyle; }
            set { throw new NotSupportedException(USE_FLATSTYLE_INSTEAD_ERROR); }
        }
        
        private FlatStyle flatStyle = FlatStyle.Standard;
        [DefaultValue(FlatStyle.Standard)]
        public FlatStyle FlatStyle
        {
            get { return flatStyle; }
            set
            {
                if (flatStyle != value)
                {
                    flatStyle = value;

                    switch (value)
                    {                        
                        case FlatStyle.Flat:
                        case FlatStyle.Popup:
                            //Popup effect not implemented, so FlatStyle.Popup behaves
                            //exactly like FlatStyle.Flat.
                            base.OwnerDraw = true;
                            base.BorderStyle = BorderStyle.FixedSingle;                            
                            break;

                        case FlatStyle.Standard:
                            //The appearance is still determined by the system, but at
                            //least the Paint events are raised.
                            base.OwnerDraw = true;
                            base.BorderStyle = BorderStyle.Fixed3D;
                            break;

                        default:
                            base.OwnerDraw = false;
                            base.BorderStyle = BorderStyle.Fixed3D;
                            break;
                    }                    
                }
            }
        }
                
        private bool drawStandardItems = true;
        [DefaultValue(true)]
        public bool DrawStandardItems
        {
            get { return drawStandardItems; }
            set
            {
                if (drawStandardItems != value)
                {
                    drawStandardItems = value;
                    Invalidate();
                }
            }
        }

        private Color flatHeaderBackColor = SystemColors.Control;
        [DefaultValue(typeof(Color), nameof(SystemColors.Control))]
        public Color FlatHeaderBackColor
        {
            get { return flatHeaderBackColor; }
            set
            {
                if (flatHeaderBackColor != value)
                {
                    flatHeaderBackColor = value;
                    if (FlatStyle == FlatStyle.Flat)
                    {
                        Invalidate();
                    }
                }                
            }
        }

        private Color flatHeaderForeColor = SystemColors.ControlText;
        [DefaultValue(typeof(Color), nameof(SystemColors.ControlText))]
        public Color FlatHeaderForeColor
        {
            get { return flatHeaderForeColor; }
            set
            {
                if (flatHeaderForeColor != value)
                {
                    flatHeaderForeColor = value;
                    if (FlatStyle == FlatStyle.Flat)
                    {
                        Invalidate();
                    }
                }
            }
        }

        private Color flatBorderColor = SystemColors.WindowFrame;
        [DefaultValue(typeof(Color), nameof(SystemColors.WindowFrame))]
        public Color FlatBorderColor
        {
            get { return flatBorderColor; }
            set
            {
                if (flatBorderColor != value)
                {
                    flatBorderColor = value;
                    {
                        //This color is also used for the focused and the 
                        //disabled states if their value is empty.
                        if (FlatStyle == FlatStyle.Flat && 
                            (!Focused && Enabled || (Focused && FlatBorderFocusedColor.IsEmpty) ||
                            (!Enabled && FlatBorderDisabledColor.IsEmpty)))
                        {
                            InvalidateNonClient();
                        }
                    }
                }
            }
        }

        private Color flatBorderFocusedColor = Color.Empty;
        [DefaultValue(typeof(Color), "")]
        public Color FlatBorderFocusedColor
        {
            get { return flatBorderFocusedColor; }
            set
            {
                if (flatBorderFocusedColor != value)
                {
                    flatBorderFocusedColor = value;
                    if (FlatStyle == FlatStyle.Flat && Focused)
                    {
                        InvalidateNonClient();
                    }
                }
            }
        }

        private Color flatBorderDisabledColor = Color.Empty;
        [DefaultValue(typeof(Color), "")]
        public Color FlatBorderDisabledColor
        {
            get { return flatBorderDisabledColor; }
            set
            {
                if (flatBorderDisabledColor != value)
                {
                    flatBorderDisabledColor = value;
                    if (FlatStyle == FlatStyle.Flat && !Enabled)
                    {
                        InvalidateNonClient();
                    }
                }
            }
        }
        #endregion

        protected override void OnEnter(EventArgs e)
        {
            if (BorderStyle == BorderStyle.FixedSingle)
            {
                InvalidateNonClient();
            }

            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            if (BorderStyle == BorderStyle.FixedSingle)
            {
                InvalidateNonClient();
            }

            base.OnLeave(e);
        }

        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {    
            if (FlatStyle == FlatStyle.Flat || FlatStyle == FlatStyle.Popup)
            {
                using (var b = new SolidBrush(FlatHeaderBackColor))
                    e.Graphics.FillRectangle(b, e.Bounds);

                TextFormatFlags flags =                     
                    TextFormatFlags.VerticalCenter |
                    TextFormatFlags.EndEllipsis |
                    LayoutAndPaintUtils.ConvertToTextFormatFlags(e.Header.TextAlign); //Header.TextAlign is already Rtl translated

                TextRenderer.DrawText(e.Graphics, e.Header.Text, Font, e.Bounds, FlatHeaderForeColor, flags);
            }
            else
            {
                e.DrawDefault = true;
            }

            base.OnDrawColumnHeader(e);
        }

        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            e.DrawDefault = DrawStandardItems;

            base.OnDrawItem(e);            
        }

        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = DrawStandardItems;

            base.OnDrawSubItem(e); 
        }

        private void InvalidateNonClient()
        {
            LayoutAndPaintUtils.InvalidateNonClient(this);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == NativeMethods.WM_NCPAINT && FlatStyle == FlatStyle.Flat)
            {
                PaintCustomBorder(m.HWnd, m.WParam);
            }                
        }

        private void PaintCustomBorder(IntPtr hDC, IntPtr hRgn)
        {
            Color bColor;
            if (!Enabled && !FlatBorderDisabledColor.IsEmpty)
            {
                bColor = FlatBorderDisabledColor;
            }
            else if (Focused && !FlatBorderFocusedColor.IsEmpty)
            {
                bColor = FlatBorderFocusedColor;
            }
            else if (FlatBorderColor != SystemColors.WindowFrame)
            {
                bColor = FlatBorderColor;
            }
            else
            {
                //Regular border, which has already been drawn by the system at this point
                return;
            }

            using (var ncg = new NonClientGraphics(hDC, hRgn))
            {
                if (ncg.Graphics == null)
                {
                    return;
                }

                ControlPaint.DrawBorder(ncg.Graphics, new Rectangle(new Point(0), Size), bColor, ButtonBorderStyle.Solid);
            }
        }

        public void ApplySkin(BaseSkin skin)
        {
            FlatStyle = skin.ListViewFlatStyle;
            FlatHeaderBackColor = skin.ListViewHeaderBackColor;
            FlatHeaderForeColor = skin.ListViewHeaderForeColor;
            BackColor = skin.ListViewBackColor;
            ForeColor = skin.ListViewForeColor;
            FlatBorderColor = skin.ListViewBorderColor;
            FlatBorderFocusedColor = skin.ListViewBorderFocusedColor;
            FlatBorderDisabledColor = skin.ListViewBorderDisabledColor;
        }
    }
}
