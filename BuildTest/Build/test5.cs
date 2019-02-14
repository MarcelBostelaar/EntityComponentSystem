namespace Testnamespace
{
	class test5{
		public int allo;
		
		public test5(int allo){
			this.allo = allo;
		}
		
		public test5 DeepCopy(){
			return new test5(
				this.allo
			);
		}
	}
}