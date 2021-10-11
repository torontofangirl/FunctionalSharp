﻿using System;
using FPLibrary;
using FsCheck.Xunit;
using Xunit;
using static FPLibrary.Tests.Utils;
using static FPLibrary.F;

namespace FPLibrary.Tests.Map {
    public class MapEqualityTests {
        [Property(Arbitrary = new[] { typeof(ArbitraryMap) })]
        public void Equal_HashCode_Holds(Map<int, bool> map1, Map<int, bool> map2) {
            if (map1 == map2)
                Assert.Equal(map1.GetHashCode(), map2.GetHashCode());
            else
                Assert.NotEqual(map1.GetHashCode(), map2.GetHashCode());
        }
        
        [Property(Arbitrary = new[] { typeof(ArbitraryMap) })]
        public void HashCode_NoThrow_Holds(Map<int, bool> map) {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            /*Assert.DoesNotThrow(*/map.GetHashCode();
        }
        
        //https://stackoverflow.com/questions/7278136/create-hash-value-on-a-list
        [Fact(Skip = "dont think this applies for maps")]
        public void HashCode_Order_Matters() {
            var expected = Map<string, int>(("foo", 0), ("bar", 0), ("spam", 0)).GetHashCode();
            var actual = Map<string, int>(("spam", 0), ("bar", 0), ("foo", 0)).GetHashCode();
            
            Assert.NotEqual(expected, actual);
        }

        [Fact(Skip = "dont think this applies for maps")]
        public void HashCode_Duplicates_Matter() {
            var a = Map<string, int>(("foo", 0), ("bar", 0), ("spam", 0)).GetHashCode();
            var b = Map<string, int>(("foo", 0), ("bar", 0), ("spam", 0), ("foo", 0), ("foo", 0)).GetHashCode();
            var c = Map<string, int>(("foo", 0), ("bar", 0), ("spam", 0), ("foo", 0), ("foo", 0), ("spam", 0), ("foo", 0), ("spam", 0), ("foo", 0)).GetHashCode();
            
            Assert.False(a == b && b == c && a == c);
        }
        
        [Fact]
        public void Equals_Operator_Equals() {
            var lhs = Map<int, bool>((0, true), (4, false));
            var rhs = Map<int, bool>((0, true), (4, false));
            
            Assert.True(lhs == rhs);
        }
        
        [Fact]
        public void Equals_Method_Equals() {
            var lhs = Map<int, bool>((0, true), (4, false));
            var rhs = Map<int, bool>((0, true), (4, false));
            
            Assert.True(lhs.Equals(rhs));
        }
        
        [Fact]
        public void Equals_Operator_NotEqual() {
            var lhs = Map<int, bool>((0, true), (4, false));
            var rhs = Map<int, bool>((1, true), (4, false));
            
            Assert.False(lhs == rhs);
        }
        
        [Fact]
        public void Equals_Method_NotEqual() {
            var lhs = Map<int, bool>((0, true), (4, false));
            var rhs = Map<int, bool>((1, true), (4, false));
            
            Assert.False(lhs.Equals(rhs));
        }
    }
}