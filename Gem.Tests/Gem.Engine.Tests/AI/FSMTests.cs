using Gem.Engine.AI.FSM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Gem.Engine.Tests.AI
{
    [TestClass]
    public class FSMTests
    {

        internal class FSMContext
        {
            public string State { get; set; }
        }

        [TestMethod]
        public void FSM_Events_Lead_To_Valid_Transitions()
        {
            string active = "active";
            string inactive = "inactive";
            string idle = "idle";

            var fsmBuilder = new FSMBuilder<FSMContext>();
            fsmBuilder
            .AddState(active, c => c.State = active)
            .AddState(inactive, c => c.State = inactive)
            .AddState(idle, c => c.State = idle);

            var turnOffEvent = fsmBuilder.From(active).To(inactive);
            var turnOnEvent = fsmBuilder.From(inactive).To(active);
            var goIdleEvent = fsmBuilder.From(active).To(idle);

            var fsm = fsmBuilder.Build(active);

            var context = new FSMContext();

            fsm.Update(context);
            Assert.AreEqual(active, context.State);

            turnOffEvent.Raise();
            fsm.Update(context);
            Assert.AreEqual(inactive, context.State);

            goIdleEvent.Raise();
            fsm.Update(context);
            Assert.AreEqual(inactive, context.State);

            turnOnEvent.Raise();
            fsm.Update(context);
            Assert.AreEqual(active, context.State);

            goIdleEvent.Raise();
            fsm.Update(context);
            Assert.AreEqual(idle, context.State);
        }
    }
}
