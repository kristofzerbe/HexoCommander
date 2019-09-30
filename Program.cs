using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Text;

namespace HexoCommander
{
    class Program
    {
        /*  HexoCommander
         *  ============
         *  Parameters:
         *  - /workdir:<Path>
         */

        static void Main(string[] args)
        {
            var opt = new Options(args);

            var _commandFile = "hexo-commands.txt";

            var _run_hexo_generate  = "hexo generate";
            var _run_hexo_publish   = "hexo generate \"@title@\"";
            var _run_git_stage      = "git add \"docs/*\"";
            var _run_git_commit     = "git commit -m \"Remote publication via HexoCommander\"";
            var _run_git_push       = "git push origin master";

            var di = new DirectoryInfo(opt.WorkDir);
            var fis = di.GetFiles(_commandFile);

            if (fis.Length == 0)
            {
                Console.WriteLine(_commandFile + " not found");
            }
            else
            {
                var fi = fis[0];
                var lns = File.ReadAllLines(fi.FullName);
                var lst = new List<String>();
                var cmdCount = 0;

                foreach (string s in lns)
                {
                    var ln = s.Split('@');
                    if (ln.Length > 1) // already executed
                    { 
                        lst.Add(s);
                    }
                    else
                    {
                        //separate command from parameters by colon
                        var arr = s.Split(':');
                        var cmd = arr[0].Trim().ToUpper();

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("HexoCommander is running " + cmd);
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.White;

                        var cmds = new List<string>();

                        switch (cmd)
                        {
                            case "GENERATE":
                                cmds.Add(_run_hexo_generate);
                                RunCommands(cmds, opt.WorkDir);
                                break;

                            case "POSTDRAFT":
                                cmds.Add(_run_hexo_publish.Replace("@title@", arr[1])); // parameter 1 expects title to publish
                                cmds.Add(_run_hexo_generate);
                                RunCommands(cmds, opt.WorkDir);
                                break;

                            case "PUBLISH":
                                cmds.Add(_run_hexo_generate);
                                cmds.Add(_run_git_stage);
                                cmds.Add(_run_git_commit);
                                cmds.Add(_run_git_push);
                                RunCommands(cmds, opt.WorkDir);
                                break;

                            default:
                                Console.WriteLine("Unknown Command");
                                break;
                        }
                        lst.Add(s + " @ " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                        cmdCount += 1;
                    }
                }
                File.WriteAllLines(fi.FullName, lst);

                if (cmdCount == 0)
                {
                    Console.WriteLine("There was nothing to do...");
                }
            }

            // comment in for debugging
            Console.ReadLine();
        }

        static void RunCommands(List<string> cmds, string workingDirectory)
        {
            var proc = new Process();
            var psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                WorkingDirectory = workingDirectory,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };
            proc.StartInfo = psi;
            proc.Start();

            proc.OutputDataReceived += (sender, e) => { Console.WriteLine(e.Data); };
            proc.ErrorDataReceived += (sender, e) => { Console.WriteLine(e.Data); };
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();

            using (StreamWriter sw = proc.StandardInput)
            {
                foreach (var cmd in cmds)
                {
                    sw.WriteLine(cmd);
                }
            }
            proc.WaitForExit();
        }

    }
}
