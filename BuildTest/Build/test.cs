namespace Testnamespace
{
	class test{
		public int test;
		public int test1;
		public int test2;
		public int test3;
		public int test4;
		public int test5;
		public int test6;
		public int test7;
		public int test8;
		public float test9;
		public int test10;
		public int test11;
		
		public test(int test, int test1, int test2, int test3, int test4, int test5, int test6, int test7, int test8, float test9, int test10, int test11){
			this.test = test;
			this.test1 = test1;
			this.test2 = test2;
			this.test3 = test3;
			this.test4 = test4;
			this.test5 = test5;
			this.test6 = test6;
			this.test7 = test7;
			this.test8 = test8;
			this.test9 = test9;
			this.test10 = test10;
			this.test11 = test11;
		}
		
		public test DeepCopy(){
			return new test(
				this.test,
				this.test1,
				this.test2,
				this.test3,
				this.test4,
				this.test5,
				this.test6,
				this.test7,
				this.test8,
				this.test9,
				this.test10,
				this.test11
			);
		}
	}
}