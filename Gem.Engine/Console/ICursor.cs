using System;
using System.Collections.Generic;

namespace Gem.Console
{
    public interface ICursor
    {
        event EventHandler<CursorEventArgs> CursorChanged;

        int Row { get; }
        int Head { get; set; }

        void Up();
        void Down();
        void Left();
        void Right();
        
        void Update(IEnumerable<Row> rows);
    }
}
