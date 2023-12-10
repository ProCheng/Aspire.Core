﻿using Aspire.Core.AOP;
using Aspire.Core.Common;
using Aspire.Core.IRepository.Base;
using Aspire.Core.IServices.BASE;
using Aspire.Core.Model;
using Aspire.Core.Repository.Base;
using Aspire.Core.Repository.UnitOfWorks;
using Aspire.Core.Services.BASE;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Serilog;
using System.Reflection;

namespace Aspire.Core.Extensions
{
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var basePath = AppContext.BaseDirectory;
            //builder.RegisterType<AdvertisementServices>().As<IAdvertisementServices>();


            #region 带有接口层的服务注入

            var servicesDllFile = Path.Combine(basePath, "Aspire.Core.Services.dll");
            var repositoryDllFile = Path.Combine(basePath, "Aspire.Core.Repository.dll");

            if (!(File.Exists(servicesDllFile) && File.Exists(repositoryDllFile)))
            {
                var msg = "Repository.dll和service.dll 丢失，因为项目解耦了，所以需要先F6编译，再F5运行，请检查 bin 文件夹，并拷贝。";
                Log.Error(msg);
                throw new Exception(msg);
            }


            // AOP 开关，如果想要打开指定的功能，只需要在 appsettigns.json 对应对应 true 就行。
            var cacheType = new List<Type>();
            if (AppSettings.app(new string[] { "AppSettings", "CachingAOP", "Enabled" }).ObjToBool())
            {
                builder.RegisterType<AspireCacheAOP>();
                cacheType.Add(typeof(AspireCacheAOP));
            }

            if (AppSettings.app(new string[] { "AppSettings", "TranAOP", "Enabled" }).ObjToBool())
            {
                builder.RegisterType<AspireTranAOP>();
                cacheType.Add(typeof(AspireTranAOP));
            }

            if (AppSettings.app(new string[] { "AppSettings", "LogAOP", "Enabled" }).ObjToBool())
            {
                builder.RegisterType<AspireLogAOP>();
                cacheType.Add(typeof(AspireLogAOP));
            }

            if (AppSettings.app(new string[] { "AppSettings", "UserAuditAOP", "Enabled" }).ObjToBool())
            {
                builder.RegisterType<AspireUserAuditAOP>();
                cacheType.Add(typeof(AspireUserAuditAOP));
            }

            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerDependency(); //注册仓储
            builder.RegisterGeneric(typeof(BaseServices<>)).As(typeof(IBaseServices<>)).InstancePerDependency();     //注册服务

            // 获取 Service.dll 程序集服务，并注册
            var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            builder.RegisterAssemblyTypes(assemblysServices)
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .PropertiesAutowired()
                .EnableInterfaceInterceptors()       //引用Autofac.Extras.DynamicProxy;
                .InterceptedBy(cacheType.ToArray()); //允许将拦截器服务的列表分配给注册。

            // 获取 Repository.dll 程序集服务，并注册
            var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            builder.RegisterAssemblyTypes(assemblysRepository)
                .AsImplementedInterfaces()
                .PropertiesAutowired()
                .InstancePerDependency();

            builder.RegisterType<UnitOfWorkManage>().As<IUnitOfWorkManage>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();

            #endregion

            #region 没有接口层的服务层注入

            //因为没有接口层，所以不能实现解耦，只能用 Load 方法。
            //注意如果使用没有接口的服务，并想对其使用 AOP 拦截，就必须设置为虚方法
            //var assemblysServicesNoInterfaces = Assembly.Load("Aspire.Core.Services");
            //builder.RegisterAssemblyTypes(assemblysServicesNoInterfaces);

            #endregion

            #region 没有接口的单独类，启用class代理拦截

            //只能注入该类中的虚方法，且必须是public
            //这里仅仅是一个单独类无接口测试，不用过多追问
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(Love)))
                .EnableClassInterceptors()
                .InterceptedBy(cacheType.ToArray());

            #endregion

            #region 单独注册一个含有接口的类，启用interface代理拦截

            //不用虚方法
            //builder.RegisterType<AopService>().As<IAopService>()
            //   .AsImplementedInterfaces()
            //   .EnableInterfaceInterceptors()
            //   .InterceptedBy(typeof(AspireCacheAOP));

            #endregion
        }
    }
}