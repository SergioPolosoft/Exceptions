#region Using Statements

using WaveEngine.Common;
using WaveEngine.Framework.Managers;
using WaveEngine.Framework.Services;

#endregion

namespace ExceptionsProject
{
    public class Game : WaveEngine.Framework.Game
    {
        private MapScene mapScene;

        public override void Initialize(IApplication application)
        {
            base.Initialize(application);

            // ViewportManager is used to automatically adapt resolution to fit screen size
            ViewportManager vm = WaveServices.ViewportManager;
            vm.Activate(1280, 720, ViewportManager.StretchMode.Uniform);

            mapScene = new MapScene();
            var screenContext = new ScreenContext(mapScene);
            WaveServices.ScreenContextManager.To(screenContext);

        }

        
    }
}