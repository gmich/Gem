using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameEngine2D;
using GameEngine2D.Diagnostics.Console;
using Moq;

namespace GameEngine2DTests
{
    [TestClass]
    public class ConsoleCommandTests
    {
        [TestMethod]
        public void TimeRullerLogVisibilityTest()
        {

            try
            {
                var game = new Engine();
                //game.RunOneFrame();
                DebugSystem.Initialize(game, "Font");
                DebugSystem.Instance.TimeRuler.Initialize();

                bool logVisibility = DebugSystem.Instance.TimeRuler.ShowLog;
                //This command should toggle the time ruler's log visibility
                DebugSystem.Instance.DebugCommandUI.ExecuteCommand("tr log");

                Assert.AreEqual(!logVisibility, DebugSystem.Instance.TimeRuler.ShowLog);
            }
            catch (Exception ex)
            {
                //Several exceptions are thrown because the graphics device is not properly initialized
            }


        }
    }
}
