using System;
namespace Gem.Gui.Alignment
{
    public class AlignmentContext
    {
        public event EventHandler onAlignmentChange;

        public AlignmentContext()
        {
            HorizontalAlignment = HorizontalAlignment.Manual;
            VerticalAlignment = VerticalAlignment.Manual;
            Movement = AlignmentMovement.Fixed;
        }
        
        //raise on alignmentchange event
        public HorizontalAlignment HorizontalAlignment { get; set; }

        //raise on alignmentchange event
        public VerticalAlignment VerticalAlignment { get; set; }

        public AlignmentMovement Movement { get; set; }
    }
}
