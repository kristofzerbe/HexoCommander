using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace HexoCommander
{
    class Program
    {
        static void Main(string[] args)
        {
            var _commandFile = "hexo-commands.txt";

            /*  AVAILABLE COMMANDS:
             *  -------------------
             *  generate 
             *  => generate only
             *  
             *  publish 
             *  => generate, stage docs, commit & push
             *  
             *  PLANNED COMMANDS:
             *  -----------------
             *  publish-draft "<filename>"  
             *  => publish draft       
             * 
             */

            var _cmd_hexo_generate  = "hexo generate";
            var _cmd_git_stage      = "git add \"docs/*\"";
            var _cmd_git_commit     = "git commit -m \"Remote publication via HexoCommander\"";
            var _cmd_git_push       = "git push origin master";

            var fi = new FileInfo(_commandFile);
            if (fi.Exists)
            {
                var cmds = File.ReadAllLines(fi.FullName);
                var lst = new List<String>();
                foreach (string s in cmds)
                {
                    var cmd = s.Split(':');
                    if (cmd.Length > 1)
                    {
                        lst.Add(s);
                    }
                    else
                    {
                        Console.WriteLine("Do Command " + s.ToUpper());

                        switch (s.ToUpper())
                        {
                            case "GENERATE":
                                Console.WriteLine(RunCommand(_cmd_hexo_generate));
                                break;

                            case "PUBLISH":
                                Console.WriteLine(RunCommand(_cmd_hexo_generate));
                                Console.WriteLine(RunCommand(_cmd_git_stage));
                                Console.WriteLine(RunCommand(_cmd_git_commit));
                                Console.WriteLine(RunCommand(_cmd_git_push));
                                break;
                            default:
                                Console.WriteLine("Unknown Command");
                                break;
                        }
                        lst.Add(s + ": " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                }
                File.WriteAllLines(fi.FullName, lst);
            }
            else {
                Console.WriteLine(_commandFile + " not found");
            }

            // comment out for release
            //Console.ReadLine();
        }

        private static string RunCommand(string cmd)
        {
            Process proc = new Process();

            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.UseShellExecute = false;

            proc.Start();

            proc.StandardInput.WriteLine(cmd);
            proc.StandardInput.Flush();
            proc.StandardInput.Close();

            var ret = proc.StandardOutput.ReadToEnd();

            proc.Close();

            return ret;
        }
    }
}
