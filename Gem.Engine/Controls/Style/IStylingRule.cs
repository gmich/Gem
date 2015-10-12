using Gem.Engine.GTerminal.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Engine.Controls.Style
{

    public interface IStylingRule
    {
        int? Width { get;  }
        int? Height { get;  }
        Texture2D BackgroundImage { get;  }
        Color BackgroundColor { get;  }
        Color TextColor { get;  }
        string Text { get;  }
        Box Padding { get;  }
        BorderBox Border { get;  }
        Box Margin { get;  }
        IAlignment Position { get;  }
    }
}
