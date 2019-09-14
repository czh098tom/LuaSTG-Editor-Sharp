using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Security.AccessControl;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Compile;
using LuaSTGEditorSharp.EditorData.Exception;
using LuaSTGEditorSharp.Zip;

namespace LuaSTGEditorSharp.EditorData
{
    /// <summary>
    /// Base class for all type of compile process.
    /// </summary>
    public abstract class CompileProcess
    {
        /// <summary>
        /// The source <see cref="DocumentData"/> of the process.
        /// </summary>
        public DocumentData source;

        /// <summary>
        /// The dictionary of archive path to resources path.
        /// </summary>
        public Dictionary<string, string> resourceFilePath = new Dictionary<string, string>();

        /// <summary>
        /// The macro definitions.
        /// </summary>
        public List<DefineMarcoSettings> marcoDefinition = new List<DefineMarcoSettings>();

        /// <summary>
        /// The namespace for given objects.
        /// </summary>
        public string NameSpace = "editor";

        /// <summary>
        /// The base directory of archive.
        /// </summary>
        public string archiveSpace = "";

        /// <summary>
        /// Current temporary path for script coding cache.
        /// </summary>
        public string currentTempPath;
        /// <summary>
        /// Current temporary path for _editor_output.lua.
        /// </summary>
        public string projLuaPath;
        /// <summary>
        /// Current temporary path for root.lua.
        /// </summary>
        public string rootLuaPath;
        /// <summary>
        /// Current temporary path for pack batch.
        /// </summary>
        public string rootZipPackPath;

        /// <summary>
        /// Current path of luastg file.
        /// </summary>
        public string projPath;
        /// <summary>
        /// Current path of meta of resource of luastg file.
        /// </summary>
        public string projMetaPath;

        /// <summary>
        /// Code for root.lua.
        /// </summary>
        public string rootCode;

        /// <summary>
        /// Path for 7z executable.
        /// </summary>
        public string zipExePath;
        /// <summary>
        /// Path for LuaSTG executable.
        /// </summary>
        public string luaSTGExePath;

        /// <summary>
        /// Name of the target mod.
        /// </summary>
        public string projName;

        /// <summary>
        /// Path for LuaSTG root.
        /// </summary>
        public string luaSTGFolder;
        /// <summary>
        /// Path for target mod folder.
        /// </summary>
        public string targetZipPath;
        /// <summary>
        /// Store the method when progress changed.
        /// </summary>
        public ProgressChangedEventHandler ProgressChangedEventHandler { get; private set; }
        /// <summary>
        /// Event on progress changed.
        /// </summary>
        private event ProgressChangedEventHandler ProgressChanged_Private;
        /// <summary>
        /// Public setter of both event and method on progress changed.
        /// </summary>
        public event ProgressChangedEventHandler ProgressChanged
        {
            add
            {
                ProgressChanged_Private += value;
                ProgressChangedEventHandler = new ProgressChangedEventHandler(value);
            }
            remove
            {
                ProgressChanged_Private -= value;
                ProgressChangedEventHandler = null;
            }
        }

