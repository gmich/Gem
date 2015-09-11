using System;
using Gem.IDE.MonoGame.Interop.Arguments;
using Gem.IDE.MonoGame.Interop.Controls;

namespace Gem.IDE.MonoGame.Interop.Module
{
    internal interface IInternalGameModule
    {
        bool IsRunning { get; }

        void Prepare(DrawingSurface drawingSurface, IServiceProvider provider);
        void Run();
        void Draw(DrawEventArgs e);
        void Stop();
    }
}