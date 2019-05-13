using System;

namespace StreamlabsEventReceiver
{
    public class StreamlabsEventArgs : EventArgs
    {
        public object Data { get; private set; }

        public StreamlabsEventArgs(object data)
        {
            Data = data;
        }
    }
}
