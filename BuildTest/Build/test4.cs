namespace Testnamespace
{
	class test4{
		public test2 allo;
		
		public test4(test2 allo){
			this.allo = allo;
		}
		
		public test4 DeepCopy(){
			return new test4(
				this.allo.DeepCopy()
			);
		}
	}
}