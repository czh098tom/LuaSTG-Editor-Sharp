using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace LuaSTGEditorSharp.Execution
{
    public delegate void Logger(string s);

    public abstract class LSTGExecution
    {
        protected Process LSTGInstance { get; set; }

        protected string Parameter { get; set; }

        protected bool UseShellExecute { get; set; }

        protected bool CreateNoWindow { get; set; }

        protected bool RedirectStandardError { get; set; }

        protected bool RedirectStandardOutput { get; set; }

        protected virtual string WorkingDirectory { get => Path.GetDirectoryName((App.Current as App)?.LuaSTGExecutablePath); }

        protected virtual string LuaSTGPath { get => (App.Current as App)?.LuaSTGExecutablePath; }

        protected abstract string LogFileName { get; }
        public abstract string ExecutableName { get; }

        public abstract void BeforeRun(ExecutionConfig config);

        public virtual void Run(Logger logger, Action end)
        {
            if (LSTGInstance == null || LSTGInstance.HasExited)
            {
                LSTGInstance = new Process
                {
                    StartInfo = new ProcessStartInfo(LuaSTGPath, Parameter)
                    {
                        UseShellExecute = UseShellExecute,
                        CreateNoWindow = CreateNoWindow,
                        WorkingDirectory = WorkingDirectory,
                        RedirectStandardError = RedirectStandardError,
                        RedirectStandardOutput = RedirectStandardOutput
                    }
                };
                LSTGInstance.Start();
                logger("LuaSTG is Running.\n\n");
                /* 
                 * what it should be like:
                 * 
                lstg.OutputDataReceived += (s, e) => DebugString += e.Data;
                lstg.ErrorDataReceived += (s, e) => DebugString += e.Data;
                 *
                 * what it actually is:
                 */
                LSTGInstance.Exited += (s, e) => {
                    FileStream fs = null;
                    StreamReader sr = null;
                    try
                    {
                        fs = new FileStream(Path.GetFullPath(Path.Combine(
                            Path.GetDirectoryName(LuaSTGPath), LogFileName)), FileMode.Open);
                        sr = new StreamReader(fs);
                        int i = 0;
                        while (!sr.EndOfStream && i < 8192)
                        {
                            logger(sr.ReadLine());
                            i++;
                        }
                        //logger(sr.ReadToEnd());
                        end();
                    }
                    catch (System.Exception exc)
                    {
                        System.Windows.MessageBox.Show(exc.ToString());
                    }
                    finally
                    {
                        if (fs != null) fs.Close();
                        if (sr != null) sr.Close();
                    }
                    logger("\nExited with code " + LSTGInstance.ExitCode + ".");
                };
                LSTGInstance.EnableRaisingEvents = true;
                LSTGInstance.BeginOutputReadLine();
                LSTGInstance.BeginErrorReadLine();
                //lstg.WaitForExit();
            }
            else
            {
                MessageBox.Show("LuaSTG is already running, please exit first."
                    , "LuaSTG Editor Sharp", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
