﻿using AutoFixture;
using AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests.Builders
{
    static class CsvTextBuilder
    {
        public static string Build(char separator, params string[] values)
        {
            var fixture = new Fixture();
            var builder = new StringBuilder();

            var header = DelimeterStringBuilder.Build(separator, values);

            builder.AppendLine(header);

            foreach(var arr in fixture.CreateMany<string[]>())
            {
                var shortenedArray = arr.Take(values.Count());

                builder.AppendLine(string.Join(separator, shortenedArray));
            }

            return builder.ToString();
        }

        public static string BuildTyped(char separator, int rowCount, params (Type type, string label)[] values)
        {
            var fixture = new Fixture();

            var builder = new StringBuilder(rowCount + 1);

            var header = DelimeterStringBuilder.Build(separator, values.Select(x => x.label).ToArray());

            builder.AppendLine(header);

            for(int i = 0; i < rowCount; i++)
            {
                var innerBuilder = new StringBuilder(values.Length);
                foreach (var value in values)
                {
                    var obj = new SpecimenContext(fixture).Resolve(value.type);

                    innerBuilder.Append(obj).Append(separator);
                }

                builder.AppendLine(innerBuilder.ToString().Trim(separator));
            }


            return builder.ToString();
        }
    }
}
