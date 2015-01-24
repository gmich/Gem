using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Messages
{
    /// <summary>
    /// The game message types.
    /// </summary>
    public enum GameMessageTypes
    {
        RequestBannerState,

        AddBannerState,

        ColorSwitchState,

        PlayerBannerState,

        PlayerDisconnectedState,

        SetTitleState,

        InGameState,

        UpdateEntityState,

        RequestTileState,

        SnapTileState,

        ReleaseTileState,

        GetEntityState,

        RemoveEntityState,

        RotateEntityState,

        UpdateScoreState,

        AddPointingAnimationState
    }
}

