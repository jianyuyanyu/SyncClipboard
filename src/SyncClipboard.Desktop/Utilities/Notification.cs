﻿using SyncClipboard.Abstract.Notification;
using SyncClipboard.Core.Utilities.Fake;
using System;

namespace SyncClipboard.Desktop.Utilities;

internal class Notification : INotification
{
    public IProgressBar CreateProgressNotification(string title)
    {
        return new FakeProgressBar();
    }

    public void SendTemporary(NotificationPara para)
    {
    }
    public void SendImage(string title, string text, Uri uri, params Button[] buttons)
    {
    }

    public void SendText(string title, string text, params Button[] buttons)
    {
    }
}
