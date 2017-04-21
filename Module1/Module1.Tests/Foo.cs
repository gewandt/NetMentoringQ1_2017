using System;

namespace Module1.Tests
{
    class Foo
    {
        public Foo()
        {
            failModule = 1;
        }
        public double FirstProp { get; set; }
        public string SecondProp { get; set; }
        public Guid ThirdProp { get; set; }
        public int OnlyGet { get; set; }
        private int failModule;

        public int Get()
        {
            return failModule;
        }
    }
}
