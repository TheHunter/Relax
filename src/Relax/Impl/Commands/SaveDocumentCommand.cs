﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Relax.Impl.Configuration;
using Relax.Impl.Http;
using Relax.Impl.Json;
using Symbiote.Core.Extensions;

namespace Relax.Impl.Commands
{
    public class SaveDocumentCommand : BaseCouchCommand
    {
        public virtual CommandResult Save<TModel>(TModel model)
        {
            try
            {
                CreateUri<TModel>()
                    .BulkInsert();

                var documents = model.GetDocmentsFromGraph();
                return SaveAll(documents);
            }
            catch (Exception ex)
            {
                throw Exception(
                    ex,
                    "An exception occurred trying to save a document of type {0} at {1}. \r\n\t {2}",
                    typeof(TModel).FullName,
                    Uri.ToString(),
                    ex
                    );
            }
        }

        public virtual CommandResult SaveAll<TModel>(IEnumerable<TModel> models)
        {
            CreateUri<TModel>()
                .BulkInsert();

            var documents = models.GetDocmentsFromGraph();
            return SaveAll(documents);
        }

        public virtual CommandResult SaveAll(string database, IEnumerable<object> models)
        {
            CreateUri(database)
                .BulkInsert();

            var documents = models.GetDocmentsFromGraph();
            return SaveAll(documents);
        }

        protected CommandResult SaveAll(IEnumerable<object> models)
        {
            try
            {
                var list = new BulkPersist(true, false, models);
                var body = list.ToJson(configuration.IncludeTypeSpecification);
                body = ScrubBulkPersistOfTypeTokens(body);

                var result = Post(body);
                var updates = result.GetResultAs<SaveResponse[]>();
                models
                    .ForEach(x =>
                                 {
                                     var update = updates.FirstOrDefault(u => u.GetDocumentId().Equals(x.GetDocumentId()));
                                     if(update != null)
                                         x.SetDocumentRevision(update.Revision);
                                 });
                return result;
            }
            catch (Exception ex)
            {
                throw Exception(
                        ex,
                        "An exception occurred trying to save a collection documents at {0}. \r\n\t {1}",
                        Uri.ToString(),
                        ex
                    );
            }
        }

        public virtual string ScrubBulkPersistOfTypeTokens(string body)
        {
            var jBlob = JObject.Parse(body);
            var docs = jBlob["docs"]["$values"];
            jBlob.Property("docs").Value = docs;
            return jBlob.ToString();
        }

        public SaveDocumentCommand(IHttpAction action, ICouchConfiguration configuration) : base(action, configuration)
        {
        }
    }
}