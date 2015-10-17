using System;
using System.Reflection;
using Aragas.Core.Wrappers;

namespace Pokemon_3D_Server_Core.SCON_Client_Listener.WrapperInstances
{
    public class AppDomainWrapperInstance : IAppDomain
    {
        public Assembly GetAssembly(Type type)
        {
            return Assembly.GetAssembly(type);
        }

        public Assembly[] GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        public Assembly LoadAssembly(byte[] assemblyData)
        {
            return Assembly.Load(assemblyData);
        }
    }
}
