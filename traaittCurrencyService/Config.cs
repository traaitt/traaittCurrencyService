using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace traaittCurrencyService
{
    partial class traaittCurrencyService
    {
        // Loads config values from file
        public static async Task LoadConfig()
        {
            if (!File.Exists(configFile))
            {
                await SaveConfig();
            }
            else
            {
                JObject configJson = JObject.Parse(File.ReadAllText(configFile));

                databaseFile = (string)configJson["databaseFile"];
                logLevel = (int)configJson["logLevel"];
                botToken = (string)configJson["botToken"];
                botPrefix = (string)configJson["botPrefix"];
                botMessageCache = (int)configJson["botMessageCache"];
                coinName = (string)configJson["coinName"];
                coinSymbol = (string)configJson["coinSymbol"];
                coinUnits = (decimal)configJson["coinUnits"];
                coinAddressLength = (int)configJson["coinAddressLength"];
                coinAddressPrefix = (string)configJson["coinAddressPrefix"];
                tipFee = (decimal)configJson["tipFee"];
                tipMixin = (int)configJson["tipMixin"];
                tipSuccessReact = (string)configJson["tipSuccessReact"];
                tipFailedReact = (string)configJson["tipFailedReact"];
                tipLowBalanceReact = (string)configJson["tipLowBalanceReact"];
                tipJoinReact = (string)configJson["tipJoinReact"];
                tipCustomReacts = configJson["tipCustomReacts"].ToObject<Dictionary<string, decimal>>();
                faucetHost = (string)configJson["faucetHost"];
                faucetEndpoint = (string)configJson["faucetEndpoint"];
                faucetAddress = (string)configJson["faucetAddress"];
                marketSource = (string)configJson["marketSource"];
                marketEndpoint = (string)configJson["marketEndpoint"];
                marketBTCEndpoint = (string)configJson["marketBTCEndpoint"];
                marketDisallowedServers = configJson["marketDisallowedServers"].ToObject<List<ulong>>();
                daemonHost = (string)configJson["daemonHost"];
                daemonPort = (int)configJson["daemonPort"];
                walletHost = (string)configJson["walletHost"];
                walletPort = (int)configJson["walletPort"];
                walletRpcPassword = (string)configJson["walletRpcPassword"];
                walletUpdateDelay = (int)configJson["walletUpdateDelay"];
            }
        }

        // Saves config values to file
        public static async Task SaveConfig()
        {
            JObject configJson = new JObject
            {
                ["databaseFile"] = databaseFile,
                ["logLevel"] = logLevel,
                ["botToken"] = botToken,
                ["botPrefix"] = botPrefix,
                ["botMessageCache"] = botMessageCache,
                ["coinName"] = coinName,
                ["coinSymbol"] = coinSymbol,
                ["coinUnits"] = coinUnits,
                ["coinAddressLength"] = coinAddressLength,
                ["coinAddressPrefix"] = coinAddressPrefix,
                ["tipFee"] = tipFee,
                ["tipMixin"] = tipMixin,
                ["tipSuccessReact"] = tipSuccessReact,
                ["tipFailedReact"] = tipFailedReact,
                ["tipLowBalanceReact"] = tipLowBalanceReact,
                ["tipJoinReact"] = tipJoinReact,
                ["tipCustomReacts"] = JToken.FromObject(tipCustomReacts),
                ["faucetHost"] = faucetHost,
                ["faucetEndpoint"] = faucetEndpoint,
                ["faucetAddress"] = faucetAddress,
                ["marketSource"] = marketSource,
                ["marketEndpoint"] = marketEndpoint,
                ["marketBTCEndpoint"] = marketBTCEndpoint,
                ["marketDisallowedServers"] = JToken.FromObject(marketDisallowedServers),
                ["daemonHost"] = daemonHost,
                ["daemonPort"] = daemonPort,
                ["walletHost"] = walletHost,
                ["walletPort"] = walletPort,
                ["walletRpcPassword"] = walletRpcPassword,
                ["walletUpdateDelay"] = walletUpdateDelay
            };

            await File.WriteAllTextAsync(configFile, configJson.ToString());
        }
    }
}
