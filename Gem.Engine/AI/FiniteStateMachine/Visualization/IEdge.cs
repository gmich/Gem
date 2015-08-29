namespace Gem.AI.FiniteStateMachine.Visualization
{
    internal interface IEdge
    {
        IVertex Source { get; }

        IVertex Target { get; }

        Direction Direction { get; set; }

    }
}
