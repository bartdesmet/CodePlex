/*
 * LINQ to Active Directory
 * http://www.codeplex.com/LINQtoAD
 * 
 * Copyright Bart De Smet (C) 2007
 * info@bartdesmet.net - http://blogs.bartdesmet.net/bart
 * 
 * This project is subject to licensing restrictions. Visit http://www.codeplex.com/LINQtoAD/Project/License.aspx for more information.
 */

#region Namespace imports

using System;
using ActiveDs;
using BdsSoft.DirectoryServices.Linq;
using System.DirectoryServices;

#endregion

namespace Demo
{
    sealed class MyContext : DirectoryContext
    {
        public MyContext(DirectoryEntry searchRoot)
            : base(searchRoot)
        {
        }

        [DirectorySearchOptions(SearchScope.Subtree)]
        public DirectorySource<User>  Users  { get; set; }

        [DirectorySearchOptions(SearchScope.Subtree)]
        public DirectorySource<Group> Groups { get; set; }

#if !NOTESTOU
        [DirectorySearchPath("OU=Demo")]
        public MyDemoContext          Demo   { get; set; }
#endif
    }

    sealed class MyDemoContext : DirectoryContext
    {
        public MyDemoContext(DirectoryEntry searchRoot)
            : base(searchRoot)
        {
        }

        public DirectorySource<MyUser> Users  { get; set; }
    }

    [DirectorySchema("user", typeof(IADsUser))]
    class User
    {
        [DirectoryAttribute("objectGUID")]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int LogonCount { get; set; }

        [DirectoryAttribute("PasswordLastChanged", DirectoryAttributeType.ActiveDs)]
        public DateTime PasswordLastSet { get; set; }

        [DirectoryAttribute("distinguishedName")]
        public string Dn { get; set; }

        [DirectoryAttribute("memberOf")]
        public string[] Groups { get; set; }
    }

    [DirectorySchema("group")]
    class Group
    {
        public string Name { get; set; }

        [DirectoryAttribute("member")]
        public string[] Members { get; set; }
    }

    [DirectorySchema("user", typeof(IADsUser))]
    class MyUser : DirectoryEntity
    {
        private DateTime expiration;

        [DirectoryAttribute("AccountExpirationDate", DirectoryAttributeType.ActiveDs)]
        public DateTime AccountExpirationDate
        {
            get { return expiration; }
            set
            {
                if (expiration != value)
                {
                    expiration = value;
                    OnPropertyChanged("AccountExpirationDate");
                }
            }
        }

        private string first;

        [DirectoryAttribute("givenName")]
        public string FirstName
        {
            get { return first; }
            set
            {
                if (first != value)
                {
                    first = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }

        private string last;

        [DirectoryAttribute("sn")]
        public string LastName
        {
            get { return last; }
            set
            {
                if (last != value)
                {
                    last = value;
                    OnPropertyChanged("LastName");
                }
            }
        }

        private string office;

        [DirectoryAttribute("physicalDeliveryOfficeName")]
        public string Office
        {
            get { return office; }
            set
            {
                if (office != value)
                {
                    office = value;
                    OnPropertyChanged("Office");
                }
            }
        }

        private string accountName;

        [DirectoryAttribute("sAMAccountName")]
        public string AccountName
        {
            get { return accountName; }
            set
            {
                if (accountName != value)
                {
                    accountName = value;
                    OnPropertyChanged("AccountName");
                }
            }
        }

        public bool SetPassword(string password)
        {
            return this.DirectoryEntry.Invoke("SetPassword", new object[] { password }) == null;
        }
    }
}
