using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace StreamlabsEventReceiver
{
    public class StreamlabsEvent<T> where T : StreamlabsEventMessage
    {
        [JsonProperty("type")]
        public string Type { get; protected set; }

        [JsonProperty("for")]
        public string For { get; protected set; }

        [JsonProperty("message")]
        public List<T> Message { get; protected set; }
    }

    public class StreamlabsEventMessage
    {
        //[JsonProperty("_id")]
        //public string _Id { get; protected set; }

        //[JsonProperty("event_id")]
        //public string EventId { get; protected set; }

        [JsonProperty("name")]
        public string Name { get; protected set; }

        [JsonProperty("isTest")] // Defaults to False when not present
        public bool IsTest { get; protected set; }

        [JsonProperty("repeat")] // Defaults to False when not present
        public bool IsRepeat { get; protected set; }

        // Custom field for real/live event indication
        public bool IsLive
        {
            get
            {
                return !IsTest && !IsRepeat;
            }
        }
    }

    #region Streamlabs Events

    public class Donation : StreamlabsEventMessage
    {
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; private set; }

        [JsonProperty("amount")]
        public double Amount { get; private set; }

        [JsonProperty("currency")]
        public string Currency { get; private set; }

        //[JsonProperty("donationCurrency")]
        //[JsonProperty("donation_currency")]
        //public string DonationCurrency { get; private set; }

        [JsonProperty("formatted_amount")]
        public string FormattedAmount { get; private set; }

        [JsonProperty("message")]
        public string Message { get; private set; }

        [JsonProperty("from")]
        public string FromName { get; private set; }

        [JsonProperty("fromId")]
        public string FromId { get; private set; }

        //[JsonProperty("to")]
        //public Dictionary<string, string> To { get; private set; }

        //[JsonProperty("emotes")]
        //public string Emotes { get; protected set; }

        [JsonProperty("source")]
        public string PaymentSource { get; private set; }

        // from_user_id
        // id
        // iconClassName

    }

    public class FaceMask
    {

    }

    public class Merch
    {

    }

    #endregion

    #region Twitch Events

    public class TwitchFollow : StreamlabsEventMessage
    {
        [JsonProperty("id")]
        public string Id { get; protected set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; private set; }
    }

    public class TwitchSubscription : StreamlabsEventMessage
    {
        [JsonProperty("display_name")]
        public string DisplayName { get; protected set; }

        [JsonProperty("subscriber_twitch_id")]
        public string TwitchId { get; protected set; }

        [JsonProperty("gifter")]
        public string Gifter { get; protected set; }

        [JsonProperty("gifter_display_name")]
        public string GifterDisplayName { get; protected set; }

        [JsonProperty("gifter_twitch_id")]
        public string GifterTwitchId { get; protected set; }

        [JsonProperty("message")]
        public string Message { get; protected set; }

        [JsonProperty("months")]
        public int Months { get; protected set; }

        [JsonProperty("streak_months")]
        public int? StreakMonths { get; protected set; }

        [JsonProperty("sub_plan")]
        public string SubPlan { get; protected set; }

        [JsonProperty("sub_plan_name")]
        public string SubPlanName { get; protected set; }

        [JsonProperty("sub_type")]
        public string SubType { get; protected set; }

        //[JsonProperty("emotes")]
        //public string Emotes { get; protected set; }

        // Replay Sub Event Fix
        [JsonProperty("subPlan")]
        private string SubPlanFix { set { SubPlan = value; } }

        [JsonProperty("planName")]
        private string SubPlanNameFix { set { SubPlanName = value; } }

        [JsonProperty("type")]
        private string SubTypeFix
        {
            set
            {
                SubType = value;
            }
        }
    }

    public class TwitchMysterySubscription : StreamlabsEventMessage
    {
        [JsonProperty("amount")]
        public int Amount { get; protected set; }

        [JsonProperty("gifter")]
        public string Gifter { get; protected set; }

        [JsonProperty("gifter_display_name")]
        public string GifterDisplayName { get; protected set; }

        [JsonProperty("sub_plan")]
        public string SubPlan { get; protected set; }

        [JsonProperty("sub_type")]
        public string SubType { get; protected set; }
    }

    public class TwitchCheer : StreamlabsEventMessage
    {
        //[JsonProperty("id")]
        //public string Id { get; protected set; }

        [JsonProperty("message")]
        public string Message { get; protected set; }

        [JsonProperty("amount")]
        public int Amount { get; private set; }

        //[JsonProperty("emotes")]
        //public string Emotes { get; protected set; }
    }

    public class TwitchHost : StreamlabsEventMessage
    {
        //[JsonProperty("type")]
        //public string Type { get; private set; }

        [JsonProperty("viewers")]
        public string Viewers { get; private set; }
    }

    public class TwitchRaid : StreamlabsEventMessage
    {
        [JsonProperty("raiders")]
        public int Raiders { get; private set; }
    }

    #endregion

    #region Mixer Events

    public class MixerFollow : StreamlabsEventMessage
    {
        //[JsonProperty("id")]
        //public string Id { get; protected set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; private set; }
    }

    public class MixerSubscription : StreamlabsEventMessage
    {
        //[JsonProperty("id")]
        //public string Id { get; protected set; }

        [JsonProperty("months")]
        public int Months { get; protected set; }

        [JsonProperty("message")]
        public string Message { get; protected set; }

        [JsonProperty("since")]
        public DateTime CreatedAt { get; protected set; }

        //[JsonProperty("emotes")]
        //public string Emotes { get; protected set; }
    }

    public class MixerHost : StreamlabsEventMessage
    {
        //[JsonProperty("type")]
        //public string Type { get; private set; }

        [JsonProperty("viewers")]
        public string Viewers { get; private set; }
    }

    #endregion

    #region Youtube Events

    public class YoutubeSubscription : StreamlabsEventMessage
    {
        [JsonProperty("id")]
        public string Id { get; protected set; }

        [JsonProperty("publishedAt")]
        public DateTime CreatedAt { get; private set; }
    }

    public class YoutubeSponsor : StreamlabsEventMessage
    {
        [JsonProperty("id")]
        public string Id { get; protected set; }

        [JsonProperty("sponsorSince")]
        public DateTime SponsorSince { get; private set; }

        [JsonProperty("channelUrl")]
        public string ChannelUrl { get; private set; }

        [JsonProperty("months")]
        public int Months { get; protected set; }
    }

    public class YoutubeSuperchat : StreamlabsEventMessage
    {
        [JsonProperty("id")]
        public string Id { get; private set; }

        [JsonProperty("channelId")]
        public string ChannelId { get; private set; }

        [JsonProperty("channelUrl")]
        public string ChannelUrl { get; private set; }

        [JsonProperty("comment")]
        public string Message { get; private set; }

        [JsonProperty("amount")]
        public double Amount { get; private set; }

        [JsonProperty("currency")]
        public string Currency { get; private set; }

        [JsonProperty("displayString")]
        public string FormattedAmount { get; private set; }

        //[JsonProperty("messageType")]
        //public int MessageType { get; protected set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; private set; }
    }

    #endregion
}