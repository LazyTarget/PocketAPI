using System;

namespace PocketAPI
{
    public class ConfirmEventArgs : EventArgs
    {
        public ConfirmEventArgs(string confirmUrl, Action callback)
        {
            ConfirmUrl = confirmUrl;
            OnConfirm = callback;
        }

        public string ConfirmUrl { get; private set; }

        public Action OnConfirm { get; private set; }
    }
}