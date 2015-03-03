using System;
using System.Linq;
namespace Joe.Security
{
    public interface ISecurity<TModel> : ISecurity
    {
        void SetCrud<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, bool forList);
        void SetCrudReflection<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, bool forList);
        IQueryable<TModel> SecureList(IQueryable<TModel> list);
        Boolean HasReadRights<TViewModel>();
        Boolean CanCreate<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, bool forList);
        Boolean CanRead<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, bool forList);
        Boolean CanUpdate<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, bool forList);
        Boolean CanDelete<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, bool forList);
    }
}
