using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gem.Console;
using System.Linq;
using System.Collections.Generic;
using Gem.Infrastructure.Functional;
using Gem.Console.Commands;

namespace Gem.Engine.Tests
{
    [TestClass]
    public class ConsoleTests
    {
        [TestMethod]
        public void ConsoleAlignerAlignsMultipleRowsCorrectly()
        {
            var appender = new CellAppender((ch) =>
            {
                return new Cell(ch.ToString(), 10, 0);
            });

            var aligner = new CellAligner(() => 70.0f, appender.GetCells);

            appender.OnCellAppend((sender, args) => aligner.ArrangeRows());

            string twoRows = "123456789";

            foreach (var ch in twoRows)
            {
                appender.AddCell(ch);
            }

            //there should be 2 rows
            Assert.AreEqual(2, aligner.Rows().ToList().Count);

            //the second row should have only 2 entries
            Assert.AreEqual(2, aligner.Rows().ToList()[1].Entries.ToList().Count);

            //which are 8,9
            CollectionAssert.AreEqual(new List<int> { 8, 9 },
                                      aligner.Rows().ToList()[1].Entries.Select(cell => Int32.Parse(cell.Content)).ToList());
        }

        [TestMethod]
        public void ConsoleAlignerAlignsSingleRowCorrectly()
        {
            var appender = new CellAppender((ch) =>
            {
                return new Cell(ch.ToString(), 10, 0);
            });

            var aligner = new CellAligner(() => 70.0f, appender.GetCells);

            appender.OnCellAppend((sender, args) => aligner.ArrangeRows());

            string twoRows = "123456";

            foreach (var ch in twoRows)
            {
                appender.AddCell(ch);
            }

            //there should be 1 rows
            Assert.AreEqual(1, aligner.Rows().ToList().Count);

            //the row should have only 6 entries
            Assert.AreEqual(6, aligner.Rows().ToList()[0].Entries.ToList().Count);

            //which are 1,2,3,4,5,6
            CollectionAssert.AreEqual(new List<int> { 1, 2, 3, 4, 5, 6 },
                                      aligner.Rows().ToList()[0].Entries.Select(cell => Int32.Parse(cell.Content)).ToList());
        }

        [TestMethod]
        public void CursorNavigatesInASingleRowCorrectly()
        {
            var cursor = new Cursor();
            var appender = new CellAppender((ch) =>
            {
                return new Cell(ch.ToString(), 10, 0);
            });
            var aligner = new CellAligner(() => 70.0f, appender.GetCells);
            appender.OnCellAppend((sender, args) => aligner.ArrangeRows());
            appender.OnCellAppend((sender, args) =>
            {
                cursor.Update(aligner.Rows());
                cursor.Right();
            });

            string twoRows = "123456789";
            for (int charIndex = 0; charIndex < twoRows.Count(); charIndex++)
            {
                appender.AddCell(twoRows[charIndex]);
                Assert.AreEqual(charIndex + 1, cursor.Head);
            }

        }

        [TestMethod]
        public void CursorNavigatesBetweenMultipleRowsCorrectly()
        {
            var cursor = new Cursor();
            var appender = new CellAppender((ch) =>
            {
                return new Cell(ch.ToString(), 10, 0);
            });
            var aligner = new CellAligner(() => 70.0f, appender.GetCells);
            appender.OnCellAppend((sender, args) => aligner.ArrangeRows());
            appender.OnCellAppend((sender, args) =>
            {
                cursor.Update(aligner.Rows());
                cursor.Right();
            });

            string twoRows = "123456789";
            for (int charIndex = 0; charIndex < twoRows.Count(); charIndex++)
            {
                appender.AddCell(twoRows[charIndex]);
            }
            Assert.AreEqual(9, cursor.Head);

            cursor.Up();
            Assert.AreEqual(2, cursor.Head);
            cursor.Up();
            Assert.AreEqual(2, cursor.Head);

            cursor.Down();
            Assert.AreEqual(9, cursor.Head);
            cursor.Down();
            Assert.AreEqual(9, cursor.Head);

            cursor.Left();
            cursor.Left();
            Assert.AreEqual(7, cursor.Head);

            appender.AddCell('1');
            Assert.AreEqual(8, cursor.Head);

            Enumerable.Range(1, 50).ToList().ForEach(x => cursor.Right());
            Assert.AreEqual(10, cursor.Head);

            Enumerable.Range(1, 50).ToList().ForEach(x => cursor.Left());
            Assert.AreEqual(0, cursor.Head);
        }

        [TestMethod]
        public void FixedSizeListLimitOverflowTest()
        {
            int listCapacity = 5;
            var fixedSizeList = new FixedSizeList<int>(listCapacity);

            for (int i = 0; i < listCapacity; i++)
            {
                fixedSizeList.Add(i);
            }
            for (int i = listCapacity; i > 0; i--)
            {
                fixedSizeList.Add(i);
            }

            CollectionAssert.AreEqual(new List<int> { 5, 4, 3, 2, 1 }, fixedSizeList.Query(x => x.Select(item => item)).ToList());
        }

        #region Terminal

        [TestMethod]
        public void TerminalRegistersMethodWithCommandAttribute()
        {
            Terminal terminal = new Terminal();
            MyTest objWithCommand = new ClassWithCommand();
            terminal.RegisterCommand(objWithCommand);

            Assert.AreEqual(2, terminal.Commands.Count);
        }

        [TestMethod]
        public void TerminalRegistersMethodWithSubCommandAttribute()
        {
            Terminal terminal = new Terminal();
            MyTest objWithCommand = new ClassWithCommand();
            terminal.RegisterCommand(objWithCommand);
            Assert.AreEqual(2, terminal.Commands.Count);
            var subcommands = terminal.Commands.Select(x=>x.SubCommand);
            Assert.AreEqual(1, subcommands.Count());
        }


        internal interface MyTest { }
        public class ClassWithCommand : MyTest
        {
            [Command(command: "firstCommand", description: "just a test")]
            private Result FirstCommandCallback(ICommandHost host,
                                                IList<string> arguments, 
                                                object executionResult)
            {
                return Result.Ok();
            }
            [Command(command: "write", 
                     description: "Writes the specified argument to the standard output stream")]
            public Result ConsoleWriteCallback(ICommandHost host,
                                               IList<string> arguments,
                                               object executionResult)
            {
                if(arguments.Count==1)
                {
                    System.Console.WriteLine(arguments[0]);
                    return Result.Ok();
                }
                return Result.Fail("Wrong number of arguments");
            }

            [Subcommand(parentCommand: "write",
                        subCommand:"color",
                        description: "[red|blue|green]")]
            private Result SubCommandCallback(ICommandHost host,
                                                IList<string> arguments,
                                                object executionResult)
            {
                return Result.Ok();
            }
        }

        #endregion
    }
}
