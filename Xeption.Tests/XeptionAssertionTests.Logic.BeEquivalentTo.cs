// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Force.DeepCloner;
using Xunit;
using Xunit.Sdk;

namespace Xeptions.Tests
{
    public partial class XeptionAssertionTests
    {
        [Fact(DisplayName = "01.0 - Level 0 - BeEquivalentToShouldPassIfNullExceptionsMatch")]
        public void BeEquivalentToShouldPassIfNullExceptionsMatch()
        {
            // given
            Xeption expectedException = null;
            Xeption actualException = null;

            // when then
            actualException.Should().BeEquivalentTo(expectedException);
        }

        [Fact(DisplayName = "02.1 - Level 0 - BeEquivalentToShouldPassIfExceptionsMatch")]
        public void BeEquivalentToShouldPassIfExceptionsMatch()
        {
            // given
            string randomMessage = GetRandomString();
            var expectedException = new Xeption(message: randomMessage);
            var actualException = new Xeption(message: randomMessage);

            // when then
            actualException.Should().BeEquivalentTo(expectedException);
        }

        [Fact(DisplayName = "02.2 - Level 0 - BeEquivalentToShouldFailIfExceptionsDontMatchOnType")]
        public void BeEquivalentToShouldFailIfExceptionsDontMatchOnType()
        {
            // given
            string randomMessage = GetRandomString();
            var expectedException = new Xeption(message: randomMessage);
            var actualException = new Exception(message: randomMessage);

            string expectedMessage =
                $"Expected exception to be \"{expectedException.GetType().FullName}\", " +
                $"but found \"{actualException.GetType().FullName}\"";

            // when
            Action assertAction = () =>
                actualException.Should().BeEquivalentTo(expectedException);

            XunitException actualError =
                Assert.Throws<XunitException>(assertAction);

            //then
            actualError.Message.Should().Contain(expectedMessage);
        }

        [Fact(DisplayName = "02.3 - Level 0 - BeEquivalentToShouldFailIfExceptionMessagesDontMatch")]
        public void BeEquivalentToShouldFailIfExceptionMessagesDontMatch()
        {
            // given
            var expectedException = new Xeption(message: GetRandomString());
            var actualException = new Xeption(message: GetRandomString());

            string expectedMessage =
                $"Expected exception message to be \"{expectedException.Message}\", " +
                $"but found \"{actualException.Message}\"";

            // when
            Action assertAction = () =>
                actualException.Should().BeEquivalentTo(expectedException);

            XunitException actualError =
                Assert.Throws<XunitException>(assertAction);

            //then
            actualError.Message.Should().Contain(expectedMessage);
        }

        [Fact(DisplayName = "02.4 - Level 0 - BeEquivalentToShouldPassIfExceptionDataMatch")]
        public void BeEquivalentToShouldPassIfExceptionDataMatch()
        {
            // given
            string exceptionMessage = GetRandomString();
            KeyValuePair<string, List<string>> randomData = GenerateKeyValuePair(count: 1);
            KeyValuePair<string, List<string>> expectedData = randomData.DeepClone();
            KeyValuePair<string, List<string>> actualData = randomData.DeepClone();

            var expectedException = new Xeption(
                message: exceptionMessage);

            expectedException.AddData(
                key: expectedData.Key,
                values: expectedData.Value.ToArray());

            var actualException = new Xeption(
                message: exceptionMessage);

            actualException.AddData(
                key: actualData.Key,
                values: actualData.Value.ToArray());

            // when then
            actualException.Should().BeEquivalentTo(expectedException);
        }

