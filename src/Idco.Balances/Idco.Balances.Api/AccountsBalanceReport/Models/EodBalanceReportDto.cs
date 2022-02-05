namespace Idco.Balances.Api.AccountBalanceReport
{
    using Idco.Balances.Utility.Serialization;
    using Newtonsoft.Json;
    using System;

    public class EodBalanceReportDto
    {
        [JsonConverter(typeof(OnlyDateConverter))]
        public DateTime Date { get; set; }
        public long Balance { get; set; }

        public EodBalanceReportDto()
        { }
    }
}