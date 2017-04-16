﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XamarinProject;

namespace DnD_Unit_tests
{
    [TestClass]
    public class ItemTest
    {
        Item item;

        [TestMethod]
        public void Item_Default_Constructor()
        {
            item = new Item();
        }

        [TestMethod]
        public void Item_Argument_Constructor()
        {
            Item item = new Item("Unicorn Helmet", "Made from dead Unicorns", 3, "Head", 0);
            Assert.AreSame(item, item);
        }
    }
}
