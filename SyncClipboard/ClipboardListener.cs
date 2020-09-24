﻿using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SyncClipboard
{
    class ClipboardListener : System.Windows.Forms.Control
    {
        [DllImport("user32.dll")]
        public static extern bool AddClipboardFormatListener(IntPtr hwnd);
        [DllImport("user32.dll")]
        public static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

        private bool switchOn = false;
        private static readonly int WM_CLIPBOARDUPDATE = 0x031D;

        public delegate void ClipBoardChangedHandler();
        public event ClipBoardChangedHandler ClipBoardChanged;

        public ClipboardListener()
        {
            Enable();
        }

        public void Enable()
        {
            if (!switchOn)
            {
                AddClipboardFormatListener(this.Handle);
                switchOn = true;
            }
        }

        public void Disable()
        {
            if (switchOn)
            {
                RemoveClipboardFormatListener(this.Handle);
                switchOn = false;
            }
        }

        public void AddHandler(ClipBoardChangedHandler handler)
        {
            ClipBoardChanged += handler;
        }

        public void RemoveHandler(ClipBoardChangedHandler handler)
        {
            ClipBoardChanged -= handler;
        }

        protected override void DefWndProc(ref Message m)
        {
            if (m.Msg == WM_CLIPBOARDUPDATE)
            {
                ClipBoardChanged();
            }
            else
            {
                base.DefWndProc(ref m);
            }
        }

        ~ClipboardListener()
        {
            Disable();
        }
    }

}
