﻿// Copyright 2014 by PeopleWare n.v..
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
using System.Runtime.Serialization;

namespace PPWCode.Vernacular.Persistence.II
{
    [Serializable, DataContract(IsReference = true)]
    public abstract class InsertAuditableVersionedPersistentObject<T, TVersion>
        : VersionedPersistentObject<T, TVersion>,
          IInsertAuditable
        where T : IEquatable<T>
        where TVersion : IEquatable<TVersion>
    {
        [DataMember(Name = "CreatedAt")]
        private DateTime? m_CreatedAt;

        [DataMember(Name = "CreatedBy")]
        private string m_CreatedBy;

        protected InsertAuditableVersionedPersistentObject(T id, TVersion persistenceVersion)
            : base(id, persistenceVersion)
        {
        }

        protected InsertAuditableVersionedPersistentObject(T id)
            : base(id)
        {
        }

        protected InsertAuditableVersionedPersistentObject()
        {
        }

        [AuditLogPropertyIgnore]
        public virtual DateTime? CreatedAt
        {
            get { return m_CreatedAt; }
            set { m_CreatedAt = value; }
        }

        [AuditLogPropertyIgnore]
        public virtual string CreatedBy
        {
            get { return m_CreatedBy; }
            set { m_CreatedBy = value; }
        }
    }
}