#region Using Statements

using System;
using System.Collections.Generic;
using Entities;
using Entities.NullObjects;
using Security;
using WaveEngine.Common.Graphics;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Gestures;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Physics2D;

#endregion

namespace ExceptionsProject
{
    public class MapScene : Scene
    {
        private readonly IDictionary<string, Character> characters = new Dictionary<string, Character>();
        private ExceptionsGame game;

        protected override void CreateScene()
        {
            // Create a 2D camera
            var camera2D = new FreeCamera2D("Camera2D") {BackgroundColor = Color.CornflowerBlue};

            EntityManager.Add(camera2D);

            var fakeApplicationContext = new FakeApplicationContext();
            var southPlayer = new Player(fakeApplicationContext.GetCurrentUser());
            for (int i = 0; i < 3; i++)
            {
                southPlayer.Add(new Character {Velocity = 10});
            }

            var northPlayer = new Player(new User("xavier"));
            northPlayer.Add(new Character {Velocity = 4});

            game = new GameFactory().NewGame(southPlayer, northPlayer, fakeApplicationContext);

            const int spriteWidth = 49;
            const int spriteHeight = 41;

            for (int x = 0; x < game.MapColumns; x++)
            {
                for (int y = 0; y < game.MapRows; y++)
                {
                    var tile = game.GetPosition(x, y);
                    var xPosition = spriteWidth*x + spriteWidth;
                    var yPosition = spriteHeight*y;
                    var positionOnScreen = new Transform2D
                        {
                            X = xPosition,
                            Y = yPosition,
                            DrawOrder = (float) (1 - x*0.01 - y*0.001)
                        };

                    var component = new Sprite("Content/Grass Block.png");

                    var tileEntity = new Entity(tile.Name).AddComponent(positionOnScreen)
                                                          .AddComponent(component)
                                                          .AddComponent(new SpriteRenderer(DefaultLayers.Alpha))
                                                          .AddComponent(new TileBehavior(tile));
                    EntityManager.Add(tileEntity);

                    var character = game.GetCharacterAtPosition(tile);
                    if (character is NullCharacter)
                    {
                        continue;
                    }

                    yPosition -= spriteHeight/2;
                    var characterTouchGestures = new TouchGestures();
                    var characterEntity =
                        new Entity(character.Id).AddComponent(new Transform2D {X = xPosition, Y = yPosition})
                                                .AddComponent(new Sprite("Content/Character Boy"))
                                                .AddComponent(new SpriteRenderer(DefaultLayers.Alpha))
                                                .AddComponent(new RectangleCollider())
                                                .AddComponent(new CharacterBehavior(character))
                                                .AddComponent(characterTouchGestures);
                    this.characters.Add(character.Id, (Character) character);
                    characterTouchGestures.TouchTap += touchGestures_TouchTap;
                    EntityManager.Add(characterEntity);
                }
            }
        }

        private void touchGestures_TouchTap(object sender, GestureEventArgs e)
        {
            var entity = sender as Entity;
            if (entity != null)
            {
                var character = this.characters[entity.Name];
                game.Select(character);
            }
        }
    }

    public class CharacterBehavior : Behavior
    {
        private readonly ICharacter character;

        public CharacterBehavior(ICharacter character)
        {
            this.character = character;
        }

        protected override void Update(TimeSpan gameTime)
        {
            var sprite = this.Owner.FindComponent<Sprite>();
            if (character.CanBeCharged)
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