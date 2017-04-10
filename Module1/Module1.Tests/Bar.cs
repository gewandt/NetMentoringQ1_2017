using System;

namespace Module1.Tests
{
    class Bar
    {
        public double FirstProp { get; set; }
        public string SecondProp { get; set; }
        public Guid ThirdProp { get; set; }
        public int FourthProp { get; set; }
        public int OnlyGet { get; }
        private int failModule;
    }
}
