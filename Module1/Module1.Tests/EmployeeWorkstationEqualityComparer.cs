using Sample03.E3SClient.Entities;
using System;
using System.Collections;

namespace Module1.Tests
{
    internal class EmployeeWorkstationEqualityComparer : IComparer
    {
        public EmployeeWorkstationEqualityComparer()
        {
        }

        public int Compare(object x, object y)
        {
            if (x == null && y == null)
                return 0;
            var first = x as EmployeeEntity;
            var second = y as EmployeeEntity;
            if (first == null && second == null)
                return 0;
            if (first == null && second != null)
                return -1;
            if (first != null && second == null)
                return 1;
            return StringComparer.InvariantCultureIgnoreCase.Compare(first.workstation, second.workstation);
        }
    }
}