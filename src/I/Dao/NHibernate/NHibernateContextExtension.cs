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
using System.Collections;
using System.Reflection;
using System.ServiceModel;
using System.Text;

using NHibernate;

namespace PPWCode.Vernacular.Persistence.I.Dao.NHibernate
{
    public class NHibernateContextExtension
        : IExtension<InstanceContext>
    {
        public NHibernateContextExtension(ISession session)
        {
            Session = session;
        }

        public ISession Session { get; private set; }

        void IExtension<InstanceContext>.Attach(InstanceContext owner)
        {
        }

        void IExtension<InstanceContext>.Detach(InstanceContext owner)
        {
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(base.ToString());
            {
                sb.Append("{ ");
                sb.AppendFormat("HashCode = '{0}'", GetHashCode());

                foreach (PropertyInfo prop in GetType().GetProperties())
                {
                    sb.AppendFormat(", {0} = ", prop.Name);
                    object value;
                    try
                    {
                        value = prop.GetValue(this, null);
                    }
                    catch (Exception e)
                    {
                        value = e.GetBaseException().Message;
                    }

                    if (value == null)
                    {
                        sb.Append("[null]");
                    }
                    else if (value is string)
                    {
                        sb.AppendFormat("'{0}'", value);
                    }
                    else if (value is IEnumerable)
                    {
                        if (value is ICollection)
                        {
                            sb.AppendFormat("[{0} elements]", ((ICollection)value).Count);
                        }
                        else
                        {
                            sb.AppendFormat("[? elements]");
                        }
                    }
                    else
                    {
                        sb.AppendFormat("'{0}'", value);
                    }
                }

                sb.Append(" }");
            }

            return sb.ToString();
        }
    }
}