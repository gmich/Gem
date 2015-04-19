using Gem.Gui.Input;
using Gem.Gui.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Gui.Controls
{
    class DesktopEvents
    {

        public DesktopEvents()
        {
            throw new NotImplementedException();
        }

        DesktopInputHelper inputHandler;
        bool gotMouseCapture;

        bool Hover(Region region)
        {
            return gotMouseCapture = 
                region.Frame.Contains(inputHandler.MousePosition);
        }

        bool LostFocus(Region region)
        {
            return region.Frame.Contains(inputHandler.MousePosition);
        }

        bool gotFocus;
        bool HasFocus(Region region)
        {
             
            return (gotMouseCapture && inputHandler.IsLeftButtonClicked());
        }

        //TODO: implement on release On release
    }
}