        [Fact(DisplayName = "02.5 - Level 0 - BeEquivalentToShouldFailIfExceptionDataDontMatch")]
        public void BeEquivalentToShouldFailIfExceptionDataDontMatch()
        {
            // given
            string exceptionMessage = GetRandomString();
            string mutualKey = $"mutual-{GetRandomString()}";
            KeyValuePair<string, List<string>> expectedDataOne = GenerateKeyValuePair(count: 1);
            KeyValuePair<string, List<string>> expectedDataTwo = GenerateKeyValuePair(count: 1);
            KeyValuePair<string, List<string>> actualData = GenerateKeyValuePair(count: 1);

            KeyValuePair<string, List<string>> expectedDataSameKeyName =
                GenerateKeyValuePair(count: 1, keyName: mutualKey);

            KeyValuePair<string, List<string>> actualDataSameKeyName =
                GenerateKeyValuePair(count: 1, keyName: mutualKey);

            var expectedException = new Xeption(
                message: exceptionMessage);

            expectedException.AddData(
                key: expectedDataOne.Key,
                values: expectedDataOne.Value.ToArray());

            expectedException.AddData(
                key: expectedDataTwo.Key,
                values: expectedDataTwo.Value.ToArray());

            expectedException.AddData(
                key: expectedDataSameKeyName.Key,
                values: expectedDataSameKeyName.Value.ToArray());

            var actualException = new Xeption(
                message: exceptionMessage);

            actualException.AddData(
                key: actualDataSameKeyName.Key,
                values: actualDataSameKeyName.Value.ToArray());

            actualException.AddData(
                key: actualData.Key,
                values: actualData.Value.ToArray());

            var expectedError = new StringBuilder();
            expectedError.AppendLine($"Expected exception to:");

            expectedError.AppendLine(
                $"- have a data count of {expectedException.Data.Count}, " +
                $"but found {actualException.Data.Count}");

            expectedError.AppendLine(
                $"- NOT contain key \"{actualData.Key}\"");

            expectedError.AppendLine(
                $"- contain key \"{expectedDataOne.Key}\" with value(s) ['{expectedDataOne.Value[0]}']");

            expectedError.AppendLine(
                $"- contain key \"{expectedDataTwo.Key}\" with value(s) ['{expectedDataTwo.Value[0]}']");

            expectedError.AppendLine(
                $"- have key \"{mutualKey}\" with value(s) ['{expectedDataSameKeyName.Value[0]}'], " +
                $"but found value(s) ['{actualDataSameKeyName.Value[0]}']");

            // when
            Action assertAction = () =>
                actualException.Should().BeEquivalentTo(expectedException);

            XunitException actualError =
                Assert.Throws<XunitException>(assertAction);

            //then
            actualError.Message.Should().BeEquivalentTo(expectedError.ToString().TrimEnd('\r', '\n'));
        }

        [Fact(DisplayName = "03.1 - Level 0 - BeEquivalentToShouldPassIfInnerExceptionsMatch")]
        public void BeEquivalentToShouldPassIfInnerExceptionsMatchOnType()
        {
            // given
            string randomMessage = GetRandomString();
            Xeption expectedInnerException = new Xeption(message: randomMessage);
            Xeption actualInnerException = new Xeption(message: randomMessage);

            var expectedException = new Xeption(
                message: randomMessage,
                innerException: expectedInnerException);

            var actualException = new Xeption(
                message: randomMessage,
                innerException: actualInnerException);

            // when then
            actualException.Should().BeEquivalentTo(expectedException);
        }

        [Fact(DisplayName = "03.2 - Level 1 - BeEquivalentToShouldFailIfInnerExceptionsTypeDontMatch")]
        public void BeEquivalentToShouldFailIfInnerExceptionsTypeDontMatch()
        {
            // given
            string randomMessage = GetRandomString();
            Xeption expectedInnerException = new Xeption(message: randomMessage);
            Exception actualInnerException = new Exception(message: randomMessage);

            var expectedException = new Xeption(
                message: randomMessage,
                innerException: expectedInnerException);

            var actualException = new Xeption(
                message: randomMessage,
                innerException: actualInnerException);

            string expectedMessage =
                $"Expected inner exception (level 1) to be \"{expectedInnerException.GetType().FullName}\", " +
                $"but found \"{actualInnerException.GetType().FullName}\"";

            // when
            Action assertAction = () =>
                actualException.Should().BeEquivalentTo(expectedException);

            XunitException actualError =
                Assert.Throws<XunitException>(assertAction);

            //then
            actualError.Message.Should().Contain(expectedMessage);
        }

