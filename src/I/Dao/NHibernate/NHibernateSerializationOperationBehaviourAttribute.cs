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
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace PPWCode.Vernacular.Persistence.I.Dao.NHibernate
{
    [AttributeUsage(AttributeTargets.Method)]
    public class NHibernateSerializationOperationBehaviourAttribute
        : Attribute,
          IOperationBehavior
    {
        private static void ApplyDataContractSurrogate(OperationDescription operationDescription)
        {
            DataContractSerializerOperationBehavior dataContractBehavior = operationDescription
                .Behaviors
                .Find<DataContractSerializerOperationBehavior>();
            if (dataContractBehavior != null)
            {
                dataContractBehavior.DataContractSurrogate = new NHibernateDataContractSurrogate();
            }
        }

        void IOperationBehavior.Validate(OperationDescription operationDescription)
        {
        }

        void IOperationBehavior.ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            ApplyDataContractSurrogate(operationDescription);
        }

        void IOperationBehavior.ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            ApplyDataContractSurrogate(operationDescription);
        }

        void IOperationBehavior.AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }
    }
}