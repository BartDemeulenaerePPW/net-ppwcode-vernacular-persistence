﻿// Copyright 2010-2016 by PeopleWare n.v..
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

using System.ServiceModel;

using log4net;

using PPWCode.Vernacular.Persistence.I.Dao.Wcf.Helpers.GenericInterceptor;

namespace PPWCode.Vernacular.Persistence.I.Dao.Wcf.Helpers.Errors
{
    public class AuditLogServiceInterceptorBehaviorAttribute
        : ServiceInterceptorBehaviorAttribute
    {
        private static readonly ILog s_Logger = LogManager.GetLogger(typeof(AuditLogServiceInterceptorBehaviorAttribute));
        private readonly AuditLevel m_AuditLevel;

        public AuditLogServiceInterceptorBehaviorAttribute(AuditLevel auditLevel)
        {
            m_AuditLevel = auditLevel;
        }

        protected override OperationInterceptorBehaviorAttribute CreateOperationInterceptor()
        {
            if (s_Logger.IsDebugEnabled)
            {
                s_Logger.DebugFormat(
                    "Requested to create an OperationInterceptor of type {0} with auditLevel {1}.",
                    typeof(AuditLogOperationInterceptorAttribute).Name,
                    m_AuditLevel);
            }

            return new AuditLogOperationInterceptorAttribute(m_AuditLevel);
        }
    }
}