using Moq;

namespace Artice.Testing.Core
{
    public class BaseMock<TAbstraction> : Mock<TAbstraction>
        where TAbstraction : class
    {
        public virtual void Build()
        {
        }

        public override TAbstraction Object
        {
            get
            {
                Build();
                return base.Object;
            }
        }

        public BaseMock<TAbstraction> Returns<TValue>(TValue value)
        {
            base.SetReturnsDefault(value);
            return this;
        }
    }

    public abstract class BaseMock<TAbstraction, TMock> : BaseMock<TAbstraction>
        where TAbstraction : class
        where TMock : BaseMock<TAbstraction, TMock>
    {
        public new TMock Returns<TValue>(TValue value)
        {
            return base.Returns(value) as TMock;
        }
    }

}