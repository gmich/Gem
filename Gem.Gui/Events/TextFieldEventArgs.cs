using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Gui.Events
{
    public class TextFieldEventArgs : EventArgs
    {
        private readonly string entry;
        private readonly char diff;

        public TextFieldEventArgs(string entry, char diff)
        {
            this.entry = entry;
            this.diff = diff;
        }

        public string Entry { get { return entry; } }

        public char CharacterDiff { get { return diff; } }
    }
}
