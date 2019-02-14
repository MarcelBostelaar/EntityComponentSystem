namespace Testnamespace
{
	class test{
		public int field1;
		public test2 test10;
		public test3 test11;
		
		public test(int field1, test2 test10, test3 test11){
			this.field1 = field1;
			this.test10 = test10;
			this.test11 = test11;
		}
		
		public test DeepCopy(){
			return new test(
				this.field1,
				this.test10.DeepCopy(),
				this.test11.DeepCopy()
			);
		}
	}
}