using Caliburn.Micro;
using Gemini;
using Gemini.Modules.CodeCompiler;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Media;

namespace Gem.IDE.Core.Modules.Editor
{
    [Export(typeof(EditorViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class EditorViewModel : Gemini.Framework.Document
    {
        private readonly ICodeCompiler codeCompiler;
        private IEditorView editorView;
        private readonly List<IScript> scripts = new List<IScript>();

        [ImportingConstructor]
        public EditorViewModel(ICodeCompiler codeCompiler)
        {
            this.codeCompiler = codeCompiler;
        }

        protected override void OnViewLoaded(object view)
        {
            editorView = (IEditorView)view;

            editorView.TextEditor.Text = @"public class GemScript";
            editorView.TextEditor.TextChanged += (sender, e) => CompileScripts();
            CompositionTarget.Rendering += OnRendering;
            CompileScripts();
            base.OnViewLoaded(view);
        }

        private void CompileScripts()
        {
            lock (scripts)
            {
                scripts.Clear();

                var assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);
                var newAssembly = codeCompiler.Compile(
                     new[] { CSharpSyntaxTree.ParseText(editorView.TextEditor.Text) },
                     new[]
                         {
                            MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "mscorlib.dll")),
                            MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.dll")),
                            MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Core.dll")),
                            MetadataReference.CreateFromFile(typeof(IResult).Assembly.Location),
                            MetadataReference.CreateFromFile(typeof(AppBootstrapper).Assembly.Location),
                            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                            MetadataReference.CreateFromFile(Assembly.GetEntryAssembly().Location)
                        },
                    "GemDemoScript");

                if (newAssembly != null)
                {
                    scripts.AddRange(newAssembly.GetTypes()
                        .Where(x => typeof(IScript).IsAssignableFrom(x))
                        .Select(x => (IScript)Activator.CreateInstance(x)));
                }
            }
        }

        private void OnRendering(object sender, EventArgs e)
        {
            lock (scripts)
                scripts.ForEach(x => x.Execute(this));
        }

        protected override void OnDeactivate(bool close)
        {
            if (close)
                CompositionTarget.Rendering -= OnRendering;
            base.OnDeactivate(close);
        }
    }
}


