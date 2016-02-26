namespace ElasticSearchMSBand
{
    using System;

    public class Constants
    {
        // free hub public const string ConnectionString = "HostName=HckthnESBand.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=pKMn+3nw38PdcfoTHMIeY/HwutiuSJKsJ6MciG+i4g4=";
        public const string ConnectionString = "HostName=HckthnESBandS1.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=GNJMLRz7/h2MpRBgu+VtDiZUjsgo+3YGfXKjoCvjcKo=";

        public static int NumDevices = 10000;
        public static TimeSpan SimulationDelay = TimeSpan.FromSeconds(180);
    }
}
