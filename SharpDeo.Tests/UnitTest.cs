using Xunit;

namespace SharpDeo.Tests {
	public class UnitTest {
		[Fact]
		public void PassingTest() {
			Assert.Equal (4, Add (2, 2));
		}

		[Fact]
		public void FailingTest() {
			Assert.Equal (5, Add (2, 2));
		}

		static int Add(int x, int y) => x + y;

		[Theory]
		[InlineData (3)]
		[InlineData (5)]
		[InlineData (6)]
		public void MyFirstTheory(int value) {
			Assert.True (IsOdd (value));
		}

		static bool IsOdd(int value) => value % 2 == 1;
	}
}