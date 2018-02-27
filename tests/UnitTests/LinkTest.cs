using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shally.Hal;

namespace Shally
{
   [TestClass]
   public class LinkTest
   {
      [TestMethod]
      public void Constructor_Success()
      {
         var link = Factory.GetValidLink();
      }

      [TestMethod]
      public void Constructor_Success_WhenHrefIsEmpty()
      {
         var link = new Link(string.Empty);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentNullException))]
      public void Constructor_ThrowsArgumentException_WhenHrefIsNull()
      {
         var link = new Link(null);
      }
   }
}