        [Fact(DisplayName = "03.3 - Level 1 - BeEquivalentToShouldFailIfInnerExceptionMessageDontMatch")]
        public void BeEquivalentToShouldFailIfInnerExceptionMessageDontMatch()
        {
            // given
            string randomMessage = GetRandomString();
            var expectedInnerException = new Xeption(message: GetRandomString());
            var actualInnerException = new Xeption(message: GetRandomString());

            var expectedException = new Xeption(
                message: randomMessage,
                innerException: expectedInnerException);

            var actualException = new Xeption(
                message: randomMessage,
                innerException: actualInnerException);

            string expectedMessage =
                $"Expected inner exception (level 1) message to be \"{expectedInnerException.Message}\", " +
                $"but found \"{actualInnerException.Message}\"";

            // when
            Action assertAction = () =>
                actualException.Should().BeEquivalentTo(expectedException);

            XunitException actualError =
                Assert.Throws<XunitException>(assertAction);

            //then
            actualError.Message.Should().Contain(expectedMessage);
        }

        [Fact(DisplayName = "03.4 - Level 1 - BeEquivalentToShouldPassIfInnerExceptionDataMatch")]
        public void BeEquivalentToShouldPassIfInnerExceptionDataMatch()
        {
            // given
            string exceptionMessage = GetRandomString();
            KeyValuePair<string, List<string>> randomData = GenerateKeyValuePair(count: 1);
            KeyValuePair<string, List<string>> expectedData = randomData.DeepClone();
            KeyValuePair<string, List<string>> actualData = randomData.DeepClone();

            var expectedInnerException = new Xeption(
                message: exceptionMessage);

            expectedInnerException.AddData(
                key: expectedData.Key,
                values: expectedData.Value.ToArray());

            var actualInnerException = new Xeption(
                message: exceptionMessage);

            actualInnerException.AddData(
                key: actualData.Key,
                values: actualData.Value.ToArray());

            var expectedException = new Xeption(
                message: exceptionMessage,
                innerException: expectedInnerException);

            var actualException = new Xeption(
                message: exceptionMessage,
                innerException: actualInnerException);

            // when then
            actualException.Should().BeEquivalentTo(expectedException);
        }

        [Fact(DisplayName = "03.5 - Level 1 - BeEquivalentToShouldFailIfInnerExceptionDataDontMatch")]
        public void BeEquivalentToShouldFailIfAggregateInnerExceptionDataDontMatch()
        {
            // given
            string exceptionMessage = GetRandomString();
            string mutualKey = GetRandomString();
            KeyValuePair<string, List<string>> expectedDataOne = GenerateKeyValuePair(count: 1);
            KeyValuePair<string, List<string>> expectedDataTwo = GenerateKeyValuePair(count: 1);
            KeyValuePair<string, List<string>> actualData = GenerateKeyValuePair(count: 1);
            KeyValuePair<string, List<string>> expectedDataSameKeyName = GenerateKeyValuePair(count: 1, mutualKey);
            KeyValuePair<string, List<string>> actualDataSameKeyName = GenerateKeyValuePair(count: 1, mutualKey);

            var expectedInnerException = new Xeption(
                message: exceptionMessage);

            expectedInnerException.AddData(
                key: expectedDataOne.Key,
                values: expectedDataOne.Value.ToArray());

            expectedInnerException.AddData(
                key: expectedDataTwo.Key,
                values: expectedDataTwo.Value.ToArray());

            expectedInnerException.AddData(
                key: expectedDataSameKeyName.Key,
                values: expectedDataSameKeyName.Value.ToArray());

            var actualInnerException = new Xeption(
                message: exceptionMessage);

            actualInnerException.AddData(
                key: actualDataSameKeyName.Key,
                values: actualDataSameKeyName.Value.ToArray());

            actualInnerException.AddData(
                key: actualData.Key,
                values: actualData.Value.ToArray());

            var expectedError = new StringBuilder();
            expectedError.AppendLine($"Expected exception to:");

            expectedError.AppendLine(
                $"- have a data count of {expectedInnerException.Data.Count}, " +
                $"but found {actualInnerException.Data.Count}");

            expectedError.AppendLine(
                $"- NOT contain key \"{actualData.Key}\"");

            expectedError.AppendLine(
                $"- contain key \"{expectedDataOne.Key}\" with value(s) ['{expectedDataOne.Value[0]}']");

            expectedError.AppendLine(
                $"- contain key \"{expectedDataTwo.Key}\" with value(s) ['{expectedDataTwo.Value[0]}']");

            expectedError.AppendLine(
                $"- have key \"{mutualKey}\" with value(s) ['{expectedDataSameKeyName.Value[0]}'], " +
                $"but found value(s) ['{actualDataSameKeyName.Value[0]}']");

            var expectedException = new Xeption(
                message: exceptionMessage,
                innerException: expectedInnerException);

            var actualException = new Xeption(
                message: exceptionMessage,
                innerException: actualInnerException);

            // when
            Action assertAction = () =>
                actualException.Should().BeEquivalentTo(expectedException);

            XunitException actualError =
                Assert.Throws<XunitException>(assertAction);

            //then
            actualError.Message.Should().BeEquivalentTo(expectedError.ToString().TrimEnd('\r', '\n'));
        }

