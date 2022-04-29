﻿using Autofac;

using Expense.Infrastructure.Repository;

using Expenses.Core.Services;

namespace Expenses.App
{
    public class AppService
    {
        private static IContainer Container { get; set; }

        static AppService()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<ItemRepository>().As<IItemRepository>();
            builder.RegisterType<ExpenseRepository>().As<IExpenseRepository>();

            Container = builder.Build();
        }

        public static IItemRepository Item
        {
            get { return Container.Resolve<IItemRepository>(); }
        }

        public static IExpenseRepository Expense
        {
            get { return Container.Resolve<IExpenseRepository>(); }
        }
    }
}