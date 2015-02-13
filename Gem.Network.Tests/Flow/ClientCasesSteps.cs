using Gem.Network.DynamicBuilders;
using Gem.Network.Messages;
using Lidgren.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using TechTalk.SpecFlow;

namespace Gem.Network.Tests.Flow
{
    [Binding]
    public class ClientCasesSteps
    {
        private Process server;
        private Client client;
        private NetIncomingMessage msg;
        private List<DynamicPropertyInfo> propertyList = new List<DynamicPropertyInfo>
            {
                new DynamicPropertyInfo{
                        PropertyName = "Name",
                        PropertyType = typeof(string)
                }
            };
        private Type myNewType;

        [Given(@"A server is running")]
        public void GivenAServerIsRunning()
        {
            server = new Process();
            try
            {
                server.StartInfo.FileName = @"C:\Users\George\Documents\GitHub\Gem\Gem.Network.Example\bin\Debug\Gem.Network.Example.exe";
                server.StartInfo.Arguments = "DecodeIncomingDynamicMessageTest";
                server.Start();
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed to launch the server. Reason:" + ex.Message);
            }
        }

        [Given(@"I connect to the server")]
        public void GivenIConnectToTheServer()
        {
            myNewType = PocoBuilder.Create("POCO", propertyList);
            client = new Client(new IPEndPoint(NetUtility.Resolve("127.0.0.1"), 14241), "local");

            client.Connect("local", 14241);
        }

        [When(@"I send a greeding message")]
        public void WhenISendAGreedingMessage()
        {
            //wait for the server to respond
            System.Threading.Thread.Sleep(100);
            msg = client.ReadMessage();

            var om = client.CreateMessage();
            client.SendMessage(om);


        }

        [Then(@"I should get a response message and connection approval")]
        public void ThenIShouldGetAResponseMessageAndConnectionApproval()
        {
            //wait for the server to respond
            System.Threading.Thread.Sleep(100);
            msg = client.ReadMessage();

            dynamic readableMessageWithType = MessageSerializer.Decode(msg, myNewType);
            Assert.AreEqual(readableMessageWithType.Name, "DynamicType");
            server.CloseMainWindow();
            server.Close();

        }
    }
}
