using System;

namespace Gem.IDE.Modules.SpriteSheets.ViewModels
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AnimationAttribute : Attribute
    { }

    [AttributeUsage(AttributeTargets.Property)]
    public class SpriteSheetAttribute : Attribute
    { }

    [AttributeUsage(AttributeTargets.Property)]
    public class PresentationAttribute : Attribute
    { }
}
