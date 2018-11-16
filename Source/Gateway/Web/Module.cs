using System;
using Autofac;

namespace Web
{
    /// <summary>
    /// 
    /// </summary>
    public class Module : Autofac.Module
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            Console.WriteLine("Hello");
        }
    }
}