        [Fact(DisplayName = "04.1 - Level 2 - BeEquivalentToShouldPassIfInnerInnerExceptionsMatch")]
        public void BeEquivalentToShouldPassIfInnerInnerExceptionsMatch()
        {
            // given
            string randomMessage = GetRandomString();
            Xeption expectedInnerInnerException = new Xeption(message: randomMessage);
            Xeption actualInnerInnerException = new Xeption(message: randomMessage);

            Xeption expectedInnerException = new Xeption(
                message: randomMessage,
                innerException: expectedInnerInnerException);

            Xeption actualInnerException = new Xeption(
                message: randomMessage,
                innerException: actualInnerInnerException);

            var expectedException = new Xeption(
                message: randomMessage,
                innerException: expectedInnerException);

            var actualException = new Xeption(
                message: randomMessage,
                innerException: actualInnerException);

            // when then
            actualException.Should().BeEquivalentTo(expectedException);
        }

        [Fact(DisplayName = "04.2 - Level 2 - BeEquivalentToShouldFailIfInnerInnerExceptionsTypeDontMatch")]
        public void BeEquivalentToShouldFailIfInnerInnerExceptionsTypeDontMatch()
        {
            // given
            string randomMessage = GetRandomString();
            Xeption expectedInnerInnerException = new Xeption(message: randomMessage);
            Exception actualInnerInnerException = new Exception(message: randomMessage);

            Xeption expectedInnerException = new Xeption(
                message: randomMessage,
                innerException: expectedInnerInnerException);

            Xeption actualInnerException = new Xeption(
                message: randomMessage,
                innerException: actualInnerInnerException);

            var expectedException = new Xeption(
                message: randomMessage,
                innerException: expectedInnerException);

            var actualException = new Xeption(
                message: randomMessage,
                innerException: actualInnerException);

            string expectedMessage =
                $"Expected inner exception (level 2) to be \"{expectedInnerInnerException.GetType().FullName}\", " +
                $"but found \"{actualInnerInnerException.GetType().FullName}\"";

            // when
            Action assertAction = () =>
                actualException.Should().BeEquivalentTo(expectedException);

            XunitException actualError =
                Assert.Throws<XunitException>(assertAction);

            //then
            actualError.Message.Should().Contain(expectedMessage);
        }

