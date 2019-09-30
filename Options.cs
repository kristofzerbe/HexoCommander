using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexoCommander
{
    public class Options
    {
        public Options(string[] args) {
            foreach (string a in args)
            {
                var arg = a.Split('=');
                switch (arg[0].ToUpper())
                {
                    case "/WORKDIR":
                        WorkDir = arg[1];
                        break;
                }
            }
        }

        public string WorkDir { get; set; } = Environment.CurrentDirectory;
    }
}
