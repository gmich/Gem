using Gem.Engine.BehaviorTreeVisualization.Behaviors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;


namespace Gem.Engine.BehaviorTreeVisualization
{
    internal class Level
    {
        private readonly int width;
        private readonly int height;
        private readonly Texture2D floor;
        private readonly double timeToBehave = 1.0d;
        private readonly Player player;

        private Key key;
        private double timePassed;

        public Level(ContentManager content, int width, int height)
        {
            this.height = height;
            this.width = width;
            Context = new BehaviorContext(this);
            Context.Behavior = new WalkBehavior().Behavior;

            floor = content.Load<Texture2D>(@"Game/emptyTile");
            player = new Player(content.Load<Texture2D>(@"Game/player"), width, height);
            key = new Key(content.Load<Texture2D>(@"Game/key"), width, height);

            PlayerPosition = 0;
            for (int i = 0; i < Map.Count(); i++)
            {
                if (i == 4 || i==8)
                {
                    Map[i] = new Key(content.Load<Texture2D>(@"Game/key"), width, height);
                }
                else if (i == 12 || i==16)
                {
                    Map[i] = new Door(content.Load<Texture2D>(@"Game/door"), width, height);
                }
                else
                {
                    Map[i] = new EmptyTile();
                }
            }
        }
        public IGameComponent[] Map { get; } = new IGameComponent[20];

        public BehaviorContext Context { get; }

        public int PlayerPosition { get; private set; }
        public bool HaveKey { get; set; } = false;

        public bool MovePlayer(int step)
        {
            PlayerPosition = MathHelper.Clamp(step + PlayerPosition, 0, Map.Count() - 1);
            return true;
        }

        public IGameComponent NextTile => Map[MathHelper.Min(PlayerPosition + 1, Map.Count() - 1)];

        public void Update(double timeDelta)
        {
            timePassed += timeDelta;

            if (timePassed >= timeToBehave)
            {
                timePassed = 0.0d;
                if(Context.Behavior.Behave(Context)!= AI.BehaviorTree.BehaviorResult.Running)
                {
                    Context.Behavior = new WalkBehavior().Behavior;
                }
            }
        }
        public void Draw(SpriteBatch batch, Vector2 position)
        {
            for (int i = 0; i < Map.Count(); i++)
            {
                var newPosition = position + new Vector2(i * width, 0);
                batch.Draw(floor, new Rectangle((int)newPosition.X, (int)newPosition.Y, width, height), Color.White);
                Map[i].Draw(batch, newPosition);
            }
            player.Draw(batch, position + new Vector2(PlayerPosition * width, 0));

            if (HaveKey)
            {
                key.Draw(batch, position + new Vector2(PlayerPosition * width, -height));
            }
        }
    }
}
