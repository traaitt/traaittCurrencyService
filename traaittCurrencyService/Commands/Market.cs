using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace traaittCurrencyService
{
    public partial class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("price")]
        public async Task PriceAsync([Remainder]string Remainder = "")
        {
            // Get current coin price
            JObject CoinPrice = Request.GET(traaittCurrencyService.marketEndpoint);
            if (CoinPrice.Count < 1)
            {
                await ReplyAsync("Failed to connect to " + traaittCurrencyService.marketSource);
                return;
            }

            // Get current BTC price
            JObject BTCPrice = Request.GET(traaittCurrencyService.marketBTCEndpoint);
            if (BTCPrice.Count < 1)
            {
                await ReplyAsync("Failed to connect to " + traaittCurrencyService.marketBTCEndpoint);
                return;
            }

            // Begin building a response
            var Response = new EmbedBuilder();
            Response.WithTitle("Current Price of TRTL: " + traaittCurrencyService.marketSource);
            Response.WithUrl(traaittCurrencyService.marketEndpoint);
            Response.AddInlineField("Low", string.Format("{0} sats", Math.Round((decimal)CoinPrice["low"] * 100000000)));
            Response.AddInlineField("Current", string.Format("{0} sats", Math.Round((decimal)CoinPrice["price"] * 100000000)));
            Response.AddInlineField("High", string.Format("{0} sats", Math.Round((decimal)CoinPrice["high"] * 100000000)));
            Response.AddInlineField(traaittCurrencyService.coinSymbol + "-USD", string.Format("${0:N5} USD", (decimal)CoinPrice["price"] * (decimal)BTCPrice["last"]));
            Response.AddInlineField("Volume", string.Format("{0:N} BTC", (decimal)CoinPrice["volume"]));
            Response.AddInlineField("BTC-USD", string.Format("{0:C} USD", (decimal)BTCPrice["last"]));

            // Send reply
            if (Context.Guild != null && traaittCurrencyService.marketDisallowedServers.Contains(Context.Guild.Id))
            {
                try { await Context.Message.DeleteAsync(); }
                catch { }
                await Context.Message.Author.SendMessageAsync("", false, Response);
            }
            else await ReplyAsync("", false, Response);
        }

        [Command("mcap")]
        public async Task MarketCapAsync([Remainder]string Remainder = "")
        {
            // Get current coin price
            JObject CoinPrice = Request.GET(traaittCurrencyService.marketEndpoint);
            if (CoinPrice.Count < 1)
            {
                await ReplyAsync("Failed to connect to " + traaittCurrencyService.marketSource);
                return;
            }

            // Get current BTC price
            JObject BTCPrice = Request.GET(traaittCurrencyService.marketBTCEndpoint);
            if (BTCPrice.Count < 1)
            {
                await ReplyAsync("Failed to connect to " + traaittCurrencyService.marketBTCEndpoint);
                return;
            }

            // Begin building a response
            string Response = string.Format("{0}'s market cap is **{1:c}** USD", traaittCurrencyService.coinName,
                (decimal)CoinPrice["price"] * (decimal)BTCPrice["last"] * traaittCurrencyService.GetSupply());

            // Send reply
            if (Context.Guild != null && traaittCurrencyService.marketDisallowedServers.Contains(Context.Guild.Id))
            {
                try { await Context.Message.DeleteAsync(); }
                catch { }
                await Context.Message.Author.SendMessageAsync(Response);
            }
            else await ReplyAsync(Response);
        }
    }
}
