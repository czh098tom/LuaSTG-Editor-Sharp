namespace LuaSTGEditorSharp.Core.Cli
{
    public class ArgsGrouper
    {
        public IReadOnlyList<string> MainParams => mainParams;
        public IReadOnlyDictionary<string, IReadOnlyList<string>> AdditionalParams => additionalParams;

        private readonly string[] mainParams;
        private readonly Dictionary<string, IReadOnlyList<string>> additionalParams;

        public ArgsGrouper(string[] args)
        {
            mainParams = Array.Empty<string>();
            additionalParams = new();
            int i = 0;
            string? paramType = null;
            List<string> param = new();
            while (i < args.Length)
            {
                if (paramType != null)
                {
                    if (!args[i].StartsWith("-"))
                    {
                        param.Add(args[i]);
                    }
                    else
                    {
                        additionalParams.Add(paramType, param);
                        param = new();
                        paramType = args[i].Substring(1);
                    }
                }
                else
                {
                    if (!args[i].StartsWith("-"))
                    {
                        param.Add(args[i]);
                    }
                    else
                    {
                        mainParams = param.ToArray();
                        param = new();
                        paramType = args[i].Substring(1);
                    }
                }
                i++;
            }
            if (paramType != null)
            {
                additionalParams.Add(paramType, param);
            }
            else
            {
                mainParams = param.ToArray();
            }
        }
    }
}