        /// <summary>
        /// This method get the MD5 Hash from a given file.
        /// </summary>
        /// <param name="filePath">The target path.</param>
        /// <returns>MD5 Hash.</returns>
        public static string GetMD5HashFromFile(string filePath)
        {
            FileStream file = null;
            try
            {
                file = new FileStream(filePath, FileMode.Open);
                var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                var bytes = md5.ComputeHash(file);
                file.Close();
                var sb = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(bytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch(System.Exception e)
            {
                MessageBox.Show(e.ToString());
                return "";
            }
            finally
            {
                if (file != null) file.Close();
            }
        }

        /// <summary>
        /// Execute the <see cref="CompileProcess"/>.
        /// </summary>
        /// <param name="SCDebug">Whether SCDebug is switched on.</param>
        /// <param name="StageDebug">Whether Stage Debug is switched on.</param>
        internal abstract void ExecuteProcess(bool SCDebug, bool StageDebug);

        /// <summary>
        /// Save code based on debug mode.
        /// </summary>
        /// <param name="SCDebug">Whether SCDebug is switched on.</param>
        /// <param name="StageDebug">Whether Stage Debug is switched on.</param>
        protected void GenerateCode(bool SCDebug, bool StageDebug)
        {
            if (SCDebug)
            {
                source.SaveSCDebugCode(projLuaPath);
            }
            else if (StageDebug)
            {
                source.SaveStageDebugCode(projLuaPath);
            }
            else
            {
                source.SaveCode(projLuaPath);
            }
        }

        /// <summary>
        /// Write root code.
        /// </summary>
        protected void WriteRoot()
        {
            FileStream s = null;
            StreamWriter sw = null;
            try
            {
                s = new FileStream(rootLuaPath, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(s, Encoding.UTF8);
                sw.Write(rootCode);
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                if (sw != null) sw.Close();
                if (s != null) s.Close();
            }
        }

        /// <summary>
        /// Get resources need to pack by meta.
        /// </summary>
        /// <param name="resNeedToPack">The output list of resources need to pack.</param>
        /// <param name="resPathToMD5">The dictionary of resource archivePath -> (directoryPath, MD5 Hash)
        /// of the resource.</param>
        protected void GatherResByResMeta(Dictionary<string, string> resNeedToPack
            , Dictionary<string, Tuple<string, string>> resPathToMD5)
        {
            StreamReader srMeta = null;
            StreamWriter swMeta = null;
            try
            {
                srMeta = new StreamReader(projMetaPath);
                string temp;
                while (!srMeta.EndOfStream)
                {
                    temp = srMeta.ReadLine();
                    string[] scom = temp.Split('|');
                    string spathWithArchive = "";
                    for (int i = 1; i < scom.Length; i++)
                    {
                        spathWithArchive += scom[i] + "|";
                    }
                    string md5 = scom[0];
                    scom = spathWithArchive.Split('|');
                    string sArchive = "";
                    for (int i = 1; i < scom.Length; i++)
                    {
                        sArchive += scom[i];
                    }
                    string spath = scom[0];
                    if (!resPathToMD5.ContainsKey(sArchive))
                        resPathToMD5.Add(sArchive, new Tuple<string, string>(spath, md5));
                }
                srMeta.Close();
                swMeta = new StreamWriter(projMetaPath, false);
                foreach (KeyValuePair<string, string> resPath in resourceFilePath)
                {
                    string resFullPath = null;
                    bool? undcPath = RelativePathConverter.IsRelativePath(resPath.Value);
                    if (undcPath == true)
                    {
                        if (string.IsNullOrEmpty(projPath)) throw new InvalidRelativeResPathException(resPath.Value);
                        resFullPath = Path.GetFullPath(Path.Combine(projPath, resPath.Value));
                    }
                    else if(undcPath == false)
                    {
                        resFullPath = resPath.Value;
                    }
                    if (undcPath != null)
                    {
                        if (resPathToMD5.ContainsKey(resPath.Key))
                        {
                            if (GetMD5HashFromFile(resFullPath) != resPathToMD5[resPath.Key].Item2)
                                resNeedToPack.Add(resPath.Key, resFullPath);
                        }
                        else
                        {
                            resNeedToPack.Add(resPath.Key, resFullPath);
                        }
                        resPathToMD5.Remove(resPath.Key);
                        swMeta.WriteLine(GetMD5HashFromFile(resFullPath) + "|" + resFullPath + "|" + resPath.Key);
                    }
                }
            }
            catch(System.Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                if (srMeta != null) srMeta.Close();
                if (swMeta != null) swMeta.Close();
            }
        }

        /// <summary>
        /// Get resources need to pack and save meta.
        /// </summary>
        /// <param name="resNeedToPack">The output list of resources need to pack.</param>
        protected void GatherResAndSaveMeta(Dictionary<string, string> resNeedToPack)
        {
            StreamWriter swMeta = null;
            try
            {
                foreach (KeyValuePair<string, string> resPath in resourceFilePath)
                {
                    bool? undcPath = RelativePathConverter.IsRelativePath(resPath.Value);
                    if (undcPath == true)
                    {
                        if (string.IsNullOrEmpty(projPath)) throw new InvalidRelativeResPathException(resPath.Value);
                        string sp = Path.GetFullPath(Path.Combine(projPath, resPath.Value));
                        resNeedToPack.Add(resPath.Key, sp);
                    }
                    else if (undcPath == false)
                    {
                        resNeedToPack.Add(resPath.Key, resPath.Value);
                    }
                }
                swMeta = new StreamWriter(projMetaPath, false);
                foreach (KeyValuePair<string, string> resPath in resourceFilePath)
                {
                    bool? undcPath = RelativePathConverter.IsRelativePath(resPath.Value);
                    if (undcPath == true)
                    {
                        if (string.IsNullOrEmpty(projPath)) throw new InvalidRelativeResPathException(resPath.Value);
                        string sp = Path.GetFullPath(Path.Combine(projPath, resPath.Value));
                        swMeta.WriteLine(GetMD5HashFromFile(sp) + "|" + sp + "|" + resPath.Key);
                    }
                    else if(undcPath == false)
                    {
                        swMeta.WriteLine(GetMD5HashFromFile(resPath.Value) + "|" + resPath.Value + "|" + resPath.Key);
                    }
                }
                swMeta.Close();
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                if(swMeta!=null) swMeta.Close();
            }
        }

        /// <summary>
        /// Get all resources need to pack.
        /// </summary>
        /// <param name="resNeedToPack">The output list of resources need to pack.</param>
        protected void GatherAllRes(Dictionary<string, string> resNeedToPack)
        {
            if (File.Exists(projMetaPath)) File.Delete(projMetaPath);
            foreach (KeyValuePair<string,string> resPath in resourceFilePath)
            {
                bool? undcPath = RelativePathConverter.IsRelativePath(resPath.Value);
                if (undcPath == true)
                {
                    if (string.IsNullOrEmpty(projPath)) throw new InvalidRelativeResPathException(resPath.Value);
                    resNeedToPack.Add(resPath.Key, Path.GetFullPath(Path.Combine(projPath, resPath.Value)));
                }
                else if (undcPath == false)
                {
                    resNeedToPack.Add(resPath.Key, resPath.Value);
                }
            }
        }

        /// <summary>
        /// Generate pack batch and execute it by given information.
        /// </summary>
        /// <param name="currentApp">The current <see cref="App"/>.</param>
        /// <param name="resNeedToPack">The output list of resources need to pack.</param>
        /// <param name="resPathToMD5">The dictionary of resource archivePath -> (directoryPath, MD5 Hash)
        /// of the resource.</param>
        /// <param name="includeRoot">Whether regenerates root.lua.</param>
        /// <param name="preserveZip">Whether zip file must be preserved.</param>
        protected void PackFileUsingInfo(App currentApp, Dictionary<string, string> resNeedToPack
            , Dictionary<string, Tuple<string, string>> resPathToMD5, bool includeRoot, bool preserveZip = false)
        {
            Dictionary<string, string> entry2File = new Dictionary<string, string>();
            string temp;
            try
            {
                if (includeRoot)
                {
                    entry2File.Add(Path.GetFileName(rootLuaPath), rootLuaPath);
                }
                entry2File.Add(Path.GetFileName(projLuaPath), projLuaPath);
                if (currentApp.SaveResMeta)
                {
                    foreach (KeyValuePair<string, string> resPath in resNeedToPack)
                    {
                        entry2File.Add(resPath.Key, resPath.Value);
                    }
                    foreach (KeyValuePair<string, Tuple<string, string>> kvp in resPathToMD5)
                    {
                        entry2File.Add(kvp.Key, kvp.Value.Item1);
                    }
                }
                else
                {
                    //if (File.Exists(targetZipPath)) File.Delete(targetZipPath);
                    foreach (KeyValuePair<string, string> resPath in resourceFilePath)
                    {
                        bool? undcPath = RelativePathConverter.IsRelativePath(resPath.Value);
                        if (undcPath == true)
                        {
                            if (string.IsNullOrEmpty(projPath)) throw new InvalidRelativeResPathException(resPath.Value);
                            temp = Path.GetFullPath(Path.Combine(projPath, resPath.Value));
                            entry2File.Add(resPath.Key, temp);
                        }
                        else if (undcPath == false)
                        {
                            entry2File.Add(resPath.Key, resPath.Value);
                        }
                    }
                }

                if (currentApp.PackProj)
                {
                    if (!string.IsNullOrEmpty(source.DocPath))
                    {
                        entry2File.Add(source.RawDocName, source.DocPath);
                    }
                }
                int entryCount = entry2File.Count;
                float currentCount = 0;
                ZipCompressor compressor;
                if (currentApp.BatchPacking)
                {
                    compressor = new ZipCompressorBatch(targetZipPath, zipExePath, rootZipPackPath);
                }
                else
                {
                    compressor = new ZipCompressorInternal(targetZipPath);
                }
                foreach (string s in compressor.PackByDictReporting(entry2File, !currentApp.SaveResMeta && !preserveZip))
                {
                    ProgressChanged_Private?.Invoke(this, new ProgressChangedEventArgs(Convert.ToInt32(currentCount), s));
                    currentCount += 1.0f / entryCount;
                }
            }
            catch(System.Exception e)
            {
                MessageBox.Show("Pack process failed.\n"+e.ToString());
            }
            finally
            {
                if (File.Exists(projLuaPath)) File.Delete(projLuaPath);
            }
        }

        /// <summary>
        /// Generate pack batch and execute it by given information.
        /// </summary>
        /// <param name="currentApp">The current <see cref="App"/>.</param>
        /// <param name="resNeedToPack">The output list of resources need to pack.</param>
        /// <param name="resPathToMD5">The dictionary of resource archivePath -> (directoryPath, MD5 Hash) 
        /// of the resource.</param>
        /// <param name="includeRoot">Whether regenerates root.lua.</param>
        protected void PackFileUsingInfo_old(App currentApp, List<string> resNeedToPack
            , Dictionary<string, Tuple<string, string>> resPathToMD5, bool includeRoot)
        {
            //Pack File using info
            FileStream packBatS = null;
            StreamWriter packBat = null;
            try
            {
                packBatS = new FileStream(rootZipPackPath, FileMode.Create);
                packBat = new StreamWriter(packBatS, Encoding.Default);
                if (includeRoot)
                {
                    packBat.WriteLine(zipExePath + " u -tzip -mcu=on \"" + targetZipPath
                      + "\" \"" + rootLuaPath + "\" \"" + projLuaPath + "\"");
                }
                else
                {
                    packBat.WriteLine(zipExePath + " u -tzip -mcu=on \"" + targetZipPath
                      + "\" \"" + projLuaPath + "\"");
                }
                if (currentApp.SaveResMeta)
                {
                    foreach (string resPath in resNeedToPack)
                    {
                        packBat.WriteLine(zipExePath + " u -tzip -mcu=on \"" + targetZipPath + "\" \""
                            + resPath + "\"");
                    }
                    foreach (KeyValuePair<string, Tuple<string, string>> kvp in resPathToMD5)
                    {
                        packBat.WriteLine(zipExePath + " d -tzip -mcu=on \"" + targetZipPath + "\" \""
                            + kvp.Key + "\"");
                    }
                }
                else
                {
                    if (File.Exists(targetZipPath)) File.Delete(targetZipPath);
                    foreach (KeyValuePair<string, string> resPath in resourceFilePath)
                    {
                        bool? undcPath = RelativePathConverter.IsRelativePath(resPath.Value);
                        if (undcPath == true)
                        {
                            if (string.IsNullOrEmpty(projPath)) throw new InvalidRelativeResPathException(resPath.Value);
                            packBat.WriteLine(zipExePath + " u -tzip -mcu=on \"" + targetZipPath + "\" \""
                                + Path.GetFullPath(Path.Combine(projPath, resPath.Value)) + "\"");
                        }
                        else if (undcPath == false)
                        {
                            packBat.WriteLine(zipExePath + " u -tzip -mcu=on \"" + targetZipPath + "\" \""
                                + resPath + "\"");
                        }
                    }
                }

                if (currentApp.PackProj)
                {
                    if (!string.IsNullOrEmpty(source.DocPath))
                    {
                        packBat.WriteLine(zipExePath + " u -tzip -mcu=on \"" + targetZipPath + "\" \""
                            + source.DocPath + "\"");
                    }
                }
                packBat.Close();
                Process pack = new Process
                {
                    StartInfo = new ProcessStartInfo(rootZipPackPath)
                    {
                        UseShellExecute = true,
                        CreateNoWindow = false
                    }
                };
                pack.Start();
                pack.WaitForExit();
                //catch { }
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                if (packBat != null) packBat.Close();
                if (packBatS != null) packBatS.Close();
                if (File.Exists(projLuaPath)) File.Delete(projLuaPath);
            }
        }

        protected void PackFileUsingInfo_old2(App currentApp, List<string> resNeedToPack, Dictionary<string, string> resPathToMD5)
        {
            //Pack File using info
            FileStream packBatS = null;
            StreamWriter packBat = null;
            try
            {
                packBatS = new FileStream(rootZipPackPath, FileMode.Create);
                packBat = new StreamWriter(packBatS, Encoding.Default);
                bool isRenew = false;
                if (resPathToMD5.Count != 0 || !currentApp.SaveResMeta)
                {
                    isRenew = true;
                    if (File.Exists(targetZipPath)) File.Delete(targetZipPath);
                }
                packBat.WriteLine(zipExePath + " u -tzip -mcu=on \"" + targetZipPath
                    + "\" \"" + rootLuaPath + "\" \"" + projLuaPath + "\"");

                if (!isRenew)
                {
                    foreach (string resPath in resNeedToPack)
                    {
                        packBat.WriteLine(zipExePath + " u -tzip -mcu=on \"" + targetZipPath + "\" \""
                            + resPath + "\"");
                    }
                }
                else
                {
                    foreach (KeyValuePair<string, string> resPath in resourceFilePath)
                    {
                        bool? undcPath = RelativePathConverter.IsRelativePath(resPath.Value);
                        if (undcPath == true)
                        {
                            if (string.IsNullOrEmpty(projPath)) throw new InvalidRelativeResPathException(resPath.Value);
                            packBat.WriteLine(zipExePath + " u -tzip -mcu=on \"" + targetZipPath + "\" \""
                                + Path.GetFullPath(Path.Combine(projPath, resPath.Value)) + "\"");
                        }
                        else if(undcPath == false)
                        {
                            packBat.WriteLine(zipExePath + " u -tzip -mcu=on \"" + targetZipPath + "\" \""
                                + resPath + "\"");
                        }
                    }
                }

                if (currentApp.PackProj)
                {
                    if (!string.IsNullOrEmpty(source.DocPath))
                    {
                        packBat.WriteLine(zipExePath + " u -tzip -mcu=on \"" + targetZipPath + "\" \""
                            + source.DocPath + "\"");
                    }
                }
                packBat.Close();
                Process pack = new Process
                {
                    StartInfo = new ProcessStartInfo(rootZipPackPath)
                    {
                        UseShellExecute = true,
                        CreateNoWindow = false
                    }
                };
                pack.Start();
                pack.WaitForExit();
                //catch { }
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                if (packBat != null) packBat.Close();
                if (packBatS != null) packBatS.Close();
                if (File.Exists(projLuaPath)) File.Delete(projLuaPath);
            }
        }

        /// <summary>
        /// Check the permission of operation at given folder.
        /// </summary>
        /// <param name="folder">The given path.</param>
        /// <returns>Whether it have operation permission</returns>
        public static bool HasOperationPermission(string folder)
        {
            var currentUserIdentity = Path.Combine(Environment.UserDomainName, Environment.UserName);

            DirectorySecurity fileAcl = Directory.GetAccessControl(folder);
            var userAccessRules = fileAcl.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount)).OfType<FileSystemAccessRule>().Where(i => i.IdentityReference.Value == currentUserIdentity).ToList();

            return userAccessRules.Any(i => i.AccessControlType == AccessControlType.Deny);
        }

        /// <summary>
        /// Check whether program can operate directly at given folder.
        /// </summary>
        /// <param name="folder">The given path.</param>
        /// <returns>Whether program can operate directly.</returns>
        public static bool CanOperate(string folder)
        {
            try
            {
                return HasOperationPermission(folder);
            }
            catch
            {
                return false;
            }
        }
    }
}
