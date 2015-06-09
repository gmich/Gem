
namespace Gem.Console
{
    interface IUpdateable
    {
        void Update(double time);

        //how to register
            //        Autofac.ContainerBuilder builder = null;
            
            //builder.RegisterAssemblyTypes(this.GetType().Assembly)
            //       .Where(t => t.IsAssignableFrom(typeof(IUpdateable)))
            //       .AsImplementedInterfaces();
    }
}
