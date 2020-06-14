using Discord.Commands;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace traaittCurrencyService
{
    public partial class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("hashrate")]
        public async Task HashrateAsync([Remainder]string Remainder = "")
        {
            // Get last block header from daemon and calculate hashrate
            decimal Hashrate = 0;
            JObject Result = Request.RPC(traaittCurrencyService.daemonHost, traaittCurrencyService.daemonPort, "getlastblockheader");
            if (Result.Count > 0 && !Result.ContainsKey("error"))
                Hashrate = (decimal)Result["block_header"]["difficulty"] / 30;

            // Send reply
            await ReplyAsync("The current global hashrate is **" + traaittCurrencyService.FormatHashrate(Hashrate) + "**");
        }

        [Command("difficulty")]
        public async Task DifficultyAsync([Remainder]string Remainder = "")
        {
            // Get last block header from daemon and calculate hashrate
            decimal Difficulty = 0;
            JObject Result = Request.RPC(traaittCurrencyService.daemonHost, traaittCurrencyService.daemonPort, "getlastblockheader");
            if (Result.Count > 0 && !Result.ContainsKey("error"))
                Difficulty = (decimal)Result["block_header"]["difficulty"];

            // Send reply
            await ReplyAsync(string.Format("The current difficulty is **{0:N0}**", Difficulty));
        }

        [Command("height")]
        public async Task HeightAsync([Remainder]string Remainder = "")
        {
            // Get height
            decimal Height = 0;
            JObject Result = Request.GET("http://" + traaittCurrencyService.daemonHost + ":" + traaittCurrencyService.daemonPort + "/getinfo");
            if (Result.Count > 0 && !Result.ContainsKey("error"))
                Height = (decimal)Result["height"];


            // Send reply
            await ReplyAsync(string.Format("The current block height is **{0:N0}**", Height));
        }

        [Command("supply")]
        public async Task SupplyAsync([Remainder]string Remainder = "")
        {
            // Get supply
            decimal Supply = traaittCurrencyService.GetSupply();

            // Send reply
            await ReplyAsync(string.Format("The current circulating supply is **{0:N}** {1}", Supply, traaittCurrencyService.coinSymbol));
        }
    }
}
