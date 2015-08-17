using Gem.AI.FiniteStateMachine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gem.Engine.Tests.AI
{
    [TestClass]
    public class StateMachineTests
    {
        private const string PauseCommand = "Pause";
        private const string EndCommand = "End";
        private const string BeginCommand = "Begin";
        private const string ResumeCommand = "Resume";
        private const string ExitCommand = "Exit";

        [TestMethod]
        public void SmartPhoneScreenStateMachineTest()
        {
            #region States

            var active = new State<SmartPhoneScreenContext>(screen => { }).Named("Active");
            var exit = new State<SmartPhoneScreenContext>(screen => { }).Named("Exit");
            var paused = new State<SmartPhoneScreenContext>(screen => { }).Named("Paused");
            var inactive = new State<SmartPhoneScreenContext>(screen => { }).Named("Inactive");

            #endregion

            #region Transitions

            active.AddTransition(Transition<SmartPhoneScreenContext>
                .To(() => paused)
                .When(screen => screen.Command == PauseCommand)
                .Named("Pausing"));

            active.AddTransition(Transition<SmartPhoneScreenContext>
                .To(() => inactive)
                .When(screen => screen.Command == EndCommand)
                .Named("Going inactive"));

            inactive.AddTransition(Transition<SmartPhoneScreenContext>
                .To(() => active)
                .When(screen => screen.Command == BeginCommand)
                .Named("Activating"));

            inactive.AddTransition(Transition<SmartPhoneScreenContext>
                .To(() => exit)
                .When(screen => screen.Command == ExitCommand)
                .Named("Exiting"));

            paused.AddTransition(Transition<SmartPhoneScreenContext>
                .To(() => inactive)
                .When(screen => screen.Command == EndCommand)
                .Named("Going inactive"));

            paused.AddTransition(Transition<SmartPhoneScreenContext>
                .To(() => exit)
                .When(screen => screen.Command == ResumeCommand)
                .Named("Activating"));

            #endregion

            var smartPhoneHandler = StateMachine.Create(inactive);

            var context = new SmartPhoneScreenContext();

            context.Command = BeginCommand;
            smartPhoneHandler.Operate(context);
            Assert.AreEqual(smartPhoneHandler.ActiveState, active);

            context.Command = PauseCommand;
            smartPhoneHandler.Operate(context);
            Assert.AreEqual(smartPhoneHandler.ActiveState, paused);

            smartPhoneHandler.Operate(context);
            Assert.AreEqual(smartPhoneHandler.ActiveState, paused);

            context.Command = EndCommand;
            smartPhoneHandler.Operate(context);
            Assert.AreEqual(smartPhoneHandler.ActiveState, inactive);

            smartPhoneHandler.Operate(context);
            Assert.AreEqual(smartPhoneHandler.ActiveState, inactive);

            context.Command = BeginCommand;
            smartPhoneHandler.Operate(context);
            Assert.AreEqual(smartPhoneHandler.ActiveState, active);

            context.Command = EndCommand;
            smartPhoneHandler.Operate(context);
            Assert.AreEqual(smartPhoneHandler.ActiveState, inactive);

            context.Command = ExitCommand;
            smartPhoneHandler.Operate(context);
            Assert.AreEqual(smartPhoneHandler.ActiveState, exit);
        }

        #region Context

        public class SmartPhoneScreenContext
        {
            public string Command { get; set; } = string.Empty;
        }

        #endregion
    }
}
