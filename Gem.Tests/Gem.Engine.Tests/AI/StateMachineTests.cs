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

            var active = new State<SmartPhoneScreen>(screen => { }).Named("Active");
            var exit = new State<SmartPhoneScreen>(screen => { }).Named("Exit");
            var paused = new State<SmartPhoneScreen>(screen => { }).Named("Paused");
            var inactive = new State<SmartPhoneScreen>(screen => { }).Named("Inactive");

            #endregion

            #region Transitions

            active.AddTransition(Transition<SmartPhoneScreen>
                .To(() => paused)
                .When(screen => screen.Command == PauseCommand)
                .Named("Pausing"));

            active.AddTransition(Transition<SmartPhoneScreen>
                .To(() => inactive)
                .When(screen => screen.Command == EndCommand)
                .Named("Going inactive"));

            inactive.AddTransition(Transition<SmartPhoneScreen>
                .To(() => active)
                .When(screen => screen.Command == BeginCommand)
                .Named("Activating"));

            inactive.AddTransition(Transition<SmartPhoneScreen>
                .To(() => exit)
                .When(screen => screen.Command == ExitCommand)
                .Named("Exiting"));

            paused.AddTransition(Transition<SmartPhoneScreen>
                .To(() => inactive)
                .When(screen => screen.Command == EndCommand)
                .Named("Going inactive"));

            paused.AddTransition(Transition<SmartPhoneScreen>
                .To(() => exit)
                .When(screen => screen.Command == ResumeCommand)
                .Named("Activating"));

            #endregion

            var smartPhoneHandler = StateMachine.Create(inactive);

            var context = new SmartPhoneScreen();

            context.Command = BeginCommand;
            smartPhoneHandler.Operate(context);
            Assert.Equals(smartPhoneHandler.ActiveState,active);

            context.Command = PauseCommand;
            smartPhoneHandler.Operate(context);
            Assert.Equals(smartPhoneHandler.ActiveState, paused);

            smartPhoneHandler.Operate(context);
            Assert.Equals(smartPhoneHandler.ActiveState, paused);

            context.Command = EndCommand;
            smartPhoneHandler.Operate(context);
            Assert.Equals(smartPhoneHandler.ActiveState, inactive);

            smartPhoneHandler.Operate(context);
            Assert.Equals(smartPhoneHandler.ActiveState, inactive);

            context.Command = BeginCommand;
            smartPhoneHandler.Operate(context);
            Assert.Equals(smartPhoneHandler.ActiveState, active);

            context.Command = EndCommand;
            smartPhoneHandler.Operate(context);
            Assert.Equals(smartPhoneHandler.ActiveState, inactive);

            context.Command = ExitCommand;
            smartPhoneHandler.Operate(context);
            Assert.Equals(smartPhoneHandler.ActiveState, exit);
        }

        #region Context

        public class SmartPhoneScreen
        {
            public string Command { get; set; } = string.Empty;
        }

        #endregion
    }
}
