﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using lifebook.core.cqrses.Domains;
using lifebook.core.projection.Attributes;
using lifebook.core.projection.Domain;
using lifebook.core.projection.Hosting;
using lifebook.core.projection.Services;
using Newtonsoft.Json.Linq;

namespace lifebook.core.projection.sampleapp
{
    public class PersonEntity : EntityProjection
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Occupation { get; set; }
        public Guid ProductId { get; set; }
    }

    // test.primary.TestPerson
    [StreamCategory("TestPerson", "test", "primary")]
    [StreamCategory("TestPerson2", "test", "primary")]
    public class PersonProjector : Projector<PersonEntity>
    {
        public PersonProjector(ProjectorServices projectorServices) : base(projectorServices)
        {
        }

        [UponEvent("TestPersonCreated")]
        public void PersonCreated(AggregateEvent aggregateEvent)
        {
            var personCreated = aggregateEvent.Data.TransformDataFromString(str => JObject.Parse(str));
            Value.FirstName = personCreated["FirstName"].ToObject<string>();
            Value.LastName = personCreated["LastName"].ToObject<string>();
            Value.Age = personCreated["Age"].ToObject<int>();
            Value.Occupation = personCreated["Occupation"].ToObject<string>();
        }

        [UponEvent("TestPersonAgeChanged")]
        public void TestPersonAgeChanged(AggregateEvent aggregateEvent)
        {
            var personCreated = aggregateEvent.Data.TransformDataFromString(str => JObject.Parse(str));
            Value.Age = personCreated["Age"].ToObject<int>();
        }

        [UponEvent("TestPersonOccupationChanged")]
        public void TestPersonOccupationChanged(AggregateEvent aggregateEvent)
        {
            var personCreated = aggregateEvent.Data.TransformDataFromString(str => JObject.Parse(str));
            Value.Occupation = personCreated["Occupation"].ToObject<string>();
        }

        [UponEvent("TestPersonNameChanged")]
        public void TestPersonNameChanged(AggregateEvent aggregateEvent)
        {
            var personCreated = aggregateEvent.Data.TransformDataFromString(str => JObject.Parse(str));
            Value.FirstName = personCreated["FirstName"].ToObject<string>();
            Value.LastName = personCreated["LastName"].ToObject<string>();
        }

        [UponEvent("TestProductPurchased")]
        public void TestProductPurchased(AggregateEvent aggregateEvent)
        {
            var personCreated = aggregateEvent.Data.TransformDataFromString(str => JObject.Parse(str));
            Value.ProductId = personCreated["ProductId"].ToObject<Guid>();
        }
    }

    public class PersonGuidToOccupation : EntityProjection
    {
        public string Occupation { get; set; }
    }

    // test.primary.TestPerson
    [StreamCategory("TestPerson", "test", "primary")]
    [StreamCategory("TestPerson2", "test", "primary")]
    public class PersonGuidToOccupationProjector : Projector<PersonGuidToOccupation>
    {
        public PersonGuidToOccupationProjector(ProjectorServices projectorServices) : base(projectorServices)
        {
        }

        [UponEvent("TestPersonCreated")]
        public void PersonCreated(AggregateEvent aggregateEvent)
        {
            var personCreated = aggregateEvent.Data.TransformDataFromString(str => JObject.Parse(str));
            Value.Occupation = personCreated["Occupation"].ToObject<string>();
        }

        [UponEvent("TestPersonOccupationChanged")]
        public void TestPersonOccupationChanged(AggregateEvent aggregateEvent)
        {
            var personCreated = aggregateEvent.Data.TransformDataFromString(str => JObject.Parse(str));
            Value.Occupation = personCreated["Occupation"].ToObject<string>();
        }
    }

    public class PersonGuidToName : EntityProjection
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    // test.primary.TestPerson
    [StreamCategory("TestPerson", "test", "primary")]
    [StreamCategory("TestPerson2", "test", "primary")]
    public class PersonGuidToNameProjector : Projector<PersonGuidToName>
    {
        public PersonGuidToNameProjector(ProjectorServices projectorServices) : base(projectorServices)
        {
        }

        [UponEvent("TestPersonCreated")]
        public void PersonCreated(AggregateEvent aggregateEvent)
        {
            var personCreated = aggregateEvent.Data.TransformDataFromString(str => JObject.Parse(str));
            Value.FirstName = personCreated["FirstName"].ToObject<string>();
            Value.LastName = personCreated["LastName"].ToObject<string>();
        }

        [UponEvent("TestPersonNameChanged")]
        public void TestPersonNameChanged(AggregateEvent aggregateEvent)
        {
            var personCreated = aggregateEvent.Data.TransformDataFromString(str => JObject.Parse(str));
            Value.FirstName = personCreated["FirstName"].ToObject<string>();
            Value.LastName = personCreated["LastName"].ToObject<string>();
        }
    }

    public class CatalogProjection : EntityProjection
    {
        public string ProductName { get; set; }
    }

    // test.primary.TestPerson
    [StreamCategory("Catalog", "test", "primary")]
    public class CatalogProjector : Projector<CatalogProjection>
    {
        public CatalogProjector(ProjectorServices projectorServices) : base(projectorServices)
        {
        }

        [UponEvent("TestProductCreated")]
        public void PersonCreated(AggregateEvent aggregateEvent)
        {
            var personCreated = aggregateEvent.Data.TransformDataFromString(str => JObject.Parse(str));
            Value.ProductName = personCreated["ProductName"].ToObject<string>();
        }
    }

    public class Program
    {
        static async Task Main(string[] args)
        {
            // Hosting
            IWindsorContainer container = new WindsorContainer();
            await ProjectorHosting.Run(container);

            container.Register(Component.For<PersonServiceApi>().ImplementedBy<PersonServiceApi>());

            try
            {
                var api = container.Resolve<PersonServiceApi>();
                var result = await api.GetPeople();
                Console.WriteLine(result);
                result = await api.GetJAmes();
                Console.WriteLine("=============/ JAMES /=================");
                Console.WriteLine(result);
                result = await api.GetProductPeoplePurchased();
                Console.WriteLine("=============/ Product Person Purchased /=================");
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }
    }

    public class PersonServiceApi
    {
        private readonly PersonGuidToNameProjector personGuidToNameProjector;
        private readonly PersonProjector personProjector;
        private readonly PersonGuidToOccupationProjector personGuidToOccupationProjector;
        private readonly CatalogProjector catalogProjector;

        public PersonServiceApi(PersonGuidToNameProjector personGuidToNameProjector,
            PersonProjector personProjector,
            PersonGuidToOccupationProjector personGuidToOccupationProjector,
            CatalogProjector catalogProjector)
        {
            this.personGuidToNameProjector = personGuidToNameProjector;
            this.personProjector = personProjector;
            this.personGuidToOccupationProjector = personGuidToOccupationProjector;
            this.catalogProjector = catalogProjector;
        }

        public async Task<JArray> GetPeople()
        {
            var result = await personProjector.Query(async e => e.ToList());
            return JArray.FromObject(result);
        }

        public async Task<JArray> GetJAmes()
        {
            var ppname = await personGuidToNameProjector.Query(async e => e.ToList());
            var result = await personGuidToOccupationProjector.Query(async e => {
                var occupations = e.AsQueryable();
                return ppname.Join(occupations, p => p.Key, pp => pp.Key, (p, pp) => new { BothEqual = p.Key == pp.Key, Key = p.Key, Name = $"{p.LastName}, {p.FirstName}", Occupation = pp.Occupation });
            });
            return JArray.FromObject(result);
        }

        public async Task<JArray> GetProductPeoplePurchased()
        {
            var people = await personProjector.Query(async e => e.ToList());
            var result = await catalogProjector.Query(async e => {
                var catalog = e.AsQueryable();
                var query = from person in people
                            join product in catalog
                            on person.ProductId equals product.Key
                            select new { PersonId = person.Key, ProductId = product.Key, ProductName = product.ProductName, Name = $"{person.FirstName} {person.LastName}", Message = $"{product.ProductName} purchased by [{person.LastName}, {person.FirstName}]" };
                 return query.ToList();
            });
            return JArray.FromObject(result);
        }
    }
}
