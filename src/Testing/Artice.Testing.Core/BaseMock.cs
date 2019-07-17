using Moq;

namespace Artice.Testing.Core
{
	public abstract class BaseMock<TAbstraction, TMock> : Mock<TAbstraction>
		where TAbstraction: class
		where TMock : BaseMock<TAbstraction, TMock>
	{
		public virtual void Build()
		{ }

		public override TAbstraction Object
		{
			get
			{
				Build();
				return base.Object;
			}
		}
	}
}