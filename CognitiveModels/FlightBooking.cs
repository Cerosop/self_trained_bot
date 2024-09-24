// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Bot.Builder;
using Microsoft.BotBuilderSamples.Clu;
using Newtonsoft.Json;

namespace Microsoft.BotBuilderSamples
{
    /// <summary>
    /// An <see cref="IRecognizerConvert"/> implementation that provides helper methods and properties to interact with
    /// the CLU recognizer results.
    /// </summary>
    public class FlightBooking : IRecognizerConvert
    {
        public enum Intent
        {
            order,
            good,
            bad,
            None
        }

        public string Text { get; set; }

        public string AlteredText { get; set; }

        public Dictionary<Intent, IntentScore> Intents { get; set; }

        public CluEntities Entities { get; set; }

        public IDictionary<string, object> Properties { get; set; }

        public void Convert(dynamic result)
        {
            var jsonResult = JsonConvert.SerializeObject(result, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
            var app = JsonConvert.DeserializeObject<FlightBooking>(jsonResult);

            Text = app.Text;
            AlteredText = app.AlteredText;
            Intents = app.Intents;
            Entities = app.Entities;
            Properties = app.Properties;
        }

        public (Intent intent, double score) GetTopIntent()
        {
            var maxIntent = Intent.None;
            var max = 0.0;
            foreach (var entry in Intents)
            {
                if (entry.Value.Score > max)
                {
                    maxIntent = entry.Key;
                    max = entry.Value.Score.Value;
                }
            }

            return (maxIntent, max);
        }

        public class CluEntities
        {
            public CluEntity[] Entities;

            public CluEntity[] foodList() => Entities.Where(e => e.Category == "food").ToArray();

            public CluEntity[] goodList() => Entities.Where(e => e.Category == "goodEntity").ToArray();

            public CluEntity[] badList() => Entities.Where(e => e.Category == "badEntity").ToArray();

            public string food() => foodList().FirstOrDefault()?.Text;

            public string good() => goodList().FirstOrDefault()?.Text;

            public string bad() => badList().FirstOrDefault()?.Text;
        }
    }
}
