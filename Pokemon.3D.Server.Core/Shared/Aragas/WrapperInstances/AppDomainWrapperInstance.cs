//using System;
//using System.Reflection;
//using Aragas.Core.Wrappers;

//namespace Pokemon_3D_Server_Core.Shared.Aragas.WrapperInstances
//{
//    /// <summary>
//    /// Class containing AppDomain Wrapper Instances.
//    /// </summary>
//    public class AppDomainWrapperInstance : IAppDomain
//    {
//        /// <summary>
//        /// Gets the currently loaded assembly in which the specified class is defined.
//        /// </summary>
//        /// <param name="type">An object representing a class in the assembly that will be returned.</param>
//        public Assembly GetAssembly(Type type)
//        {
//            return Assembly.GetAssembly(type);
//        }

//        /// <summary>
//        /// Gets the assemblies that have been loaded into the execution context of this application domain.
//        /// </summary>
//        public Assembly[] GetAssemblies()
//        {
//            return AppDomain.CurrentDomain.GetAssemblies();
//        }

//        /// <summary>
//        /// Loads the assembly with a common object file format (COFF)-based image containing
//        /// an emitted assembly. The assembly is loaded into the application domain of the
//        /// caller.
//        /// </summary>
//        /// <param name="assemblyData">A byte array that is a COFF-based image containing an emitted assembly.</param>
//        public Assembly LoadAssembly(byte[] assemblyData)
//        {
//            return Assembly.Load(assemblyData);
//        }
//    }
//}