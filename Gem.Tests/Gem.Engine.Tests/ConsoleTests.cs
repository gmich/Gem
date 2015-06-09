using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gem.Console;
using System.Linq;
using System.Collections.Generic;

namespace Gem.Engine.Tests
{
    [TestClass]
    public class ConsoleTests
    {
        [TestMethod]
        public void ConsoleAlignerAlignsCorrectly()
        {
            var appender = new CellAppender((ch) =>
            {
                return new Cell(ch.ToString(), 10, 0);
            });

            var aligner = new CellAligner(() => 70.0f, appender.GetCells);

            appender.OnCollectionChanged((sender, args) => aligner.ArrangeRows());

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
            CollectionAssert.AreEqual(new List<int>{8,9},
                                      aligner.Rows().ToList()[1].Entries.Select(cell=>Int32.Parse(cell.Content)).ToList());
        }
    }
}
