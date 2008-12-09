// <copyright file="ForestDisjointSetTTest.cs" company="Jonathan de Halleux">Copyright http://www.codeplex.com/quickgraph</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickGraph.Collections;

namespace QuickGraph.Collections
{
    /// <summary>This class contains parameterized unit tests for ForestDisjointSet`1</summary>
    [TestClass]
    [PexClass(typeof(ForestDisjointSet<>))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class ForestDisjointSetTTest
    {
        /// <summary>Test stub for AreInSameSet(!0, !0)</summary>
        [PexMethod(MaxConstraintSolverTime = 2, MaxConditions = 1000)]
        public void Unions(int elementCount, [PexAssumeNotNull]int[] unions)
        {
            PexAssume.IsTrue(elementCount > 0);
            PexSymbolicValue.Minimize(elementCount);

            var target = new ForestDisjointSet<int>();
            for (int i = 0; i < elementCount; i++)
            {
                target.MakeSet(i);
                Assert.AreEqual(i + 1, target.ElementCount);
                Assert.AreEqual(i + 1, target.SetCount);
            }

            for (int i = 0; i + 1 < unions.Length; i+=2)
            {
                var left = unions[i];
                var right= unions[i+1];
                PexAssume.IsTrue(target.Contains(left));
                PexAssume.IsTrue(target.Contains(right));
                var leftSet = target.FindSet(left);
                var rightSet = target.FindSet(right);
                if(left == right)
                    Assert.AreEqual(leftSet, rightSet);

                var setCount = target.SetCount;

                target.Union(left, right);

                if (leftSet != rightSet)
                    Assert.AreEqual(setCount - 1, target.SetCount);
                else
                    Assert.AreEqual(setCount, target.SetCount);
            }
        }
        /// <summary>Test stub for Contains(!0)</summary>
        [PexMethod]
        [PexGenericArguments(typeof(int))]
        public void Contains<T>(T value)
        {
            var target = new ForestDisjointSet<T>();
            target.MakeSet(value);
            Assert.IsTrue(target.Contains(value));
        }
    }
}