        [Fact(DisplayName = "04.3 - Level 2 - BeEquivalentToShouldFailIfInnerInnerExceptionMessageDontMatch")]
        public void BeEquivalentToShouldFailIfInnerInnerExceptionMessageDontMatch()
        {
            // given
            string randomMessage = GetRandomString();
            var expectedInnerInnerException = new Xeption(message: GetRandomString());
            var actualInnerInnerException = new Xeption(message: GetRandomString());

            Xeption expectedInnerException = new Xeption(
                message: randomMessage,
                innerException: expectedInnerInnerException);

            Xeption actualInnerException = new Xeption(
                message: randomMessage,
                innerException: actualInnerInnerException);

            var expectedException = new Xeption(
                message: randomMessage,
                innerException: expectedInnerException);

            var actualException = new Xeption(
                message: randomMessage,
                innerException: actualInnerException);

            string expectedMessage =
                $"Expected inner exception (level 2) message to be \"{expectedInnerInnerException.Message}\", " +
                $"but found \"{actualInnerInnerException.Message}\"";

            // when
            Action assertAction = () =>
                actualException.Should().BeEquivalentTo(expectedException);

            XunitException actualError =
                Assert.Throws<XunitException>(assertAction);

            //then
            actualError.Message.Should().Contain(expectedMessage);
        }

        [Fact(DisplayName = "04.4 - Level 2 - BeEquivalentToShouldPassIfInnerInnerExceptionDataMatch")]
        public void BeEquivalentToShouldPassIfInnerInnerExceptionDataMatch()
        {
            // given
            string exceptionMessage = GetRandomString();
            KeyValuePair<string, List<string>> randomData = GenerateKeyValuePair(count: 1);
            KeyValuePair<string, List<string>> expectedData = randomData.DeepClone();
            KeyValuePair<string, List<string>> actualData = randomData.DeepClone();

            var expectedInnerInnerException = new Xeption(
                message: exceptionMessage);

            expectedInnerInnerException.AddData(
                key: expectedData.Key,
                values: expectedData.Value.ToArray());

            var actualInnerInnerException = new Xeption(
                message: exceptionMessage);

            actualInnerInnerException.AddData(
                key: actualData.Key,
                values: actualData.Value.ToArray());

            Xeption expectedInnerException = new Xeption(
                message: exceptionMessage,
                innerException: expectedInnerInnerException);

            Xeption actualInnerException = new Xeption(
                message: exceptionMessage,
                innerException: actualInnerInnerException);

            var expectedException = new Xeption(
                message: exceptionMessage,
                innerException: expectedInnerException);

            var actualException = new Xeption(
                message: exceptionMessage,
                innerException: actualInnerException);

            // when then
            actualException.Should().BeEquivalentTo(expectedException);
        }

