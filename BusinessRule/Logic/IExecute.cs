using BusinessRule.Model;

namespace BusinessRule.Logic
{
    public interface IExecute<in T> where T :class,new()
    {
        void DoStuff(T message);
    }
}