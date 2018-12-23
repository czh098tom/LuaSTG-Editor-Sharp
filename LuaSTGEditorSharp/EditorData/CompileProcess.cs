using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.AccessControl;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using LuaSTGEditorSharp.EditorData.Document;
using LuaSTGEditorSharp.EditorData.Compile;
using LuaSTGEditorSharp.EditorData.Exception;

namespace LuaSTGEditorSharp.EditorData
{
    public abstract class CompileProcess
    {
        public DocumentData source;

        public HashSet<string> resourceFilePath = new HashSet<string>();
        public List<DefineMarco> marcoDefination = new List<DefineMarco>();

        public string currentPath;
        public string projLuaPath;
        public string rootLuaPath;
        public string rootZipPackPath;

        public string projPath;
        public string projMetaPath;

        public string rootCode;

        public string zipExePath;
        public string luaSTGExePath;

        public string projName;

        public string luaSTGFolder;
        public string targetZipPath;
        
        public static string GetMD5HashFromFile(string fileName)
        {
            FileStream file = null;
            try
            {
                file = new FileStream(fileName, FileMode.Open);
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
            finally
            {
                if (file != null) file.Close();
            }
        }

        internal abstract void ExecuteProcess(bool SCDebug, bool StageDebug);

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
            finally
            {
                if (sw != null) sw.Close();
                if (s != null) s.Close();
            }
        }