        [Fact(DisplayName = "04.5 - Level 2 - BeEquivalentToShouldFailIfInnerInnerExceptionDataDontMatch")]
        public void BeEquivalentToShouldFailIfAggregateInnerInnerExceptionDataDontMatch()
        {
            // given
            string exceptionMessage = GetRandomString();
            string mutualKey = GetRandomString();
            KeyValuePair<string, List<string>> expectedDataOne = GenerateKeyValuePair(count: 1);
            KeyValuePair<string, List<string>> expectedDataTwo = GenerateKeyValuePair(count: 1);
            KeyValuePair<string, List<string>> actualData = GenerateKeyValuePair(count: 1);
            KeyValuePair<string, List<string>> expectedDataSameKeyName = GenerateKeyValuePair(count: 1, mutualKey);
            KeyValuePair<string, List<string>> actualDataSameKeyName = GenerateKeyValuePair(count: 1, mutualKey);

            var expectedInnerInnerException = new Xeption(
                message: exceptionMessage);

            expectedInnerInnerException.AddData(
                key: expectedDataOne.Key,
                values: expectedDataOne.Value.ToArray());

            expectedInnerInnerException.AddData(
                key: expectedDataTwo.Key,
                values: expectedDataTwo.Value.ToArray());

            expectedInnerInnerException.AddData(
                key: expectedDataSameKeyName.Key,
                values: expectedDataSameKeyName.Value.ToArray());

            var actualInnerInnerException = new Xeption(
                message: exceptionMessage);

            actualInnerInnerException.AddData(
                key: actualDataSameKeyName.Key,
                values: actualDataSameKeyName.Value.ToArray());

            actualInnerInnerException.AddData(
                key: actualData.Key,
                values: actualData.Value.ToArray());

            Xeption expectedInnerException = new Xeption(
                message: exceptionMessage,
                innerException: expectedInnerInnerException);

            Xeption actualInnerException = new Xeption(
                message: exceptionMessage,
                innerException: actualInnerInnerException);

            var expectedError = new StringBuilder();
            expectedError.AppendLine($"Expected exception to:");

            expectedError.AppendLine(
                $"- have a data count of {expectedInnerInnerException.Data.Count}, " +
                $"but found {actualInnerInnerException.Data.Count}");

            expectedError.AppendLine(
                $"- NOT contain key \"{actualData.Key}\"");

            expectedError.AppendLine(
                $"- contain key \"{expectedDataOne.Key}\" with value(s) ['{expectedDataOne.Value[0]}']");

            expectedError.AppendLine(
                $"- contain key \"{expectedDataTwo.Key}\" with value(s) ['{expectedDataTwo.Value[0]}']");

            expectedError.AppendLine(
                $"- have key \"{mutualKey}\" with value(s) ['{expectedDataSameKeyName.Value[0]}'], " +
                $"but found value(s) ['{actualDataSameKeyName.Value[0]}']");

            var expectedException = new Xeption(
                message: exceptionMessage,
                innerException: expectedInnerException);

            var actualException = new Xeption(
                message: exceptionMessage,
                innerException: actualInnerException);

            // when
            Action assertAction = () =>
                actualException.Should().BeEquivalentTo(expectedException);

            XunitException actualError =
                Assert.Throws<XunitException>(assertAction);

            //then
            actualError.Message.Should().BeEquivalentTo(expectedError.ToString().TrimEnd('\r', '\n'));
        }











        [Fact(DisplayName = "05.1 - BeEquivalentToShouldPassIfAggregateExceptionsMatch")]
        public void BeEquivalentToShouldPassIfAggregateExceptionsMatch()
        {
            // given
            string randomMessage = GetRandomString();
            Xeption expectedInnerException = new Xeption(message: randomMessage);
            Xeption actualInnerException = new Xeption(message: randomMessage);
            string aggregateExceptionMessage = GetRandomString();

            AggregateException expectedAggregateException = new AggregateException(
                message: aggregateExceptionMessage,
                innerExceptions: new List<Exception> { expectedInnerException });

            AggregateException actualAggregateException = new AggregateException(
                message: aggregateExceptionMessage,
                innerExceptions: new List<Exception> { actualInnerException });

            var expectedException = new Xeption(
                message: randomMessage,
                innerException: expectedAggregateException);

            var actualException = new Xeption(
                message: randomMessage,
                innerException: actualAggregateException);

            // when then
            actualException.Should().BeEquivalentTo(expectedException);
        }

        [Fact(DisplayName = "05.2 - BeEquivalentToShouldFailIfAggregateExceptionsDontMatchOnMessage")]
        public void BeEquivalentToShouldFailIfAggregateExceptionsDontMatchOnMessage()
        {
            // given
            string randomMessage = GetRandomString();
            string aggregateExpectedMessage = GetRandomString();
            string aggregateActualMessage = GetRandomString();

            AggregateException expectedAggregateException = new AggregateException(
                message: aggregateExpectedMessage);

            AggregateException actualAggregateException = new AggregateException(
                message: aggregateActualMessage);

            var expectedException = new Xeption(
                message: randomMessage,
                innerException: expectedAggregateException);

            var actualException = new Xeption(
                message: randomMessage,
                innerException: actualAggregateException);

            string expectedMessage =
                $"Expected inner exception (level 1) message to be " +
                $"\"{expectedAggregateException.Message}\", " +
                $"but found \"{actualAggregateException.Message}\"";

            // when
            Action assertAction = () =>
                actualException.Should().BeEquivalentTo(expectedException);

            XunitException actualError =
                Assert.Throws<XunitException>(assertAction);

            //then
            actualError.Message.Should().Contain(expectedMessage);
        }

