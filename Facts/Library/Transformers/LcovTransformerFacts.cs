﻿using Chutzpah.Models;
using Chutzpah.Transformers;
using System;
using Xunit;

namespace Chutzpah.Facts.Library.Transformers
{
    public class LcovTransformerFacts
    {
        private static TestCaseSummary GetTestCaseSummary()
        {
            var toReturn = new TestCaseSummary();
            toReturn.CoverageObject = new CoverageData();

            toReturn.CoverageObject["/no/coverage"] = new CoverageFileData
            {
                FilePath = "/no/coverage",
                LineExecutionCounts = null
            };

            toReturn.CoverageObject["/some/lines"] = new CoverageFileData
            {
                FilePath = "/some/lines",
                LineExecutionCounts = new int?[] { 1, 2, null, 5, 0 }
            };

            return toReturn;
        }

        [Fact]
        public void Has_Appropriate_Name()
        {
            Assert.Equal("lcov", new LcovTransformer().Name);
        }

        [Fact]
        public void Has_Description()
        {
            Assert.False(string.IsNullOrWhiteSpace(new LcovTransformer().Description));
        }

        [Fact]
        public void Returns_Empty_String_If_No_Coverage_Data()
        {
            Assert.Equal(string.Empty, new LcovTransformer().Transform(new TestCaseSummary()));
        }

        [Fact]
        public void Throws_If_Null_TestCaseSummary_Supplied()
        {
            Exception ex = Record.Exception(() => new LcovTransformer().Transform(null));

            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void Outupts_Lcov_Data()
        {
            var expected =
@"SF:/no/coverage
end_of_record
SF:/some/lines
DA:1,2
DA:3,5
DA:4,0
end_of_record
";

            var actual = new LcovTransformer().Transform(GetTestCaseSummary());

            Assert.Equal(expected, actual);
        }
    }
}
