// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class EchoBot : ActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            //var testSSML = "<speak version=\"1\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xmlns:mstts=\"https://www.w3.org/2001/mstts\" xml:lang=\"en-US\"><voice name=\"en-US-JennyNeural\"><mstts:express-as style=\"assistant\"><prosody pitch=\"0%\" rate=\"0%\"><break time=\"1s\"/>Welcome to Contoso</prosody></mstts:express-as></voice></speak>
            //var testSSML = "<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xmlns:mstts=\"https://www.w3.org/2001/mstts\" xml:lang=\"en-US\"><voice name=\"en-US-JennyNeural\"><mstts:express-as style=\"assistant\"><prosody pitch=\"0%\" rate=\"0%\"><break time=\"1s\"/>Welcome to Contoso</prosody></mstts:express-as></voice></speak>";

            //var replyText = $"Echo: {turnContext.Activity.Text}";
            //var audioSSML1 = "<speak version=\"1.0\" xml:lang=\"en-US\" xmlns:mstts=\"http://www.w3.org/2001/mstts\">\r\n    <ssml:audio src=\"http://www.thewavsite.com/Birthday/bday11.wav\"/></speak>";
            //await turnContext.SendActivityAsync(MessageFactory.Text("ignore", testSSML));
            await SendActivitiesInParellel($"echo: {turnContext.Activity.Text}", turnContext);
        }

        private async Task SendActivitiesInParellel(string text, ITurnContext<IMessageActivity> turnContext)
        {
            var audioSSML = "<speak version=\"1.0\" xml:lang=\"en-US\" xmlns:mstts=\"http://www.w3.org/2001/mstts\">\r\n    <mstts:backgroundaudio src=\"http://www.thewavsite.com/Birthday/bday11.wav\" volume=\"0.7\" fadein=\"3000\" fadeout=\"4000\"/>\r\n    <voice name=\"en-US-JennyNeural\">\r\n        The text provided in this document will be spoken over the background audio.\r\n    </voice>\r\n</speak>";
            var task1 = turnContext.SendActivityAsync(MessageFactory.Text(text, text));
            var task2 = turnContext.SendActivityAsync(MessageFactory.Text("ignored", audioSSML));
            await Task.WhenAll(task1, task2);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
