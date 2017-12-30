using System;
using System.Collections.Generic;
using System.Text;

namespace ECS.Utilities
{
    public abstract class Union<A, B>
    {
        private Union() { }
        public abstract T Match<T>(Func<A, T> f, Func<B, T> g);
        public Union<A, B> Create(A from)
        {
            return new CaseA(from);
        }
        public Union<A, B> Create(B from)
        {
            return new CaseB(from);
        }

        public sealed class CaseA : Union<A,B>
        {
            public readonly A item;
            public CaseA(A value)
            {
                item = value;
            }

            public override T Match<T>(Func<A, T> f, Func<B, T> g)
            {
                return f(item);
            }
        }
        public sealed class CaseB : Union<A, B>
        {
            public readonly B item;
            public CaseB(B value)
            {
                item = value;
            }
            public override T Match<T>(Func<A, T> f, Func<B, T> g)
            {
                return g(item);
            }
        }
    }
}
