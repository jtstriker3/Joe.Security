using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Joe.Reflection;

namespace Joe.Security
{
    public class Security : ISecurity
    {
        public static ISecurityProvider Provider { get; set; }
        public ISecurityProvider ProviderInstance
        {
            get
            {
                Provider = Provider ?? SecurityProviderFactory.Instance.CreateSecurityProviderByLookup();
                return Provider;
            }
        }
    }

    public class Security<TModel> : Security, ISecurity<TModel>
    {
        protected ISecurityRoles Roles { get; set; }
        public Boolean DefaultCreate { get; set; }
        public Boolean DefaultUpdate { get; set; }
        public Boolean DefaultRead { get; set; }
        public Boolean DefaultDelete { get; set; }

        public Security()
        {
            if (ProviderInstance == null)
                throw new Exception("You must set the ISecurityProvider of the Security Object.");

            DefaultCreate = true;
            DefaultUpdate = true;
            DefaultRead = true;
            DefaultDelete = true;
        }

        public virtual void SetCrud<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, Boolean listMode = false)
        {
            ((ICrud)viewModel).CanCreate = this.CanCreate(getModel, viewModel, listMode);
            ((ICrud)viewModel).CanRead = this.CanRead(getModel, viewModel, listMode);
            ((ICrud)viewModel).CanUpdate = this.CanUpdate(getModel, viewModel, listMode);
            ((ICrud)viewModel).CanDelete = this.CanDelete(getModel, viewModel, listMode);
        }

        public virtual void SetCrudReflection<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, Boolean listMode = false)
        {
            ReflectionHelper.SetEvalProperty(viewModel, "CanCreate", this.CanCreate(getModel, viewModel, listMode));
            ReflectionHelper.SetEvalProperty(viewModel, "CanRead", this.CanRead(getModel, viewModel, listMode));
            ReflectionHelper.SetEvalProperty(viewModel, "CanUpdate", this.CanUpdate(getModel, viewModel, listMode));
            ReflectionHelper.SetEvalProperty(viewModel, "CanDelete", this.CanDelete(getModel, viewModel, listMode));
        }

        public Boolean CanCreate<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, Boolean listMode = false)
        {
            SetRoles<TViewModel>();
            if (Roles != null)
                return (AllRoles || Provider.IsUserInRole(Roles.GetCreateRolesArray()) || OrCreateRules(getModel, viewModel, listMode)) && AndCreateRules(getModel, viewModel, listMode);
            return DefaultCreate;
        }

        public Boolean CanRead<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, Boolean listMode = false)
        {
            SetRoles<TViewModel>();
            if (Roles != null)
                return (AllRoles || Provider.IsUserInRole(Roles.GetReadRolesArray()) || OrReadRules(getModel, viewModel, listMode)) && AndReadRules(getModel, viewModel, listMode);
            return DefaultRead;
        }

        public Boolean CanUpdate<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, Boolean listMode = false)
        {
            SetRoles<TViewModel>();
            if (Roles != null)
                return (AllRoles || Provider.IsUserInRole(Roles.GetUpdateRolesArray()) || OrUpdateRules(getModel, viewModel, listMode)) && AndUpdateRules(getModel, viewModel, listMode);
            return DefaultUpdate;
        }

        public Boolean CanDelete<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, Boolean listMode = false)
        {
            SetRoles<TViewModel>();
            if (Roles != null)
                return (AllRoles || Provider.IsUserInRole(Roles.GetDeleteRolesArray()) || OrDeleteRules(getModel, viewModel, listMode)) && AndDeleteRules(getModel, viewModel, listMode);
            return DefaultDelete;
        }

        private Boolean? _allRoles;
        protected Boolean AllRoles
        {
            get
            {
                _allRoles = _allRoles ?? Provider.IsUserInRole(Roles.GetAllRolesArray());
                return _allRoles.Value;
            }
        }

        protected virtual Boolean OrCreateRules<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, Boolean listMode = false)
        {
            return false;
        }

        protected virtual Boolean OrReadRules<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, Boolean listMode = false)
        {
            return false;
        }

        protected virtual Boolean OrUpdateRules<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, Boolean listMode = false)
        {
            return false;
        }

        protected virtual Boolean OrDeleteRules<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, Boolean listMode = false)
        {
            return false;
        }

        protected virtual Boolean AndCreateRules<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, Boolean listMode = false)
        {
            return true;
        }

        protected virtual Boolean AndReadRules<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, Boolean listMode = false)
        {
            return true;
        }

        protected virtual Boolean AndUpdateRules<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, Boolean listMode = false)
        {
            return true;
        }

        protected virtual Boolean AndDeleteRules<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, Boolean listMode = false)
        {
            return true;
        }

        protected virtual void SetRoles<TViewModel>()
        {
            Roles = (SecurityAttribute)typeof(TViewModel).GetCustomAttributes(typeof(SecurityAttribute), true).SingleOrDefault();
        }

    }
}
