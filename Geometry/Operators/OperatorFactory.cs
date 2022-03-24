using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
    public static class OperatorFactory
    {
        public static IOperator CreateIntersectionOperator()
        {
            return new IntersectionOperator();
        }
        public static IOperator CreateUnionOperator()
        {
            return new UnionOperator();
        }
        public static IOperator CreateExclusionOperator()
        {
            return new ExclusionOperator();
        }
    }
}
