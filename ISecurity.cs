using System;
namespace Joe.Security
{
    public interface ISecurity<TModel> : ISecurity
    {
        void SetCrud<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, Boolean listMode = false);
        void SetCrudReflection<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, Boolean listMode = false);
        Boolean CanCreate<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, Boolean listMode = false);
        Boolean CanRead<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, Boolean listMode = false);
        Boolean CanUpdate<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, Boolean listMode = false);
        Boolean CanDelete<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, Boolean listMode = false);
    }
}