        [Fact(DisplayName = "05.3 - BeEquivalentToShouldFailIfAggregateInnerExceptionsMessagesDontMatch")]
        public void BeEquivalentToShouldFailIfAggregateInnerExceptionsMessagesDontMatch()
        {
            // given
            string randomMessage = GetRandomString();
            Xeption expectedInnerExceptionOne = new Xeption(message: GetRandomString());
            Xeption expectedInnerExceptionTwo = new Xeption(message: GetRandomString());

            AggregateException expectedAggregateException = new AggregateException(
                message: randomMessage,
                innerExceptions: new List<Exception> { expectedInnerExceptionOne, expectedInnerExceptionTwo });

            AggregateException actualAggregateException = new AggregateException(
                message: randomMessage);

            var expectedException = new Xeption(
                message: randomMessage,
                innerException: expectedAggregateException);

            var actualException = new Xeption(
                message: randomMessage,
                innerException: actualAggregateException);

            string expectedMessage =
                $"Expected inner exception (level 1) message to be " +
                $"\"{expectedAggregateException.Message}\", " +
                $"but found \"{actualAggregateException.Message}\"";

            // when
            Action assertAction = () =>
                actualException.Should().BeEquivalentTo(expectedException);

            XunitException actualError =
                Assert.Throws<XunitException>(assertAction);

            //then
            actualError.Message.Should().Contain(expectedMessage);
        }

        [Fact(DisplayName = "05.3 - BeEquivalentToShouldFailIfAggregateInnerExceptionsDataDontMatch")]
        public void BeEquivalentToShouldFailIfAggregateInnerExceptionsDataDontMatch()
        {
            // given
            string randomMessage = GetRandomString();
            Xeption expectedInnerException = new Xeption(message: randomMessage);
            Xeption actualInnerException = new Xeption(message: randomMessage);
            string sameKey = GetRandomString();
            string expectedSameValue = GetRandomString();
            string unexpectedSameValue = GetRandomString();
            string expectedKey = $"expected-{GetRandomString()}";
            string expectedValue = GetRandomString();
            string unexpectedKey = $"unexpected-{GetRandomString()}";
            string unexpectedValue = GetRandomString();

            expectedInnerException.AddData(
                key: expectedKey,
                values: new[] { expectedValue });

            expectedInnerException.AddData(
                key: sameKey,
                values: new[] { expectedSameValue });

            actualInnerException.AddData(
                key: unexpectedKey,
                values: new[] { unexpectedValue });

            actualInnerException.AddData(
                key: sameKey,
                values: new[] { unexpectedSameValue });

            AggregateException expectedAggregateException = new AggregateException(
                message: randomMessage,
                innerExceptions: new List<Exception> { expectedInnerException });

            AggregateException actualAggregateException = new AggregateException(
                message: randomMessage,
                innerExceptions: new List<Exception> { actualInnerException });

            var expectedException = new Xeption(
                message: randomMessage,
                innerException: expectedAggregateException);

            var actualException = new Xeption(
                message: randomMessage,
                innerException: actualAggregateException);

            var expectedMessage = new StringBuilder();
            expectedMessage.AppendLine($"Aggregate exception differences:");
            expectedMessage.AppendLine($"* Difference in inner exception at index[0] - Expected exception to:");
            expectedMessage.AppendLine($"- NOT contain key \"{unexpectedKey}\"");
            expectedMessage.AppendLine($"- contain key \"{expectedKey}\" with value(s) ['{expectedValue}']");

            expectedMessage.AppendLine($"- have key \"{sameKey}\" with value(s) ['{expectedSameValue}'], " +
                $"but found value(s) ['{unexpectedSameValue}']");

            // when
            Action assertAction = () =>
                actualException.Should().BeEquivalentTo(expectedException);

            XunitException actualError =
                Assert.Throws<XunitException>(assertAction);

            //then
            actualError.Message.Should().Contain(expectedMessage.ToString().TrimEnd('\r', '\n'));
        }
    }
}
