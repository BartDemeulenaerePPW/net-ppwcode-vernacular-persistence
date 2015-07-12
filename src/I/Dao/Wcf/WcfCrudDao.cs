﻿// Copyright 2010-2015 by PeopleWare n.v..
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

using log4net;

using PPWCode.Vernacular.Exceptions.I;

namespace PPWCode.Vernacular.Persistence.I.Dao.Wcf
{
    public abstract class WcfCrudDao :
        IWcfCrudDao
    {
        private static readonly ILog s_Logger = LogManager.GetLogger(typeof(WcfCrudDao));

        public IStatelessCrudDao StatelessCrudDao { get; protected set; }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public IPersistentObject Retrieve(string persistentObjectType, long? id)
        {
            CheckObjectAlreadyDisposed();

            Type poType = Type.GetType(persistentObjectType);
            IPersistentObject result = StatelessCrudDao.Retrieve<IPersistentObject>(poType, id);
            return result;
        }

        public T Retrieve<T>(long? id)
            where T : class, IPersistentObject
        {
            CheckObjectAlreadyDisposed();

            T result = StatelessCrudDao.Retrieve<T>(typeof(T), id);
            return result;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public ICollection<IPersistentObject> RetrieveAll(string persistentObjectType)
        {
            CheckObjectAlreadyDisposed();

            Type poType = Type.GetType(persistentObjectType);
            ICollection<IPersistentObject> result = StatelessCrudDao.RetrieveAll<IPersistentObject>(poType);
            return result;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public virtual IPersistentObject Create(IPersistentObject po)
        {
            CheckObjectAlreadyDisposed();

            IPersistentObject result = StatelessCrudDao.Create(po);
            return result;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public virtual IPersistentObject Update(IPersistentObject po)
        {
            CheckObjectAlreadyDisposed();

            IPersistentObject result = StatelessCrudDao.Update(po);
            return result;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public virtual ICollection<IPersistentObject> UpdateAll(ICollection<IPersistentObject> col)
        {
            CheckObjectAlreadyDisposed();

            return col.Select(o => StatelessCrudDao.Update(o)).ToList();
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public virtual IPersistentObject Delete(IPersistentObject po)
        {
            CheckObjectAlreadyDisposed();

            IPersistentObject result = StatelessCrudDao.Delete(po);
            return result;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public IPersistentObject DeleteById(string persistentObjectType, long? id)
        {
            return Delete(Retrieve(persistentObjectType, id));
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public object GetPropertyValue(IPersistentObject po, string propertyName)
        {
            CheckObjectAlreadyDisposed();

            object result = StatelessCrudDao.GetPropertyValue<IPersistentObject, object>(po, propertyName);
            return result;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public ICollection<IPersistentObject> GetChildren(IPersistentObject po, string property)
        {
            CheckObjectAlreadyDisposed();

            ICollection<IPersistentObject> result = StatelessCrudDao.GetChildren<IPersistentObject, IPersistentObject>(po, property);
            return result;
        }

        public abstract void FlushAllCaches();

        protected void DoFlush()
        {
            if (StatelessCrudDao.IsFlushable())
            {
                StatelessCrudDao.DoFlush();
            }
        }

        public bool IsOperational
        {
            get
            {
                return StatelessCrudDao != null
                       && StatelessCrudDao.IsOperational;
            }
        }

        private readonly object m_Locker = new object();
        private bool m_Disposed;

        ~WcfCrudDao()
        {
            if (m_Locker != null)
            {
                lock (m_Locker)
                {
                    SafeCleanup();
                }
            }
        }

        protected bool Disposed
        {
            get
            {
                lock (m_Locker)
                {
                    return m_Disposed;
                }
            }
        }

        public void Dispose()
        {
            lock (m_Locker)
            {
                if (!m_Disposed)
                {
                    SafeCleanup();
                    m_Disposed = true;
                    GC.SuppressFinalize(this);
                }
            }
        }

        private void SafeCleanup()
        {
            try
            {
                Cleanup();
            }
            catch (Exception e)
            {
                s_Logger.Error(e);
            }
        }

        protected virtual void Cleanup()
        {
            if (IsOperational)
            {
                StatelessCrudDao.Dispose();
                StatelessCrudDao = null;
            }
        }

        protected void CheckObjectAlreadyDisposed()
        {
            if (Disposed)
            {
                throw new ObjectAlreadyDisposedError(GetType().FullName);
            }
        }
    }
}