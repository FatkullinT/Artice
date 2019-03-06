namespace Artice.LogicCore
{
    public interface IServiceLocator
    {
        T Get<T>();

        void CreateScope();

        void DeleteScope();
    }
}