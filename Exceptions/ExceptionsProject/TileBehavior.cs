using System;
using Entities;
using WaveEngine.Common.Graphics;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework;

namespace ExceptionsProject
{
    public class TileBehavior : Behavior
    {
        private readonly IPosition position;

        public TileBehavior(IPosition tile)
        {
            this.position = tile;
        }

        protected override void Update(TimeSpan gameTime)
        {
            var sprite = this.Owner.FindComponent<Sprite>();
            if (position.Selectable)
            {
                sprite.TintColor = Color.Gray;
            }
            else
            {
                sprite.TintColor = Color.White;
            }
        }
    }
}