using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Infrastructure.Crosscutting.Tests
{
    [TestFixture]
    public class CommonTest
    {
        [Test]
        public void Text_String_Format()
        {
            Console.WriteLine(MyEnum.Get.ToString("g"));
        }
        
        public enum MyEnum
        {
            Get,
            Post
        }
    }
}
