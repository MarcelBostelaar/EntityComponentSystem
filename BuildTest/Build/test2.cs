namespace Testnamespace
{
	class test2{
		public int field1;
		
		public test2(int field1){
			this.field1 = field1;
		}
		
		public test2 DeepCopy(){
			return new test2(
				this.field1
			);
		}
	}
}