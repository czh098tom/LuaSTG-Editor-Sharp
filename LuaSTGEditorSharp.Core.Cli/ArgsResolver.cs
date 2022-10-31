namespace LuaSTGEditorSharp.Core.Cli
{
    public class ArgsResolver
    {
        public bool IsHelp { get; private set; }

        public string? File { get; private set; }

        public string? OutputDirectory { get; private set; }

        public string? OutputFilename { get; private set; }

        public string? Plugin { get; private set; }

        public ArgsResolver(ArgsGrouper grouper)
        {
            if (grouper.MainParams.Count > 0)
            {
                File = grouper.MainParams[0];
            }
            if (grouper.AdditionalParams.ContainsKey("h"))
            {
                IsHelp = true;
            }
            if (grouper.AdditionalParams.ContainsKey("d"))
            {
                if (grouper.AdditionalParams["d"].Count > 0)
                {
                    OutputDirectory = grouper.AdditionalParams["d"][0];
                }
                else
                {
                    throw new ArgumentException($"Insufficient argument for \"{nameof(OutputDirectory)}\".");
                }
            }
            if (grouper.AdditionalParams.ContainsKey("n"))
            {
                if (grouper.AdditionalParams["n"].Count > 0)
                {
                    OutputFilename = grouper.AdditionalParams["n"][0];
                }
                else
                {
                    throw new ArgumentException($"Insufficient argument for \"{nameof(OutputFilename)}\".");
                }
            }
            if (grouper.AdditionalParams.ContainsKey("p"))
            {
                if (grouper.AdditionalParams["p"].Count > 0)
                {
                    Plugin = grouper.AdditionalParams["p"][0];
                }
                else
                {
                    throw new ArgumentException($"Insufficient argument for \"{nameof(Plugin)}\".");
                }
            }
        }
    }
}
