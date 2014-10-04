using System;

namespace PocketAPI
{
    public class ConfirmEventArgs : EventArgs
    {
        public ConfirmEventArgs(string confirmUrl, System.Action callback)
        {
            ConfirmUrl = confirmUrl;
            OnConfirm = callback;
        }

        public string ConfirmUrl { get; private set; }

        public System.Action OnConfirm { get; private set; }
    }
}