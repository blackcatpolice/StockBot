﻿using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using NSEBot.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NSEBot.Dialogs
{
    [Serializable]
    public class PriceCheckDialog : IDialog<string>
    {
        private int attempts = 3;
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Enter company code.");

            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            IStockService nseService = new StockService();

            var resp = await nseService.GetQuote(message.Text);

            var scrip = resp.quoteResponse.result.FirstOrDefault();


            context.Done($"Company : {scrip.longName}. Value : {scrip.regularMarketPrice.Fmt}. Change : {scrip.regularMarketChange.Fmt}");
        }
    }
}