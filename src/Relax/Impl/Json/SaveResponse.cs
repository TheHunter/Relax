// /* 
// Copyright 2008-2011 Alex Robson
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// */
using Newtonsoft.Json;

namespace Relax.Impl.Json
{
    public class SaveResponse
    {
        [JsonProperty( PropertyName = "ok" )]
        public bool Success { get; set; }

        [JsonProperty( PropertyName = "id" )]
        public string Id { get; set; }

        [JsonProperty( PropertyName = "rev" )]
        public string Revision { get; set; }
    }
}