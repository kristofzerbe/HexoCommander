using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace HexoCommander
{
    class Program
    {
        /*  HexoCommander
         *  =============
         *  Parameters:
         *  - /workdir:<Path>
         */

        static void Main(string[] args)
        {
            var opt = new Options(args);

            var _commandFile = "hexo-commands.txt";

            var _run_hexo_newpost       = "hexo new post \"@TITLE@\"";
            var _run_hexo_newdiscovery  = "hexo new discovery \"Discovery #@NUMBER@\"";
            var _run_hexo_newdraft      = "hexo new draft \"@TITLE@\"";
            var _run_hexo_postdraft     = "hexo publish @FILENAME@";
            var _run_hexo_clean         = "hexo clean";
            var _run_hexo_generate      = "hexo generate";
            var _run_git_stage          = "git add \"source/*\" \"docs/*\"";
            var _run_git_commit         = "git commit -m \"Remote @COMMAND@ via HexoCommander\"";
            var _run_git_push           = "git push origin master";

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
                    if (s.Trim().Length == 0 || s.StartsWith("//"))
                    {
                        lst.Add(s);
                    } 
                    else
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
                                case "NEWPOST": // parameter 1 expects title of draft
                                    cmds.Add(_run_hexo_newpost.Replace("@TITLE@", arr[1].Trim().Replace("\"", "")));
                                    RunCommands(cmds, opt.WorkDir);
                                    break;

                                case "NEWDISCOVERY": // parameter 1 expects number of Discovery issue
                                    cmds.Add(_run_hexo_newdiscovery.Replace("@NUMBER@", arr[1].Trim()));
                                    RunCommands(cmds, opt.WorkDir);
                                    break;

                                case "NEWDRAFT": // parameter 1 expects title of draft
                                    cmds.Add(_run_hexo_newdraft.Replace("@TITLE@", arr[1].Trim().Replace("\"", "")));
                                    RunCommands(cmds, opt.WorkDir);
                                    break;

                                case "POSTDRAFT": // parameter 1 expects filename of draft to publish
                                    cmds.Add(_run_hexo_postdraft.Replace("@FILENAME@", arr[1].Trim()));
                                    cmds.Add(_run_hexo_generate);
                                    RunCommands(cmds, opt.WorkDir);
                                    break;

                                case "REGENERATE":
                                    cmds.Add(_run_hexo_clean);
                                    cmds.Add(_run_hexo_generate);
                                    cmds.Add(_run_git_stage);
                                    cmds.Add(_run_git_commit.Replace("@COMMAND@", cmd));
                                    cmds.Add(_run_git_push);
                                    RunCommands(cmds, opt.WorkDir);
                                    break;

                                case "GENERATE":
                                    cmds.Add(_run_hexo_generate);
                                    cmds.Add(_run_git_stage);
                                    cmds.Add(_run_git_commit.Replace("@COMMAND@", cmd));
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
                }
                
                if (cmdCount == 0)
                {
                    Console.WriteLine("There was nothing to do...");
                } else
                {
                    //write file after processing all commands
                    File.WriteAllLines(fi.FullName, lst);
                }
            }

            if (Debugger.IsAttached)
            {
                Console.ReadLine();
            }
        }

        static void RunCommands(List<string> cmds, string workingDirectory)
        {
            //https://stackoverflow.com/questions/437419/execute-multiple-command-lines-with-the-same-process-using-net

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
