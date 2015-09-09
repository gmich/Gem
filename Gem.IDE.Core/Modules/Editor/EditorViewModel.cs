using Gemini.Framework;
using Gemini.Modules.CodeCompiler;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Media;

namespace Gem.IDE.Core.Modules.Editor
{
    [Export(typeof(EditorViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class EditorViewModel : Document
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

                var newAssembly = codeCompiler.Compile(
                    new[] { SyntaxTree.ParseText(editorView.TextEditor.Text) },
                    new []
                        {
                            MetadataReference.CreateAssemblyReference("mscorlib"),
                            MetadataReference.CreateAssemblyReference("System"),
                            MetadataReference.CreateAssemblyReference("System.ObjectModel"),
                            MetadataReference.CreateAssemblyReference("System.Runtime"),
                            MetadataReference.CreateAssemblyReference("PresentationCore"),
                            //new MetadataFileReference(typeof(IResult).Assembly.Location) ,
                            //new MetadataFileReference(typeof(AppBootstrapper).Assembly.Location) ,
                            //new MetadataFileReference(GetType().Assembly.Location) 
                        },
                    "GemDemoScript");

                scripts.AddRange(newAssembly.GetTypes()
                    .Where(x => typeof(IScript).IsAssignableFrom(x))
                    .Select(x => (IScript)Activator.CreateInstance(x)));
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


