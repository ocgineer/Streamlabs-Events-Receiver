using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;
//using Newtonsoft.Json;
using Quobject.SocketIoClientDotNet.Client;
using SocketIO = Quobject.SocketIoClientDotNet.Client.Socket;
using ImmutableList = Quobject.Collections.Immutable.ImmutableList;

/*
// Set JSON seralization settings
//jSettings = new JsonSerializerSettings
//{
//    Formatting = Formatting.None,
//    ContractResolver = new RealPropertyNameContractResolver()
//    // Using Actual .Net property names for json string
//};
// Not as Global // deseralization issues
//JsonConvert.DefaultSettings = () =>
//{
//    return new JsonSerializerSettings
//    {
//        Formatting = Formatting.None,
//        ContractResolver = new RealPropertyNameContractResolver()
//        // Using Actual .Net property names for json string
//    };
//};
*/

namespace StreamlabsEventReceiver
{
    public class StreamlabsEventClient
    {
        #region Fields

        private readonly Uri _uri;
        private IO.Options _opt;
        private SocketIO _sio;

        //private readonly JsonSerializerSettings jSettings;

        #endregion

        #region Event Delegates

        public event EventHandler<EventArgs> StreamlabsSocketConnected;
        public event EventHandler<EventArgs> StreamlabsSocketDisconnected;
        public event EventHandler<StreamlabsEventArgs> StreamlabsSocketEvent;

        #endregion

        #region Constructors

        public StreamlabsEventClient()
        {            
            // Set SocketIO URL
            _uri = new Uri("https://sockets.streamlabs.com");
        }

        #endregion

        #region Public Methods

        public void Connect(string token)
        {
            // Set options
            _opt = new IO.Options
            {
                Transports = ImmutableList.Create("websocket"),
                AutoConnect = false,
                Query = new Dictionary<string, string>
                {
                    { "token", token }
                }
            };

            // Socket IO with URI and Options
            _sio = IO.Socket(_uri, _opt);
           
            // Attach SocketIO events
            _sio.On(SocketIO.EVENT_CONNECT, () => SocketConnected());
            _sio.On(SocketIO.EVENT_DISCONNECT, () => SocketDisconnected());
            _sio.On("event", (data) => SocketEvent(data));

            // Connect
            _sio.Connect();
        }

        public void Disconnect()
        {
            _sio.Disconnect();
        }

        #endregion

        #region Private Methods

        private void SocketConnected()
        {
            StreamlabsSocketConnected?.Invoke(this, null);
            IsConnected = true;
        }

        private void SocketDisconnected()
        {
            StreamlabsSocketDisconnected?.Invoke(this, null);
            IsConnected = false;
        }

        private void SocketEvent(object data)
        {
            try
            {
                // Cast received data to JObject and then to modal object
                JObject jData = (JObject)data;
                object obj = null;
                
                // Check if for and type exists
                bool evt_for_exists = jData.TryGetValue("for", out JToken evt_for);
                bool evt_type_exists = jData.TryGetValue("type", out JToken evt_type);

                // Events
                if (evt_for_exists && evt_type_exists)
                {
                    
                    switch (jData.Value<string>("for"))
                    {
                        case "twitch_account":
                            
                            switch (jData.Value<string>("type"))
                            {
                                case "follow":
                                    obj = jData.ToObject<StreamlabsEvent<TwitchFollow>>();
                                    break;

                                case "subMysteryGift":
                                    obj = jData.ToObject<StreamlabsEvent<TwitchMysterySubscription>>();
                                    break;

                                case "subscription":
                                case "resub":
                                    obj = jData.ToObject<StreamlabsEvent<TwitchSubscription>>();
                                    break;

                                case "bits":
                                    obj = jData.ToObject<StreamlabsEvent<TwitchCheer>>();
                                    break;

                                case "host":
                                    obj = jData.ToObject<StreamlabsEvent<TwitchHost>>();
                                    break;

                                case "raid":
                                    obj = jData.ToObject<StreamlabsEvent<TwitchRaid>>();
                                    break;
                            }

                            break;
                            // End of twitch_account for

                        case "mixer_account":

                            switch (jData.Value<string>("type"))
                            {
                                case "follow":
                                    obj = jData.ToObject<StreamlabsEvent<MixerFollow>>();
                                    break;

                                case "subscription":
                                    obj = jData.ToObject<StreamlabsEvent<MixerSubscription>>();
                                    break;

                                case "host":
                                    obj = jData.ToObject<StreamlabsEvent<MixerHost>>();
                                    break;
                            }

                            break;
                            // End of mixer_account for

                        case "youtube_account":

                            switch (jData.Value<string>("type"))
                            {
                                case "follow":
                                    obj = jData.ToObject<StreamlabsEvent<YoutubeSubscription>>();
                                    break;

                                case "subscription":
                                    obj = jData.ToObject<StreamlabsEvent<YoutubeSponsor>>();
                                    break;

                                case "superchat":
                                    obj = jData.ToObject<StreamlabsEvent<YoutubeSuperchat>>();
                                    break;
                            }

                            break;
                            // End of youtube_account for

                        // data.for == streamlabs
                        case "streamlabs":

                            switch (jData.Value<string>("type"))
                            {
                                case "donation":
                                    obj = jData.ToObject<StreamlabsEvent<Donation>>();
                                    break;
                            }

                            break;
                            // End of streamlabs for
                    }
                }
                // Real donation Event fix
                else if (!evt_for_exists && (evt_type_exists && jData.Value<string>("type") == "donation"))
                {
                    // Manually set for to 'streamlabs' to have both test, replay and real be under streamlabs
                    jData["for"] = "streamlabs";
                    obj = jData.ToObject<StreamlabsEvent<Donation>>();
                }

                if (obj != null)
                {
                    StreamlabsSocketEvent?.Invoke(this, new StreamlabsEventArgs(obj));
                }
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine(data.ToString());
                Console.WriteLine(e.ToString());
#endif
            }
        }

        #endregion

        public bool IsConnected
        {
            get; private set;
        }

    }
}
