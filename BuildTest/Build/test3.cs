namespace Testnamespace
{
	class test3{
		public test5 allo;
		
		public test3(test5 allo){
			this.allo = allo;
		}
		
		public test3 DeepCopy(){
			return new test3(
				this.allo.DeepCopy()
			);
		}
	}
}