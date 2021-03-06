﻿// Copyright 2012-2016 Chris Patterson
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace GreenPipes
{
    using System;
    using System.Threading;
    using Introspection;


    public static class IntrospectionExtensions
    {
        public static ProbeResult GetProbeResult(this IProbeSite probeSite, CancellationToken cancellationToken = default(CancellationToken))
        {
            var builder = new ProbeResultBuilder(Guid.NewGuid(), cancellationToken);

            probeSite.Probe(builder);

            return ((IProbeResultBuilder)builder).Build();
        }
    }
}