        protected void GatherResByResMeta(List<string> resNeedToPack, Dictionary<string, string> resPathToMD5)
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
                    string[] scom = temp.Split(',');
                    string spath = "";
                    for (int i = 1; i < scom.Length; i++)
                    {
                        spath += scom[i];
                    }
                    if (!resPathToMD5.ContainsKey(spath)) resPathToMD5.Add(spath, scom[0]);
                }
                srMeta.Close();
                swMeta = new StreamWriter(projMetaPath, false);
                foreach (string resPath in resourceFilePath)
                {
                    string resFullPath = null;
                    bool? undcPath = RelativePathConverter.IsRelativePath(resPath);
                    if (undcPath == true)
                    {
                        if (string.IsNullOrEmpty(projPath)) throw new InvalidRelativeResPathException(resPath);
                        resFullPath = Path.GetFullPath(Path.Combine(projPath, resPath));
                    }
                    else if(undcPath == false)
                    {
                        resFullPath = resPath;
                    }
                    if (undcPath != null)
                    {
                        if (resPathToMD5.ContainsKey(resFullPath))
                        {
                            if (GetMD5HashFromFile(resFullPath) != resPathToMD5[resFullPath]) resNeedToPack.Add(resFullPath);
                        }
                        else
                        {
                            resNeedToPack.Add(resFullPath);
                        }
                        resPathToMD5.Remove(resFullPath);
                        swMeta.WriteLine(GetMD5HashFromFile(resFullPath) + "," + resFullPath);
                    }
                }
            }
            finally
            {
                if (srMeta != null) srMeta.Close();
                if (swMeta != null) swMeta.Close();
            }
        }

        protected void GatherResAndSaveMeta(List<string> resNeedToPack)
        {
            StreamWriter swMeta = null;
            try
            {
                foreach (string resPath in resourceFilePath)
                {
                    bool? undcPath = RelativePathConverter.IsRelativePath(resPath);
                    if (undcPath == true)
                    {
                        if (string.IsNullOrEmpty(projPath)) throw new InvalidRelativeResPathException(resPath);
                        string sp = Path.GetFullPath(Path.Combine(projPath, resPath));
                        resNeedToPack.Add(sp);
                    }
                    else if (undcPath == false)
                    {
                        resNeedToPack.Add(resPath);
                    }
                }
                swMeta = new StreamWriter(projMetaPath, false);
                foreach (string resPath in resourceFilePath)
                {
                    bool? undcPath = RelativePathConverter.IsRelativePath(resPath);
                    if (undcPath == true)
                    {
                        if (string.IsNullOrEmpty(projPath)) throw new InvalidRelativeResPathException(resPath);
                        string sp = Path.GetFullPath(Path.Combine(projPath, resPath));
                        swMeta.WriteLine(GetMD5HashFromFile(sp) + "," + sp);
                    }
                    else if(undcPath == false)
                    {
                        swMeta.WriteLine(GetMD5HashFromFile(resPath) + "," + resPath);
                    }
                }
                swMeta.Close();
            }
            finally
            {
                if(swMeta!=null) swMeta.Close();
            }
        }

        protected void GatherAllRes(List<string> resNeedToPack)
        {
            if (File.Exists(projMetaPath)) File.Delete(projMetaPath);
            foreach (string resPath in resourceFilePath)
            {
                bool? undcPath = RelativePathConverter.IsRelativePath(resPath);
                if (undcPath == true)
                {
                    if (string.IsNullOrEmpty(projPath)) throw new InvalidRelativeResPathException(resPath);
                    resNeedToPack.Add(Path.GetFullPath(Path.Combine(projPath, resPath)));
                }
                else if (undcPath == false)
                {
                    resNeedToPack.Add(resPath);
                }
            }
        }

        protected void PackFileUsingInfo(App currentApp, List<string> resNeedToPack, Dictionary<string, string> resPathToMD5,
            bool includeRoot)
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
                    foreach(KeyValuePair<string,string> kvp in resPathToMD5)
                    {
                        packBat.WriteLine(zipExePath + " d -tzip -mcu=on \"" + targetZipPath + "\" \""
                            + kvp.Key + "\"");
                    }
                }
                else
                {
                    if (File.Exists(targetZipPath)) File.Delete(targetZipPath);
                    foreach (string resPath in resourceFilePath)
                    {
                        bool? undcPath = RelativePathConverter.IsRelativePath(resPath);
                        if (undcPath == true)
                        {
                            if (string.IsNullOrEmpty(projPath)) throw new InvalidRelativeResPathException(resPath);
                            packBat.WriteLine(zipExePath + " u -tzip -mcu=on \"" + targetZipPath + "\" \""
                                + Path.GetFullPath(Path.Combine(projPath, resPath)) + "\"");
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
            finally
            {
                if (packBat != null) packBat.Close();
                if (packBatS != null) packBatS.Close();
                if (File.Exists(projLuaPath)) File.Delete(projLuaPath);
            }
        }

        protected void PackFileUsingInfo_old(App currentApp, List<string> resNeedToPack, Dictionary<string, string> resPathToMD5)
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
                    foreach (string resPath in resourceFilePath)
                    {
                        bool? undcPath = RelativePathConverter.IsRelativePath(resPath);
                        if (undcPath == true)
                        {
                            if (string.IsNullOrEmpty(projPath)) throw new InvalidRelativeResPathException(resPath);
                            packBat.WriteLine(zipExePath + " u -tzip -mcu=on \"" + targetZipPath + "\" \""
                                + Path.GetFullPath(Path.Combine(projPath, resPath)) + "\"");
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
            finally
            {
                if (packBat != null) packBat.Close();
                if (packBatS != null) packBatS.Close();
                if (File.Exists(projLuaPath)) File.Delete(projLuaPath);
            }
        }

        /// <summary>
        /// 检查当前用户是否拥有此文件夹的操作权限
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static bool HasOperationPermission(string folder)
        {
            var currentUserIdentity = Path.Combine(Environment.UserDomainName, Environment.UserName);

            DirectorySecurity fileAcl = Directory.GetAccessControl(folder);
            var userAccessRules = fileAcl.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount)).OfType<FileSystemAccessRule>().Where(i => i.IdentityReference.Value == currentUserIdentity).ToList();

            return userAccessRules.Any(i => i.AccessControlType == AccessControlType.Deny);
        }

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
