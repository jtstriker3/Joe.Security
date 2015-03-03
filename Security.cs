using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Joe.Reflection;

namespace Joe.Security
{
    public class Security : ISecurity
    {
        protected internal static ISecurityProvider _provider { get; set; }
        public static ISecurityProvider Provider
        {
            get
            {
                _provider = _provider ?? SecurityProviderFactory.Instance.CreateSecurityProviderByLookup();
                return _provider;
            }
            set
            {
                _provider = value;
            }
        }
        public ISecurityProvider ProviderInstance
        {
            get
            {
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

        public virtual void SetCrud<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, bool forList)
        {
            ((ICrud)viewModel).CanCreate = this.CanCreate(getModel, viewModel, forList);
            ((ICrud)viewModel).CanRead = this.CanRead(getModel, viewModel, forList);
            ((ICrud)viewModel).CanUpdate = this.CanUpdate(getModel, viewModel, forList);
            ((ICrud)viewModel).CanDelete = this.CanDelete(getModel, viewModel, forList);
        }

        public virtual void SetCrudReflection<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, bool forList)
        {
            if (ReflectionHelper.TryGetEvalPropertyInfo(typeof(TViewModel), "CanCreate") != null)
                ReflectionHelper.SetEvalProperty(viewModel, "CanCreate", this.CanCreate(getModel, viewModel, forList));
            if (ReflectionHelper.TryGetEvalPropertyInfo(typeof(TViewModel), "CanRead") != null)
                ReflectionHelper.SetEvalProperty(viewModel, "CanRead", this.CanRead(getModel, viewModel, forList));
            if (ReflectionHelper.TryGetEvalPropertyInfo(typeof(TViewModel), "CanUpdate") != null)
                ReflectionHelper.SetEvalProperty(viewModel, "CanUpdate", this.CanUpdate(getModel, viewModel, forList));
            if (ReflectionHelper.TryGetEvalPropertyInfo(typeof(TViewModel), "CanDelete") != null)
                ReflectionHelper.SetEvalProperty(viewModel, "CanDelete", this.CanDelete(getModel, viewModel, forList));
        }

        public Boolean CanCreate<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, bool forList)
        {
            SetRoles<TViewModel>();
            if (Roles != null)
                return (AllRoles || Provider.IsUserInRole(Roles.GetCreateRolesArray()) || OrCreateRules(getModel, viewModel, forList)) && AndCreateRules(getModel, viewModel, forList);
            else
                return AndCreateRules(getModel, viewModel, forList);
        }

        public Boolean CanRead<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, bool forList)
        {
            SetRoles<TViewModel>();
            if (Roles != null)
                return (AllRoles || Provider.IsUserInRole(Roles.GetReadRolesArray()) || OrReadRules(getModel, viewModel, forList)) && AndReadRules(getModel, viewModel, forList);
            else
                return AndReadRules(getModel, viewModel, forList);
        }

        public Boolean CanUpdate<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, bool forList)
        {
            SetRoles<TViewModel>();
            if (Roles != null)
                return (AllRoles || Provider.IsUserInRole(Roles.GetUpdateRolesArray()) || OrUpdateRules(getModel, viewModel, forList)) && AndUpdateRules(getModel, viewModel, forList);
            else
                return AndUpdateRules(getModel, viewModel, forList);
        }

        public Boolean CanDelete<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, bool forList)
        {
            SetRoles<TViewModel>();
            if (Roles != null)
                return (AllRoles || Provider.IsUserInRole(Roles.GetDeleteRolesArray()) || OrDeleteRules(getModel, viewModel, forList)) && AndDeleteRules(getModel, viewModel, forList);
            else
                return AndDeleteRules(getModel, viewModel, forList);
        }

        public IQueryable<TModel> SecureList(IQueryable<TModel> list)
        {
            return list;
        }

        /// <summary>
        /// This Will Verify The user has the Appropreate Role to Read If Roles Are Set It will not evaluate based of individual ViewModel
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <returns></returns>
        public Boolean HasReadRights<TViewModel>()
        {
            SetRoles<TViewModel>();
            if (Roles != null)
                return (AllRoles || Provider.IsUserInRole(Roles.GetReadRolesArray()));

            return DefaultRead;
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

        protected virtual Boolean OrCreateRules<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, bool forList)
        {
            return false;
        }

        protected virtual Boolean OrReadRules<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, bool forList)
        {
            return false;
        }

        protected virtual Boolean OrUpdateRules<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, bool forList)
        {
            return false;
        }

        protected virtual Boolean OrDeleteRules<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, bool forList)
        {
            return false;
        }

        protected virtual Boolean AndCreateRules<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, bool forList)
        {
            return DefaultCreate;
        }

        protected virtual Boolean AndReadRules<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, bool forList)
        {
            return DefaultRead;
        }

        protected virtual Boolean AndUpdateRules<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, bool forList)
        {
            return DefaultUpdate;
        }

        protected virtual Boolean AndDeleteRules<TViewModel>(Func<TViewModel, TModel> getModel, TViewModel viewModel, bool forList)
        {
            return DefaultDelete;
        }

        protected virtual void SetRoles<TViewModel>()
        {
            Roles = (SecurityAttribute)typeof(TViewModel).GetCustomAttributes(typeof(SecurityAttribute), true).SingleOrDefault();
        }

    }
}
