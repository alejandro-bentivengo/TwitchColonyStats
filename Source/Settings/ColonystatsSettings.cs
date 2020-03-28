﻿using Colonystats.Utilities;
using System;
using UnityEngine;
using Verse;
namespace Colonystats
{
    class ColonyStatsSettings : ModSettings
    {
        public static string channel_username = "";
        public static string bot_username = "";
        public static string oauth_token = "";

        public static bool connectOnGameStartup = false;
        public static bool showWelcomeMessage = false;
        public static bool subsOnlyCommands = false;

        public void DoWindowContents(Rect inRect)
        {
            Rect channelDetails = new Rect(0f, verticalSpacing, inRect.width, 64f);
            Widgets.Label(channelDetails, TCText.BigText("Channel Details"));

            float sectionVertical = channelDetails.y + (verticalSpacing * 2f);

            Rect label = new Rect(0f, sectionVertical, 250f, verticalHeight);
            Widgets.Label(label, "Channel:");

            label.y += verticalSpacing;

            Widgets.Label(label, "Bot Username:");

            label.y += verticalSpacing;

            Widgets.Label(label, "OAuth Token:");

            Rect input = new Rect(250f, sectionVertical, 150f, verticalHeight);

            channel_username = Widgets.TextField(input, channel_username);

            input.y += verticalSpacing;

            bot_username = Widgets.TextField(input, bot_username);

            input.y += verticalSpacing;

            Rect oauthToggle = new Rect(input.x + input.width + 10f, input.y, 60f, verticalHeight);

            if (showOauth)
            {
                oauth_token = Widgets.TextField(input, oauth_token);

                if (Widgets.ButtonText(oauthToggle, "Hide")) showOauth = !showOauth;
            }
            else
            {
                Widgets.Label(input, new string('*', Math.Min(oauth_token.Length, 16)));

                if (Widgets.ButtonText(oauthToggle, "Show")) showOauth = !showOauth;
            }

            Rect newToken = new Rect(500f, input.y, 140f, verticalHeight);

            if (Widgets.ButtonText(newToken, "New OAuth Token")) Application.OpenURL("https://www.twitchapps.com/tmi/");

            input.y += verticalSpacing;

            if (Widgets.ButtonText(input, "Paste from Clipboard")) oauth_token = GUIUtility.systemCopyBuffer;

            // Connection

            Rect connectionDetails = new Rect(0f, input.y + (verticalSpacing * 2), inRect.width, 64f);
            Widgets.Label(connectionDetails, TCText.BigText("Connection"));

            sectionVertical = connectionDetails.y + (verticalSpacing * 2f);

            label.y = sectionVertical;
            input.y = sectionVertical;

            Widgets.Label(label, "Status:");

            Rect connectionButton = new Rect(input.x, input.y + verticalSpacing, input.width, verticalHeight);

            if (TwitchWrapper.Client.IsConnected)
            {
                Widgets.Label(input, TCText.ColoredText("Connected", Color.green));

                if (Widgets.ButtonText(connectionButton, "Disconnect")) TwitchWrapper.Client.Disconnect();
            }
            else
            {
                Widgets.Label(input, TCText.ColoredText("Not Connected", Color.red));

                if (Widgets.ButtonText(connectionButton, "Connect")) TwitchWrapper.StartAsync();
            }

            label.y += verticalSpacing * 2;
            float secondMenu = label.y;
            Widgets.Label(label, "Auto Connect on Startup:");
            input.y = label.y;
            Widgets.Checkbox(input.position, ref connectOnGameStartup);

            label.y += verticalSpacing * 1;
            Widgets.Label(label, "Show welcome message:");
            input.y = label.y;
            Widgets.Checkbox(input.position, ref showWelcomeMessage);

            Rect subsMenu = new Rect(350f, secondMenu, 250f, 50f);
            Widgets.Label(subsMenu, "Subs Only:");
            subsMenu.x += 150f;
            Widgets.Checkbox(subsMenu.position, ref subsOnlyCommands);

            Rect footer = new Rect(0f, inRect.height - 10f, inRect.width, 20f);

            Widgets.Label(footer, "Special thanks to hodldeeznuts!");

            footer.y += verticalSpacing * 1;

            Widgets.Label(footer, "Without him this wouldnt be possible!");

        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref channel_username, "channel_username", "", true);
            Scribe_Values.Look(ref bot_username, "bot_username", "", true);
            Scribe_Values.Look(ref oauth_token, "oauth_token", "", true);
            Scribe_Values.Look(ref connectOnGameStartup, "connectOnGameStartup", false);
            Scribe_Values.Look(ref showWelcomeMessage, "showWelcomeMessage", false);
            Scribe_Values.Look(ref subsOnlyCommands, "subsOnlyCommands", false);
        }

        bool showOauth = false;

        static readonly float verticalHeight = 32f;
        static readonly float verticalSpacing = 40f;
    }
}