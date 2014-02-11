﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Net.Sf.Dbdeploy.Exceptions;
using Net.Sf.Dbdeploy.Scripts.EmbeddedResources;

namespace Net.Sf.Dbdeploy.Scripts
{
    public class AssemblyScanner : IScriptScanner
    {
        private readonly FilenameParser filenameParser;

        private readonly TextWriter infoTextWriter;
        private readonly Encoding encoding;
        private readonly IEnumerable<Assembly> assemblies;

        public AssemblyScanner(TextWriter writer, Encoding encoding, IEnumerable<Assembly> assemblies)
        {
            filenameParser = new FilenameParser();

            infoTextWriter = writer;
            this.encoding = encoding;
            this.assemblies = assemblies;
        }

        public List<ChangeScript> GetChangeScripts()
        {
            if(assemblies == null || !assemblies.Any())
                return EmptyList();

            var changeScripts = new List<ChangeScript>();
            foreach (var assembly in assemblies)
            {
                var assemblyScripts = GetChangeScriptsFromAssembly(assembly);  
                changeScripts.AddRange(assemblyScripts);
            }

            return changeScripts;
        }

        private IEnumerable<ChangeScript> GetChangeScriptsFromAssembly(Assembly assembly)
        {
            try
            {
                this.infoTextWriter.WriteLine("Reading change scripts from assembly '" + assembly.FullName + "'...");
            }
            catch (IOException)
            {
                // ignore
            }

            var scripts = new List<ChangeScript>();

            var resourceScripts = assembly.GetManifestResourceNames().Where(f => f.EndsWith(".sql"));
            foreach (var resourceScript in resourceScripts)
            {
                string folder = ExtractFolderNameWithUnderlines(resourceScript);
                string fileName = ExtractFileName(resourceScript, folder);

                try
                {
                    folder = folder.Replace("._", ".");
                    int scriptNumber = filenameParser.ExtractScriptNumberFromFilename(fileName);

                    var embeddedFileInfo = new EmbeddedFileInfo
                        {
                            Assembly = assembly, FileName = fileName, Folder = folder, ResourceName = resourceScript
                        };
                    scripts.Add(new ChangeScript(embeddedFileInfo, scriptNumber, encoding));
                }
                catch (UnrecognisedFilenameException)
                {
                    // ignore
                }
            }

            return scripts;
        }

        private static string ExtractFolderNameWithUnderlines(string resourceScript)
        {
            var builder = new StringBuilder();

            var matchCollection = Regex.Matches(resourceScript, @"([v]?)(\d+[.][_])+(\d+)([^.]*)");
            foreach (var match in matchCollection)
            {
                builder.Append(match);
            }

            return builder.ToString().TrimEnd('.');
        }

        private static string ExtractFileName(string resourceScript, string folderResourceName)
        {
            int startIndex = resourceScript.IndexOf(folderResourceName) + folderResourceName.Length + 1;
            return resourceScript.Substring(startIndex);
        }

        private List<ChangeScript> EmptyList()
        {
            return new List<ChangeScript>();
        }
    }
}