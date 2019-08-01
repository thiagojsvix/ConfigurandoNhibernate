using System;
using System.Collections;

namespace TesteMapeamento.Comparador
{
    public class ComparadorData : IEqualityComparer
    {
        public new bool Equals(object x, object y)
        {
            if (x is DateTime && y is DateTime)
            {
                return x.ToString().Equals(y.ToString());
            }
            else
            {
                return x.Equals(y);
            }
        }

        public int GetHashCode(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
